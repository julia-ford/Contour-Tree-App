using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JuliaFordContourTreeApp.Tree_Views;

namespace JuliaFordContourTreeApp
{
    /// <summary>
    /// These panels are specially designed so that I only need to create the
    /// toolbar once.
    /// </summary>
    class GUIpanel : TableLayoutPanel
    {
        /// <summary>
        /// Create a new GUIpanel.
        /// </summary>
        /// <param name="colSpan">
        /// the number of columns this panel should cover
        /// </param>
        /// <param name="dataPanel">a reference to the data view panel</param>
        /// <param name="contourTreePanel">a reference to the contour tree view panel</param>
        /// <param name="joinTreePanel">a reference to the join tree view panel</param>
        /// <param name="splitTreePanel">a reference to the split tree view panel</param>
        /// <param name="uncertaintyPanel">a reference to the uncertainty view panel</param>
		public GUIpanel(Panel dataPanel, Panel contourTreePanel, JoinTreeView joinTreePanel, Panel splitTreePanel, Panel uncertaintyPanel, Panel defaultPanel)
            : base()
        {
            // display properties
            this.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
			this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // internal layout stuff
            this.ColumnCount = 1;
			this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.RowCount = 2;
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

			// determine the appropriate label
			string label = getLabel(dataPanel, contourTreePanel, joinTreePanel, splitTreePanel, uncertaintyPanel, defaultPanel);

            // create menu
            this.Controls.Add(new GUIpanelMenu(this, dataPanel, contourTreePanel, joinTreePanel, splitTreePanel, uncertaintyPanel, label));

            // add default view
            this.Controls.Add(defaultPanel);
        }

		/// <summary>
		/// Determines the appropriate text to display on the label on the menu.
		/// </summary>
		/// <returns>text indicating which view is being shown</returns>
		private string getLabel(Panel dataPanel, Panel contourTreePanel, JoinTreeView joinTreePanel, Panel splitTreePanel, Panel uncertaintyPanel, Panel defaultPanel)
		{
			if (dataPanel        == defaultPanel) { return "Data View"; }
			if (contourTreePanel == defaultPanel) { return "Contour Tree View"; }
			if (joinTreePanel    == defaultPanel) { return "Join Tree View"; }
			if (splitTreePanel   == defaultPanel) { return "Split Tree View"; }
			if (uncertaintyPanel == defaultPanel) { return "Topology Change View"; }
			return "Unknown View";
		}

    }
}
