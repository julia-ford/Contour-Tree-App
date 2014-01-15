using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using JuliaFordContourTreeApp.Trees;

namespace JuliaFordContourTreeApp.Tree_Views
{
	class TopologyChangeView : Panel
	{
		public static readonly int MARGIN_BOT = 10;
		public static readonly int MARGIN_LEF = 10;
		public static readonly int NODE_RADIUS = 3;

		private int timeSteps = 0;

		private float maxValue;
		private float minValue;

		private ContourTree tree;

		public TopologyChangeView() : base()
		{
			drawMe();
			this.Anchor = (AnchorStyles)(((AnchorStyles.Left | AnchorStyles.Right) | AnchorStyles.Top) | AnchorStyles.Bottom);
			this.Resize += new EventHandler(resize);
		}

		/// <summary>
		/// Draws the view.
		/// </summary>
		private void drawMe()
		{
			Graphics graphics = this.CreateGraphics();
			Pen pen = new Pen(Color.Black);
			Brush eraser = new SolidBrush(Color.White);
			Brush nodeFill = new SolidBrush(Color.Red);

			graphics.FillRectangle(eraser, 0, 0, this.Width, this.Height);

			if (timeSteps > 1 && tree != null)
			{
				drawNode(tree.GetRoot(), graphics, nodeFill);
				
				List<ContourNode> currentLevel = new List<ContourNode>();
				currentLevel.Add(tree.GetRoot());

				while (currentLevel.Count > 0) {
					// Create next level.
					List<ContourNode> nextLevel = new List<ContourNode>();

					// Fill next level with nodes.
					foreach (ContourNode node in currentLevel) {
						nextLevel = nextLevel.Union(node.GetBranches()).ToList(); }

					// Draw all the new nodes.
					foreach (ContourNode node in nextLevel) {
						drawNode(node, graphics, nodeFill);
						drawLine(node, node.GetTrunk(), graphics, pen);
					}

					// Look at next set of nodes.
					currentLevel = nextLevel;
				}
			}

			nodeFill.Dispose();
			eraser.Dispose();
			pen.Dispose();
			graphics.Dispose();

			if (this.tree != null) {
				Console.Write("Max View Value: " + this.maxValue + '\n' +
							  "Max Tree Value: " + this.tree.GetMaxValue() + '\n' +
							  "Max Y Coord:    " + this.getY(this.maxValue) + '\n' +
							  "Min Tree Value: " + this.tree.GetMinValue() + '\n' +
							  "Min Y Coord:    " + this.getY(this.minValue) + '\n' +
							  "View Height:    " + this.Height + '\n'); }
		}

		/// <summary>
		/// Determines the appropriate x coordiante for a given float.
		/// </summary>
		/// <param name="value">the time coordinate of a node</param>
		/// <returns>the x coordiante at which to display a node</returns>
		private float getX(float value)
		{
			int usableWidth = this.Width - MARGIN_LEF - (NODE_RADIUS * 2);
			float valueSpan = timeSteps;
			float scaleRate = usableWidth / valueSpan;
			float offset = scaleRate / 2;
			return scaleRate * value + MARGIN_LEF + NODE_RADIUS + offset;
		}

		/// <summary>
		/// Determines the appropriate y coordiante for a given float.
		/// </summary>
		/// <param name="value">the value of a node</param>
		/// <returns>the y coordiante at which to display a node</returns>
		private float getY(float value)
		{
			int usableHeight = this.Height - MARGIN_BOT - (NODE_RADIUS * 2);
			float valueSpan = maxValue - minValue;
			float scaleRate = usableHeight / valueSpan;
			if (this.Height - (scaleRate * (value - minValue)) - MARGIN_BOT < -0.1 || this.Height - (scaleRate * (value - minValue)) - MARGIN_BOT > this.Height)
			{
				Console.WriteLine("\nError: y coord = " + (this.Height - (scaleRate * (value - minValue)) + MARGIN_BOT) + "; value = " + value + '\n');
			}
			return this.Height - (scaleRate * (value - minValue)) - MARGIN_BOT - NODE_RADIUS;
		}

		/// <summary>
		/// Makes the view re-draw itself when resized.
		/// </summary>
		/// <param name="uselessParam1">required by C#</param>
		/// <param name="uselessParam2">required by C#</param>
		private void resize(object uselessParam1, EventArgs uselessParam2)
		{
			this.drawMe();
		}

		/// <summary>
		/// Increments the number of timesteps recorded by one when a new data file is added.
		/// </summary>
		public void AddTimeStep()
		{
			this.timeSteps++;
			this.drawMe();
		}

		/// <summary>
		/// Used when the data is cleared; resets the number of timesteps recorded to 0.
		/// </summary>
		public void ResetTimeSteps()
		{
			this.timeSteps = 0;
		}

		/// <summary>
		/// Mutator for the contour tree that this view displays information about.
		/// </summary>
		/// <param name="tree"></param>
		public void SetTree(ContourTree tree)
		{
			this.tree = tree;
			this.maxValue = tree.GetMaxValue();
			this.minValue = tree.GetMinValue();
		}

		/// <summary>
		/// Draws an ellipse to represent a given node.
		/// </summary>
		/// <param name="node">the node in the treee to display</param>
		/// <param name="graphics">the Graphics object that hadles drawing for this view</param>
		/// <param name="nodeFill">the brush that determines the color of this node</param>
		private void drawNode(ContourNode node, Graphics graphics, Brush nodeFill) 
		{
			graphics.FillEllipse(nodeFill, getX(node.t) - NODE_RADIUS, getY(node.value) - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);
		}

		/// <summary>
		/// Draws a line between two nodes.
		/// </summary>
		/// <param name="node1">one of the two nodes between wich to draw a line</param>
		/// <param name="node2">the other of the two nodes between wich to draw a line</param>
		/// <param name="graphics">the Graphics object that hadles drawing for this view</param>
		/// <param name="pen">the pen that determines the color and thickness of this line</param>
		private void drawLine(ContourNode node1, ContourNode node2, Graphics graphics, Pen pen)
		{
			graphics.DrawLine(pen, getX(node1.t), getY(node1.value), getX(node2.t), getY(node2.value));
		}

	}
}