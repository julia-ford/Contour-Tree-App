using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using JuliaFordContourTreeApp.Trees;

namespace JuliaFordContourTreeApp.Tree_Views
{
	class ContourTreeView : Panel
	{
		//===================================================================//
		//                           Variables                               //
		//===================================================================//

		private float maxValue;
		private float minValue;

		private List<ContourTree> trees;

		//===================================================================//
		//                          Constructors                             //
		//===================================================================//

		/// <summary>
		/// Create a new ContourTreeView.
		/// </summary>
		public ContourTreeView() : base()
		{
			this.Anchor = (AnchorStyles)(((AnchorStyles.Left | AnchorStyles.Bottom) | AnchorStyles.Top) | AnchorStyles.Right);
			this.trees = new List<ContourTree>();
			this.Resize += new EventHandler(resize);
		}

		//===================================================================//
		//                            Getters                                //
		//===================================================================//

		/// <summary>
		/// Accessor for the current contour tree.
		/// </summary>
		/// <returns></returns>
		public ContourTree GetContourTree()
		{
			return this.trees[0];
		}

		/// <summary>
		/// Determines the number of pixels that can be alloted to each "slot"
		/// for a given number of slots.
		/// </summary>
		/// <param name="numSlots">the number of slots</param>
		/// <returns>
		/// the number of pixels that can be alloted to each slot
		/// </returns>
		private float GetPixelsPerSlot(int numSlots)
		{
			return (this.Width - AbstractTreeView.NODE_DIAMETER) / numSlots;
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
			float scaled = inverted * (this.Height - AbstractTreeView.NODE_DIAMETER) + (AbstractTreeView.NODE_DIAMETER / 2);
			return scaled;
		}

		//===================================================================//
		//                            Actions                                //
		//===================================================================//

		/// <summary>
		/// Draw a particular contour tree.
		/// </summary>
		/// <param name="tree">the tree to draw</param>
		/// <param name="graphics">allows things to be drawn</param>
		/// <param name="pen">for drawing lines</param>
		/// <param name="nodeFill">for drawing nodes</param>
		private void drawTree(ContourTree tree, Graphics graphics, Pen pen, Brush nodeFill)
		{
			int   nrSlotsNeeded = tree.GetNumSlotsNeeded();
			float pixelsPerSlot = this.GetPixelsPerSlot(nrSlotsNeeded);
			float screenCenterX = this.Width / 2.0f;
			float half_Diameter = AbstractTreeView.NODE_DIAMETER / 2.0f;

			int topLeSlotsUsed = 0;
			int topRiSlotsUsed = 0;
			int botLeSlotsUsed = 0;
			int botRiSlotsUsed = 0;

			// Draw root, top, and bottom nodes.
			// TODO: Make the root node normal-colored again.
			graphics.FillEllipse(new SolidBrush(Color.Blue), screenCenterX - half_Diameter, this.ValueToYCoordinate(tree.GetRoot().value) - half_Diameter, AbstractTreeView.NODE_DIAMETER, AbstractTreeView.NODE_DIAMETER);
			graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(tree.maxValue) - half_Diameter, AbstractTreeView.NODE_DIAMETER, AbstractTreeView.NODE_DIAMETER);
			graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(tree.minValue) - half_Diameter, AbstractTreeView.NODE_DIAMETER, AbstractTreeView.NODE_DIAMETER);

			ContourNode currentTopMainPathNode = tree.GetTopMostLeaf();
			ContourNode currentBotMainPathNode = tree.GetBotMostLeaf();
			int currentTopDir = -1;
			int currentBotDir =  1;

			while (currentTopMainPathNode.GetTrunk() != tree.GetRoot() 
				|| currentBotMainPathNode.GetTrunk() != tree.GetRoot()) {

				// Top half of tree:
				if (currentTopMainPathNode.GetTrunk() != tree.GetRoot()) {
					// Look ahead at next "main path" node.
					ContourNode nextTopMainPathNode = currentTopMainPathNode.GetTrunk();

					// Draw the next node.
					graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(nextTopMainPathNode.value) - half_Diameter, AbstractTreeView.NODE_DIAMETER, AbstractTreeView.NODE_DIAMETER);

					// Draw the path to next node.
					graphics.DrawLine(pen, screenCenterX, this.ValueToYCoordinate(currentTopMainPathNode.value),
										   screenCenterX, this.ValueToYCoordinate(   nextTopMainPathNode.value));

					// Make a list of branches from the next node.
					List<ContourNode> branches = new List<ContourNode>(nextTopMainPathNode.GetBranches());

					// Remove the current node from the list of branches.
					branches.Remove(currentTopMainPathNode);

					// Draw all the branches.
					foreach (ContourNode branch in branches) {
						drawBranch(branch, graphics, pen, nodeFill, currentTopDir, currentTopDir == -1 ? topLeSlotsUsed : topRiSlotsUsed, pixelsPerSlot, screenCenterX);
						if (currentTopDir == -1) {
							topLeSlotsUsed += branch.GetNumLeaves(); }
						else {
							topRiSlotsUsed += branch.GetNumLeaves(); } }

					// Toggle the direction of adding.
					currentTopDir *= -1;

					// Point at next node.
					currentTopMainPathNode = nextTopMainPathNode; }

				// Bottom half of tree:
				if (currentBotMainPathNode.GetTrunk() != tree.GetRoot()) {
					// Look ahead at next "main path" node.
					ContourNode nextBotMainPathNode = currentBotMainPathNode.GetTrunk();

					// Draw the next node.
					graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(nextBotMainPathNode.value) - half_Diameter, AbstractTreeView.NODE_DIAMETER, AbstractTreeView.NODE_DIAMETER);

					// Draw the path to next node.
					graphics.DrawLine(pen, screenCenterX, this.ValueToYCoordinate(currentBotMainPathNode.value),
											screenCenterX, this.ValueToYCoordinate(   nextBotMainPathNode.value));

					// Make a list of branches from the next node.
					List<ContourNode> branches = new List<ContourNode>(nextBotMainPathNode.GetBranches());

					// Remove the current node from the list of branches.
					branches.Remove(currentBotMainPathNode);

					// Draw all the branches.
					foreach (ContourNode branch in branches) {
						drawBranch(branch, graphics, pen, nodeFill, currentBotDir, currentBotDir == -1 ? botLeSlotsUsed : botRiSlotsUsed, pixelsPerSlot, screenCenterX);
						if (currentBotDir == -1) {
							botLeSlotsUsed += branch.GetNumLeaves(); }
						else {
							botRiSlotsUsed += branch.GetNumLeaves(); } }

					// Toggle the direction of adding.
					currentBotDir *= -1;

					// Point at next node.
					currentBotMainPathNode = nextBotMainPathNode; }
			}

			// Draw paths from currentTopMainPathNode and currentBotMainPathNode to root.
			graphics.DrawLine(pen, screenCenterX, this.ValueToYCoordinate(currentTopMainPathNode.value), screenCenterX, this.ValueToYCoordinate(currentBotMainPathNode.value));

			// Leftover branches from the root:
			foreach (ContourNode branch in tree.GetRoot().GetBranches())
			{
				if (branch != currentTopMainPathNode && branch != currentBotMainPathNode)
				{
					if (branch.value > tree.GetRoot().value) {
						drawBranch(branch, graphics, pen, nodeFill, currentTopDir, currentTopDir == -1 ? topLeSlotsUsed : topRiSlotsUsed, pixelsPerSlot, screenCenterX);
						if (currentTopDir == -1) {
							topLeSlotsUsed += branch.GetNumLeaves(); }
						else {
							topRiSlotsUsed += branch.GetNumLeaves(); }
						currentTopDir *= -1; }
					else {
						drawBranch(branch, graphics, pen, nodeFill, currentBotDir, currentBotDir == -1 ? botLeSlotsUsed : botRiSlotsUsed, pixelsPerSlot, screenCenterX);
						if (currentBotDir == -1) {
							botLeSlotsUsed += branch.GetNumLeaves(); }
						else {
							botRiSlotsUsed += branch.GetNumLeaves(); }
						currentBotDir *= -1; }
				}
			}
		}

		/// <summary>
		/// Draws a branch in a tree. This function is recursive.
		/// </summary>
		/// <param name="branch">the branch to draw</param>
		/// <param name="graphics">allows graphics to be drawn</param>
		/// <param name="pen">for drawing lines</param>
		/// <param name="nodeFill">for drawing nodes</param>
		/// <param name="direction">the side of the tree the node is drawn on</param>
		/// <param name="slotsUsed">the number of slots on this side of the tree that have already been used</param>
		/// <param name="pixelsPerSlot">the number of pixels in a slot</param>
		/// <param name="childPositionX">the x-position of this branch's child</param>
		private void drawBranch(ContourNode branch, Graphics graphics, Pen pen, Brush nodeFill, int direction, int slotsUsed, float pixelsPerSlot, float childPositionX)
		{
			// calculate some useful numbers
			int slotsInThisBranch = branch.GetNumLeaves();
			float nodeCenterX = (this.Width / 2) + (direction * (slotsUsed + ((slotsInThisBranch + 1) / 2)) * pixelsPerSlot);

			// draw this node
			graphics.FillEllipse(nodeFill, nodeCenterX - (AbstractTreeView.NODE_DIAMETER / 2), ValueToYCoordinate(branch.value) - (AbstractTreeView.NODE_DIAMETER / 2), AbstractTreeView.NODE_DIAMETER, AbstractTreeView.NODE_DIAMETER);

			// Draw lines from the branch node back to its trunk node.
			graphics.DrawLine(pen, childPositionX, ValueToYCoordinate(branch.GetTrunk().value), nodeCenterX, ValueToYCoordinate(branch.GetTrunk().value));
			graphics.DrawLine(pen, nodeCenterX, ValueToYCoordinate(branch.GetTrunk().value), nodeCenterX, ValueToYCoordinate(branch.value));

			int slotsUsedAfterLeaf = slotsUsed;
			foreach (ContourNode leaf in branch.GetBranches()) {
				drawBranch(leaf, graphics, pen, nodeFill, direction, slotsUsedAfterLeaf, pixelsPerSlot, nodeCenterX);
				slotsUsedAfterLeaf += leaf.GetNumLeaves(); }
		}
		
		/// <summary>
		/// Adds a new tree to the list of trees to draw.
		/// </summary>
		/// <param name="tree">the tree to add</param>
		public void AddTree(ContourTree tree)
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
			this.trees = new List<ContourTree>();
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

			foreach (ContourTree tree in this.trees) {
				drawTree(tree, graphics, pen, nodeFill); }

			nodeFill.Dispose();
			eraser.Dispose();
			pen.Dispose();
			graphics.Dispose();
		}

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
