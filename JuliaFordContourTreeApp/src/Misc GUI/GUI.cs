using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using JuliaFordContourTreeApp.File_Data;
using JuliaFordContourTreeApp.Trees;
using JuliaFordContourTreeApp.Tree_Views;

namespace JuliaFordContourTreeApp
{
    /// <summary>
    /// The GUI class handles all the user interface for the program.
    /// </summary>
    public partial class GUI : Form
    {
		private JoinTreeView joinTreeView;
		private SplitTreeView splitTreeView;
		private ContourTreeView contourTreeView;

		private TopologyChangeView topology;

		private int currentTimeStep;

		private List<FileDataAbstract> fileDataList;

        /// <summary>
        /// Creates a new instance of the GUI.
        /// </summary>
        public GUI()
        {
			// Create the panels that display views.
			this.joinTreeView = new JoinTreeView();
			this.splitTreeView = new SplitTreeView();
			this.contourTreeView = new ContourTreeView();
			this.topology = new TopologyChangeView();

			// Keep track of how many data files have been read.
			this.currentTimeStep = 0;
			this.fileDataList = new List<FileDataAbstract>();

			// Required by C# for GUI stuff.
            InitializeComponent();

			// Create the actual panels and make them display one view each.
			GUIpanel ULpanel = new GUIpanel(new Panel(), contourTreeView, joinTreeView, splitTreeView, topology, joinTreeView);
			GUIpanel URpanel = new GUIpanel(new Panel(), contourTreeView, joinTreeView, splitTreeView, topology, splitTreeView);
			GUIpanel BLpanel = new GUIpanel(new Panel(), contourTreeView, joinTreeView, splitTreeView, topology, contourTreeView);
			GUIpanel BMpanel = new GUIpanel(new Panel(), contourTreeView, joinTreeView, splitTreeView, topology, new Panel());
			GUIpanel BRpanel = new GUIpanel(new Panel(), contourTreeView, joinTreeView, splitTreeView, topology, topology);

			// Add the panels to the GUI.
			this.GUI_layout.Controls.Add(ULpanel);
			this.GUI_layout.Controls.Add(URpanel);
			this.GUI_layout.Controls.Add(BLpanel);
			this.GUI_layout.Controls.Add(BMpanel);
			this.GUI_layout.Controls.Add(BRpanel);

			// Set the spacing to look nice.
			this.GUI_layout.SetColumnSpan(ULpanel, 3);
			this.GUI_layout.SetColumnSpan(URpanel, 3);
			this.GUI_layout.SetColumnSpan(BLpanel, 2);
			this.GUI_layout.SetColumnSpan(BMpanel, 2);
			this.GUI_layout.SetColumnSpan(BRpanel, 2);
        }

        /// <summary>
        /// Add a new data file.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void addNewDataFile(object uselessParam1, EventArgs uselessParam2)
        {
			// If opened properly, the file data will be stored here.
            Stream myStream = null;
			// This is a popup window that lets you pick a file to open.
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // TODO: change starting file directory back to c:\\
            openFileDialog.InitialDirectory = "C:\\Users\\Julia\\Documents\\Visual Studio 2010\\Projects\\JuliaFordContourTreeApp\\JuliaFordContourTreeApp\\data";
            openFileDialog.Filter = "txt files (*.txt)|*.txt"; // Only look at text files.

			// If the file doesn't get opened / read / added properly, display an error message.
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    if ((myStream = openFileDialog.OpenFile()) != null) {
                        readFileData(myStream); } }
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message); }
            }
        }
		
        /// <summary>
        /// Add a new data file with formatting like the data file from Nathaniel.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void addNewNathanielDataFile(object uselessParam1, EventArgs uselessParam2)
        {
			// If opened properly, the file data will be stored here.
            Stream myStream = null;
			// This is a popup window that lets you pick a file to open.
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt"; // Only look at text files.

			// If the file doesn't get opened / read / added properly, display an error message.
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    if ((myStream = openFileDialog.OpenFile()) != null) {
						// Make a new StreamReader to read the text input file.
						StreamReader reader = new StreamReader(myStream);

						// The new file data object will be stored here if successful.
						FileDataAbstract fileData;
						fileData = new FileDataNathaniel(reader.ReadToEnd(), this.currentTimeStep);

						// If there are already other data files, make sure they are compatible.
						if (this.fileDataList.Count > 0)
						{
							FileDataAbstract tempData = this.fileDataList[0];
							if (tempData.Xdim != fileData.Xdim || tempData.Ydim != fileData.Ydim || tempData.Zdim != fileData.Zdim)
							{
								throw new Exception("The file you are attempting to add has different dimensions than the previously added file(s). "
									+ "If you would like to start over with new data, please clear the current data first.");
							}
						}

						// If we've gotten this far without Exception, we can
						// assume the file data will be added successfully.
						this.currentTimeStep++;
						this.fileDataList.Add(fileData);

						// Take the file data and turn it into a list of grid points.
						List<GridPoint> pointsList = fileData.GetSortedGridPointList();

						// If there are already other file(s) in the program, clear out
						// the current trees and add their data to the points list.
						if (this.fileDataList.Count > 1)
						{
							// Clear the trees.
							this.joinTreeView.ClearTrees();
							this.splitTreeView.ClearTrees();
							this.contourTreeView.ClearTrees();
							// Add the data.
							foreach (FileDataAbstract data in this.fileDataList)
							{
								pointsList = pointsList.Union(data.GetSortedGridPointList()).ToList();
								pointsList.Sort(new Comparison<GridPoint>(GridPoint.Compare));
							}
						}

						// Create the join tree and split tree from the file data.
						JoinTree joinTree = new JoinTree(pointsList[0].value, pointsList.Last().value, pointsList);
						SplitTree splitTree = new SplitTree(pointsList[0].value, pointsList.Last().value, pointsList);

						// Simplify the trees, except for the others' critical points.
						joinTree.SimplifyExceptFor(splitTree.GetCritical());
						splitTree.SimplifyExceptFor(joinTree.GetCritical());

						// Make the contour tree.
						ContourTree contourTree = new ContourTree(joinTree, splitTree);

						this.contourTreeView.AddTree(contourTree);
						this.topology.SetTree(contourTree);
						this.topology.AddTimeStep();

						// Simplify trees the rest of the way.
						joinTree.Simplify();
						splitTree.Simplify();

						// Add trees to their views.
						this.joinTreeView.AddTree(joinTree);
						this.splitTreeView.AddTree(splitTree);

						// Dispose of unneeded resources.
						reader.Dispose();
						myStream.Close();
					}
				}
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message); }
            }
        }

        /// <summary>
        /// Figures out what kind of data the file contains and passes it to parser.
        /// </summary>
        /// <param name="myStream">the file to parse</param>
        private void readFileData(Stream myStream)
        {
			// Make a new StreamReader to read the text input file.
            StreamReader reader = new StreamReader(myStream);

			// Check the first line of the file for its file type identifier.
            string fileType = reader.ReadLine().Trim();

			// The new file data object will be stored here if successful.
            FileDataAbstract fileData;

			// Try to read the file data.
            if (fileType == "I0") { // TODO: Finish other data file types.
                fileData = new FileDataI0(reader.ReadToEnd(), this.currentTimeStep); }
			// Throw an exception if the format is not recognized.
            else {
                throw new Exception("The file format was not recognized; only files in 'I0' format work at this time."); }

			// If there are already other data files, make sure they are compatible.
			if (this.fileDataList.Count > 0) {
				FileDataAbstract tempData = this.fileDataList[0];
				if (tempData.Xdim != fileData.Xdim || tempData.Ydim != fileData.Ydim || tempData.Zdim != fileData.Zdim) {
					throw new Exception("The file you are attempting to add has different dimensions than the previously added file(s). "
						+ "If you would like to start over with new data, please clear the current data first."); } }

			// If we've gotten this far without Exception, we can
			// assume the file data will be added successfully.
			this.currentTimeStep++;
			this.fileDataList.Add(fileData);

			// Take the file data and turn it into a list of grid points.
            List<GridPoint> pointsList = fileData.GetSortedGridPointList();

			// If there are already other file(s) in the program, clear out
			// the current trees and add their data to the points list.
			if (this.fileDataList.Count > 1) {
				// Clear the trees.
				this.joinTreeView.ClearTrees();
				this.splitTreeView.ClearTrees();
				this.contourTreeView.ClearTrees();
				// Add the data.
				foreach (FileDataAbstract data in this.fileDataList) {
					pointsList = pointsList.Union(data.GetSortedGridPointList()).ToList();
					pointsList.Sort(new Comparison<GridPoint>(GridPoint.Compare)); } }

			// Create the join tree and split tree from the file data.
             JoinTree  joinTree = new  JoinTree(pointsList[0].value, pointsList.Last().value, pointsList);
			SplitTree splitTree = new SplitTree(pointsList[0].value, pointsList.Last().value, pointsList);

			// Simplify the trees, except for the others' critical points.
			 joinTree.SimplifyExceptFor(splitTree.GetCritical());
			splitTree.SimplifyExceptFor( joinTree.GetCritical());

			// Make the contour tree.
			ContourTree contourTree = new ContourTree(joinTree, splitTree);

			this.contourTreeView.AddTree(contourTree);
			this.topology.SetTree(contourTree);
			this.topology.AddTimeStep();

			// Simplify trees the rest of the way.
			 joinTree.Simplify();
			splitTree.Simplify();

			// Add trees to their views.
			this.joinTreeView.AddTree(joinTree);
			this.splitTreeView.AddTree(splitTree);

			// Dispose of unneeded resources.
			reader.Dispose();
			myStream.Close();
        }

		/// <summary>
		/// Clears all loaded data.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void clearData(object uselessParam1, object uselessParam2)
		{
			this.joinTreeView.ClearTrees();
			this.splitTreeView.ClearTrees();
			this.contourTreeView.ClearTrees();

			this.topology.SetTree(null);

			this.fileDataList = new List<FileDataAbstract>();

			this.currentTimeStep = 0;
			this.topology.ResetTimeSteps();
		}

		/// <summary>
		/// Prints out file data in vtk format.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void printOutVtkData(object uselessParam1, EventArgs uselessParam2)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = "C:\\Users\\Julia\\Documents\\Visual Studio 2010\\Projects\\JuliaFordContourTreeApp\\JuliaFordContourTreeApp\\vtk_data";
			saveFileDialog.Filter = "VTK format (*.vtk)|*.vtk";

			// If created properly, the file will be saved here.
			Stream myStream = null;

			// If the file doesn't get opened / read / added properly, display an error message.
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    if ((myStream = saveFileDialog.OpenFile()) != null) {
						myStream.Close();
						writeVtkData(saveFileDialog.FileName);
					}
				}
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message); }
            }
		}

		/// <summary>
		/// Helper method for printOutVtkData; does the actual writing.
		/// Currently writes out only the first file loaded.
		/// </summary>
		/// <param name="fileName">the name of the file to write to</param>
		private void writeVtkData(string fileName)
		{
			string[] lines = new string[11];
			lines[0] = "# vtk DataFile Version 3.0";
			lines[1] = "File auto-generated by JuliaFordContourTreeApp.";
			lines[2] = "ASCII";
			lines[3] = "DATASET STRUCTURED_POINTS";
			lines[4] = "DIMENSIONS " + fileDataList[0].Xdim + " " + fileDataList[0].Ydim + " " + fileDataList[0].Zdim;
			lines[5] = "ORIGIN 0 0 0";
			lines[6] = "SPACING 1 1 1";
			lines[7] = "POINT_DATA " + (fileDataList[0].Xdim * fileDataList[0].Ydim * fileDataList[0].Zdim);
			lines[8] = "SCALARS volume_scalars char 1";
			lines[9] = "LOOKUP_TABLE default";

			// The following ugly loop is desgined to write every scalar value from the
			// data to the new file, while avoiding an off-by-one error. The scalar 
			// values must be offset by spaces, but this line cannot have leading or
			// trailing spaces. Therefore the first character is a special case.

			// The line is initialized with the first scalar value.
			lines[10] = "" + fileDataList[0].GetValueAt(0, 0, 0); 

			// The following ugly loop is desgined to start x after the first
			// value, but only the first time through the loop.

			// Initialize counters.
			int z = 0;
			int y = 0;
			int x = 1; // x is offset by one

			// Loop through all the scalars.
			for (; z < fileDataList[0].Zdim; z++) {
				for (; y < fileDataList[0].Ydim; y++) {
					for (; x < fileDataList[0].Xdim; x++) {
						lines[10] += " " + fileDataList[0].GetValueAt(x, y, z);
					}
					x = 0; // reset x counter
				}
				y = 0; // reset y counter
			}

			// Write to file.
			File.WriteAllLines(fileName, lines);
		}

		/// <summary>
		/// Prints out data about the contour tree to a text file.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void printOutTreeData(object uselessParam1, EventArgs uselessParam2)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = "C:\\Users\\Julia\\Documents\\Visual Studio 2010\\Projects\\JuliaFordContourTreeApp\\JuliaFordContourTreeApp\\tree_data";
			saveFileDialog.Filter = "text file (*.txt)|*.txt";

			// If created properly, the file will be saved here.
			Stream myStream = null;

			// If the file doesn't get opened / read / added properly, display an error message.
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    if ((myStream = saveFileDialog.OpenFile()) != null) {
						myStream.Close();
						ContourTree tree = this.contourTreeView.GetContourTree();
						File.WriteAllLines(saveFileDialog.FileName, tree.GetStringOutput());
					}
				}
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message); }
            }
		}

    }
}
