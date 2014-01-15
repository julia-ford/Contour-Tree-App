using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuliaFordContourTreeApp.Trees;
using System.Windows.Forms;
using System.Drawing;

namespace JuliaFordContourTreeApp.Tree_Views
{
    abstract class AbstractTreeView : Panel
    {
		/// <summary>
		/// The width of a node that gets drawn.
		/// </summary>
		public static readonly float NODE_DIAMETER = 5.0f;

		/// <summary>
		/// the list of trees to draw in this view
		/// </summary>
        protected List<AbstractTree> trees;

		/// <summary>
		/// the maximum value of any node in any tree in this view
		/// </summary>
		protected float maxValue;
		/// <summary>
		/// the minimum value of any node in any tree in this view
		/// </summary>
		protected float minValue;

		/// <summary>
		/// Basic setup that applies to both the split and join tree views.
		/// </summary>
        public AbstractTreeView() : base()
        {
			this.Anchor = (AnchorStyles)(((AnchorStyles.Left | AnchorStyles.Bottom) | AnchorStyles.Top) | AnchorStyles.Right);
			this.trees = new List<AbstractTree>();
			this.Resize += new EventHandler(resize);
        }

		/// <summary>
		/// Converts a node's value to a y coordinate.
		/// </summary>
		/// <param name="value">the value of the node</param>
		/// <returns>a y coordinate</returns>
		protected float ValueToYCoordinate(float value)
		{
			float percent = (value - this.minValue) / (this.maxValue - this.minValue);
			float inverted = 1 - percent;
			float scaled = inverted * (this.Height - NODE_DIAMETER) + (NODE_DIAMETER / 2);
			return scaled;
		}

		/// <summary>
		/// Determines the number of pixels that can be alloted to each "slot"
		/// for a given number of slots.
		/// </summary>
		/// <param name="numSlots">the number of slots</param>
		/// <returns>
		/// the number of pixels that can be alloted to each slot
		/// </returns>
		protected float GetPixelsPerSlot(int numSlots)
		{
			return (this.Width - NODE_DIAMETER) / numSlots;
		}

		/// <summary>
		/// Adds a new tree to the list of trees to draw.
		/// </summary>
		/// <param name="tree">the tree to add</param>
		public void AddTree(AbstractTree tree)
		{
			if (tree.maxValue > this.maxValue || this.trees.Count == 0) {
				this.maxValue = tree.maxValue; }
			if (tree.minValue < this.minValue || this.trees.Count == 0) {
				this.minValue = tree.minValue; }
			this.trees.Add(tree);
			this.DrawTrees();
		}

		/// <summary>
		/// Remove all of the trees from this view.
		/// </summary>
		public void ClearTrees()
		{
			this.trees = new List<AbstractTree>();
			this.DrawTrees();
		}

		/// <summary>
		/// Causes the view to draw all of its trees.
		/// </summary>
		public void DrawTrees()
		{
			Graphics graphics = this.CreateGraphics();
			Pen pen = new Pen(Color.Black);
			Brush eraser = new SolidBrush(Color.White);
			Brush nodeFill = new SolidBrush(Color.Red);

			graphics.FillRectangle(eraser, 0, 0, this.Width, this.Height);

			foreach (AbstractTree tree in this.trees) {
				drawTree(tree, graphics, pen, nodeFill); }

			nodeFill.Dispose();
			eraser.Dispose();
			pen.Dispose();
			graphics.Dispose();
		}

		/// <summary>
		/// Draw one particular tree.
		/// </summary>
		/// <param name="tree">the tree to draw</param>
		/// <param name="graphics">allows things to be drawn</param>
		/// <param name="pen">for drawing lines</param>
		/// <param name="nodeFill">for drawing nodes</param>
		protected abstract void drawTree(AbstractTree tree, Graphics graphics, Pen pen, Brush nodeFill);

		/// <summary>
		/// Makes the view re-draw itself when resized.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void resize(object uselessParam1, EventArgs uselessParam2)
		{
			this.DrawTrees();
		}
    }
}
