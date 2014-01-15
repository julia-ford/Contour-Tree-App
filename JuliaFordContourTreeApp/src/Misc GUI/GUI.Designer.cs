using System.Windows.Forms;

namespace JuliaFordContourTreeApp
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.GUI_layout = new System.Windows.Forms.TableLayoutPanel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addNewDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addNewNathanielstyleDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearAllDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.writeVTKDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.writeTreeDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// GUI_layout
			// 
			this.GUI_layout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GUI_layout.ColumnCount = 6;
			this.GUI_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.GUI_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.GUI_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.GUI_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.GUI_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.GUI_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.GUI_layout.Location = new System.Drawing.Point(12, 27);
			this.GUI_layout.Name = "GUI_layout";
			this.GUI_layout.RowCount = 2;
			this.GUI_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.67F));
			this.GUI_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
			this.GUI_layout.Size = new System.Drawing.Size(260, 223);
			this.GUI_layout.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(284, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewDataFileToolStripMenuItem,
            this.addNewNathanielstyleDataToolStripMenuItem,
            this.clearAllDataToolStripMenuItem,
            this.writeVTKDataToolStripMenuItem,
            this.writeTreeDataToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// addNewDataFileToolStripMenuItem
			// 
			this.addNewDataFileToolStripMenuItem.Name = "addNewDataFileToolStripMenuItem";
			this.addNewDataFileToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.addNewDataFileToolStripMenuItem.Text = "Add New Data File";
			this.addNewDataFileToolStripMenuItem.Click += new System.EventHandler(this.addNewDataFile);
			// 
			// addNewNathanielstyleDataToolStripMenuItem
			// 
			this.addNewNathanielstyleDataToolStripMenuItem.Name = "addNewNathanielstyleDataToolStripMenuItem";
			this.addNewNathanielstyleDataToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.addNewNathanielstyleDataToolStripMenuItem.Text = "Add New Nathaniel-style Data";
			this.addNewNathanielstyleDataToolStripMenuItem.Click += new System.EventHandler(this.addNewNathanielDataFile);
			// 
			// clearAllDataToolStripMenuItem
			// 
			this.clearAllDataToolStripMenuItem.Name = "clearAllDataToolStripMenuItem";
			this.clearAllDataToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.clearAllDataToolStripMenuItem.Text = "Clear All Data";
			this.clearAllDataToolStripMenuItem.Click += new System.EventHandler(this.clearData);
			// 
			// writeVTKDataToolStripMenuItem
			// 
			this.writeVTKDataToolStripMenuItem.Name = "writeVTKDataToolStripMenuItem";
			this.writeVTKDataToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.writeVTKDataToolStripMenuItem.Text = "Write VTK Data";
			this.writeVTKDataToolStripMenuItem.Click += new System.EventHandler(this.printOutVtkData);
			// 
			// writeTreeDataToolStripMenuItem
			// 
			this.writeTreeDataToolStripMenuItem.Name = "writeTreeDataToolStripMenuItem";
			this.writeTreeDataToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.writeTreeDataToolStripMenuItem.Text = "Write Tree Data";
			this.writeTreeDataToolStripMenuItem.Click += new System.EventHandler(this.printOutTreeData);
			// 
			// GUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.GUI_layout);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "GUI";
			this.Text = "GUI";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel GUI_layout;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem addNewDataFileToolStripMenuItem;
        private ToolStripMenuItem clearAllDataToolStripMenuItem;
		private ToolStripMenuItem writeVTKDataToolStripMenuItem;
		private ToolStripMenuItem writeTreeDataToolStripMenuItem;
		private ToolStripMenuItem addNewNathanielstyleDataToolStripMenuItem;
    }
}