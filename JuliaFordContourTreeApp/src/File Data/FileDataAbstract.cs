using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JuliaFordContourTreeApp.File_Data
{
    /// <summary>
    /// This abstract class implements some basic functionailty that is shared
    /// by FileDataS, FileDataS3D, and FileDataI0.
    /// </summary>
    abstract class FileDataAbstract
	{
		public enum FileType {I0, Nathaniel};

		//===================================================================//
		//                            Variables                              //
		//===================================================================//

        /// <summary>
        /// the number of "columns" in the data file
        /// </summary>
        public readonly int Xdim;
        /// <summary>
        /// the number of "rows" in the data file
        /// </summary>
        public readonly int Ydim;
        /// <summary>
        /// the number of "blocks" in the data file
        /// </summary>
        public readonly int Zdim;
		/// <summary>
		/// the time step that this file represents;
		/// if this is a standalone file, this dimension should be 0
		/// </summary>
		public readonly int Tdim;

        /// <summary>
        /// the maximum value in the data file
        /// </summary>
        protected float maxValue;
        /// <summary>
        /// the minimum value in the data file
        /// </summary>
        protected float minValue;

        /// <summary>
        /// The actual values in the file.
        /// </summary>
        protected float[, ,] values;

		//===================================================================//
		//                          Constructors                             //
		//===================================================================//

        /// <summary>
        /// Reads the dimensions of the file data and stores them.
        /// </summary>
        /// <param name="fileData">
        /// the string containing the text of the file
		/// </param>
		/// <param name="timestep">the time coordinate of the file data</param>
		/// <param name="flag">
		/// indicates how the data is formatted so that the dimensions can be
		/// grabbed properly
		/// </param>
        public FileDataAbstract(string fileData, int timestep, FileType flag)
        {
			// Create a new reader to read the string.
            StringReader reader = new StringReader(fileData);

			// Procedure for grabbing dimensions from I0 data:
			if (flag == FileType.I0)
			{
				// The dimensions are stated on the first line.
				string dimens = reader.ReadLine().Trim();
				dimens = dimens.Substring(dimens.IndexOf('=') + 1);

				// Stripped dimensions will be stored in these strings.
				string xString;
				string yString;
				string zString;
				// There is no "tstring" because the files contain no time data.

				// Grab the dimensions.
				xString = dimens.Substring(0, dimens.IndexOf(','));
				dimens = dimens.Substring(dimens.IndexOf('=') + 1);
				if (dimens.Contains(',')) {
					yString = dimens.Substring(0, dimens.IndexOf(','));
					dimens = dimens.Substring(dimens.IndexOf('=') + 1);
					zString = dimens; }
				else {
					yString = dimens;
					zString = null; }

				// Convert the strings into integers.
				this.Xdim = Convert.ToInt32(xString);
				this.Ydim = Convert.ToInt32(yString);
				if (zString != null) { this.Zdim = Convert.ToInt32(zString); }
				else { this.Zdim = 1; }
			}

			// Procedure for grabbing dimensions from Nathaniel data:
			else if (flag == FileType.Nathaniel)
			{
				// File dimensions will be stored here.
				int xDimen = 0;
				int yDimen = 0;
				int zDimen = 0;

				// Find the end of the first line.
				// TODO: Find out what happens if the file contains no newline characters.
				int firstNewlineIndex = fileData.IndexOf('\n');

				// TODO: remove debugging statement.
				Console.WriteLine("First newline is at index " + firstNewlineIndex + ".");

				int currentSpaceIndex = 0;
				int numSpacesRead = 0;
				// Keep reading spaces until the end of the line is reached.
				while (currentSpaceIndex < firstNewlineIndex - 1) {
					currentSpaceIndex = fileData.IndexOf(' ', currentSpaceIndex + 1);
					numSpacesRead++; }
				// The x dimen of the file is the number of values on a line.
				xDimen = numSpacesRead;

				// Find the end of the first block.
				// TODO: Find out what happens if the file does not contain any instances of two adjacent newline characters.
				int firstDoubleNewlineIndex = fileData.IndexOf("\n\r\n");

				// TODO: remove debugging statement.
				Console.WriteLine("Test string is at index " + firstDoubleNewlineIndex + ".");

				int currentNewlineIndex = 0;
				int numNewlinesRead = 0;
				// Note  the lack of a '- 1' at the end; this is because the
				// first newline character should be counted, but not the second.
				while (currentNewlineIndex < firstDoubleNewlineIndex) {
					currentNewlineIndex = fileData.IndexOf('\n', currentNewlineIndex + 1);
					numNewlinesRead++; }
				// The y dimen of the file is the number of lines in a block.
				yDimen = numNewlinesRead;

				// Find the end of the file.
				// TODO: Find out what happens if the file does not contain any instances of two adjacent newline characters.
				int eofIndex = fileData.Length;
				int currentDoubleNewlineIndex = 0;
				int numDoubleNewlinesRead = 0;
				// The '- 4' at the end is so the program doesn't try to read past the end of the string.
				while (currentDoubleNewlineIndex < eofIndex - 4) {
					currentDoubleNewlineIndex = fileData.IndexOf("\n\r\n", currentDoubleNewlineIndex + 1);
					numDoubleNewlinesRead++; }
				// The z dimen of the file is the number of blocks in the file.
				zDimen = numDoubleNewlinesRead;

				// Set the non-local variables to the calculated values.
				this.Xdim = xDimen;
				this.Ydim = yDimen;
				this.Zdim = zDimen;
			}

			// Error message in case additional file types are added but not implemented.
			else { throw new Exception("Error: file type not implemented."); }

			// Regardless of file format, Tdim is given as a parameter.
			this.Tdim = timestep;

			// Then an array of values is created with appropriate dimensions.
            this.values = new float[Xdim, Ydim, Zdim];
        }

		//===================================================================//
		//                             Getters                               //
		//===================================================================//

        /// <summary>
        /// Accessor for the largest value of a point in the data file.
        /// </summary>
        /// <returns>the largest value of a point in the data file</returns>
        public float GetMaxValue()
        {
            return this.maxValue;
        }

        /// <summary>
        /// Accessor for the smallest value of a point in the data file.
        /// </summary>
        /// <returns>the smallest value of a point in the data file</returns>
        public float GetMinValue()
        {
            return this.minValue;
        }

		/// <summary>
		/// Accessor for the value of a specific point in the grid.
		/// </summary>
		/// <param name="x">the x-coordinate of the point</param>
		/// <param name="y">the y-coordinate of the point</param>
		/// <param name="z">the z-coordinate of the point</param>
		/// <returns>the value of the point</returns>
		public float GetValueAt(int x, int y, int z)
		{
			return this.values[x, y, z];
		}

        /// <summary>
        /// Creates a list of grid points, sorted by value.
        /// </summary>
        /// <returns>a list of grid points, sorted by value</returns>
        public List<GridPoint> GetSortedGridPointList()
        {
            List<GridPoint> pointList = new List<GridPoint>(Xdim * Ydim * Zdim);

            for (int x = 0; x < this.Xdim; x++) {
                for (int y = 0; y < this.Ydim; y++) {
                    for (int z = 0; z < this.Zdim; z++) {
                        pointList.Add(new GridPoint(x,y,z,this.Tdim,this.values[x,y,z]));
                    }
                }
            }

            pointList.Sort(new Comparison<GridPoint>(GridPoint.Compare));

            return pointList;
        }

    }
}
