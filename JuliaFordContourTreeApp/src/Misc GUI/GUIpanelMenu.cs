using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using JuliaFordContourTreeApp.Tree_Views;

namespace JuliaFordContourTreeApp
{
    /// <summary>
    /// A special menu bar that has the standard interactions for the GUI panels.
    /// </summary>
    class GUIpanelMenu : MenuStrip
    {
		/// <summary>
		/// reference to the panel this appears in
		/// </summary>
		private readonly GUIpanel myGUIpanel;
        /// <summary>
        /// reference to the data view panel
        /// </summary>
        private readonly Panel dataPanel;
        /// <summary>
        /// reference to the contour tree view panel
        /// </summary>
        private readonly Panel contourTreePanel;
        /// <summary>
        /// reference to the join tree view panel
        /// </summary>
        private readonly JoinTreeView joinTreePanel;
        /// <summary>
        /// reference to the split tree view panel
        /// </summary>
        private readonly Panel splitTreePanel;
        /// <summary>
        /// reference to the uncertainty view panel
        /// </summary>
        private readonly Panel uncertaintyPanel;

		/// <summary>
		/// An item on the menu that does nothing when clicked, but is updated
		/// to display the name of the current view.
		/// </summary>
		private ToolStripMenuItem label;

        /// <summary>
        /// Creates the menu for a GUIpanel.
        /// </summary>
		/// <param name="myGUIpanel">the GUIpanel that this menu is attached to</param>
        /// <param name="dataPanel">a reference to the data view panel</param>
        /// <param name="contourTreePanel">a reference to the contour tree view panel</param>
        /// <param name="joinTreePanel">a reference to the join tree view panel</param>
        /// <param name="splitTreePanel">a reference to the split tree view panel</param>
        /// <param name="uncertaintyPanel">a reference to the uncertainty view panel</param>
		/// <param name="label">the text to display on the "label" ToolStripMenuItem when this is first instantiated</param>
		public GUIpanelMenu(GUIpanel myGUIpanel, Panel dataPanel, Panel contourTreePanel, JoinTreeView joinTreePanel, Panel splitTreePanel, Panel uncertaintyPanel, string label)
			: base()
        {
            // display properties
            this.Anchor = (AnchorStyles)((AnchorStyles.Left | AnchorStyles.Top) | AnchorStyles.Right);

            // panel references
			this.myGUIpanel       = myGUIpanel;
            this.dataPanel        = dataPanel;
            this.contourTreePanel = contourTreePanel;
            this.joinTreePanel    = joinTreePanel;
            this.splitTreePanel   = splitTreePanel;
            this.uncertaintyPanel = uncertaintyPanel;

            // menu items
			ToolStripMenuItem display = new ToolStripMenuItem("Display...");
			this.Items.Add(display);
			display.DropDownItems.Add(new ToolStripMenuItem("Data", null, new EventHandler(displayData)));
			display.DropDownItems.Add(new ToolStripMenuItem("Contour Tree", null, new EventHandler(displayContourTree)));
			display.DropDownItems.Add(new ToolStripMenuItem("Join Tree", null, new EventHandler(displayJoinTree)));
			display.DropDownItems.Add(new ToolStripMenuItem("Split Tree", null, new EventHandler(displaySplitTree)));
			display.DropDownItems.Add(new ToolStripMenuItem("Uncertainty", null, new EventHandler(displayUncertainty)));

			// the label
			this.label = new ToolStripMenuItem(label);
			this.Items.Add(label);
        }

        /// <summary>
        /// Causes the panel to which this menu corresponds to set its display
        /// to the data view.
        /// </summary>
        /// <param name="uselessParam1">parameter required by C#</param>
        /// <param name="uselessParam2">parameter required by C#</param>
        private void displayData(object uselessParam1, EventArgs uselessParam2)
        {
			if (myGUIpanel.Controls.Count > 1) {
				myGUIpanel.Controls.Remove(myGUIpanel.Controls[1]); }
			this.myGUIpanel.Controls.Add(dataPanel);

			// update label
			this.label.Text = "Data View";
        }

        /// <summary>
        /// Causes the panel to which this menu corresponds to set its display
        /// to the contour tree view.
        /// </summary>
        /// <param name="uselessParam1">parameter required by C#</param>
        /// <param name="uselessParam2">parameter required by C#</param>
        private void displayContourTree(object uselessParam1, EventArgs uselessParam2)
        {
            if (myGUIpanel.Controls.Count > 1) {
				myGUIpanel.Controls.Remove(myGUIpanel.Controls[1]); }
			this.myGUIpanel.Controls.Add(contourTreePanel);

			// update label
			this.label.Text = "Contour Tree View";
        }

        /// <summary>
        /// Causes the panel to which this menu corresponds to set its display
        /// to the join tree view.
        /// </summary>
        /// <param name="uselessParam1">parameter required by C#</param>
        /// <param name="uselessParam2">parameter required by C#</param>
        private void displayJoinTree(object uselessParam1, EventArgs uselessParam2)
        {
            if (myGUIpanel.Controls.Count > 1) {
				myGUIpanel.Controls.Remove(myGUIpanel.Controls[1]); }
			this.myGUIpanel.Controls.Add(joinTreePanel);
			joinTreePanel.DrawTrees();

			// update label
			this.label.Text = "Join Tree View";
        }

        /// <summary>
        /// Causes the panel to which this menu corresponds to set its display
        /// to the split tree view.
        /// </summary>
        /// <param name="uselessParam1">parameter required by C#</param>
        /// <param name="uselessParam2">parameter required by C#</param>
        private void displaySplitTree(object uselessParam1, EventArgs uselessParam2)
        {
            if (myGUIpanel.Controls.Count > 1) {
				myGUIpanel.Controls.Remove(myGUIpanel.Controls[1]); }
			this.myGUIpanel.Controls.Add(splitTreePanel);

			// update label
			this.label.Text = "Split Tree View";
        }

        /// <summary>
        /// Causes the panel to which this menu corresponds to set its display
        /// to the uncertainty view.
        /// </summary>
        /// <param name="uselessParam1">parameter required by C#</param>
        /// <param name="uselessParam2">parameter required by C#</param>
        private void displayUncertainty(object uselessParam1, EventArgs uselessParam2)
        {
            if (myGUIpanel.Controls.Count > 1) {
				myGUIpanel.Controls.Remove(myGUIpanel.Controls[1]); }
			this.myGUIpanel.Controls.Add(uncertaintyPanel);

			// update label
			this.label.Text = "Topology Change View";
        }

    }
}
