using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using JuliaFordContourTreeApp.Trees;

namespace JuliaFordContourTreeApp.Tree_Views
{
	class SplitTreeView : AbstractTreeView
	{
		/// <summary>
		/// Create a new SplitTreeView.
		/// </summary>
		public SplitTreeView() : base() { }

		/// <summary>
		/// Draw a particular split tree.
		/// </summary>
		/// <param name="tree">the tree to draw</param>
		/// <param name="graphics">allows things to be drawn</param>
		/// <param name="pen">for drawing lines</param>
		/// <param name="nodeFill">for drawing nodes</param>
		protected override void drawTree(AbstractTree tree, Graphics graphics, Pen pen, Brush nodeFill)
		{
			int   nrSlotsNeeded = tree.GetNumSlotsNeeded();
			float pixelsPerSlot = this.GetPixelsPerSlot(nrSlotsNeeded);
			float screenCenterX = this.Width / 2.0f;
			float half_Diameter = NODE_DIAMETER / 2.0f;

			int leftSlotsUsed = 0;
			int rightSlotsUsed = 0;

			// draw the top node
			graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(tree.minValue) - half_Diameter, NODE_DIAMETER, NODE_DIAMETER);

			TreeNode currentTrunkNode = tree.GetChildless()[0];
			int      currentDirection = -1;

			while (currentTrunkNode.GetParents().Count > 0) {
				// look ahead at next trunk node
				TreeNode nextTrunkNode = currentTrunkNode.GetParents()[0];

				// draw next node
				graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(nextTrunkNode.value) - half_Diameter, NODE_DIAMETER, NODE_DIAMETER);

				// draw path to next trunk node
				graphics.DrawLine(pen, screenCenterX, this.ValueToYCoordinate(currentTrunkNode.value),
									   screenCenterX, this.ValueToYCoordinate(   nextTrunkNode.value));

				// make a list of branches from the next trunk node
				List<TreeNode> branches = new List<TreeNode>(nextTrunkNode.GetChildren());

				// remove the trunk node from the list of branches
				branches.Remove(currentTrunkNode);

				foreach (TreeNode branch in branches) {
					drawBranch(branch, graphics, pen, nodeFill, currentDirection, currentDirection == -1 ? leftSlotsUsed : rightSlotsUsed, pixelsPerSlot, screenCenterX);
					if (currentDirection == -1) {
						leftSlotsUsed += branch.GetNumChildlessDescendents(); }
					else {
						rightSlotsUsed += branch.GetNumChildlessDescendents(); }
				}

				// toggle direction
				currentDirection *= -1;

				// point at next trunk node
				currentTrunkNode = nextTrunkNode;
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
		/// <param name="parentPositionX">the x-position of this branch's parent</param>
		private void drawBranch(TreeNode branch, Graphics graphics, Pen pen, Brush nodeFill, int direction, int slotsUsed, float pixelsPerSlot, float parentPositionX)
		{
			// calculate some useful numbers
			int slotsInThisBranch = branch.GetNumChildlessDescendents();
			float nodeCenterX = (this.Width / 2) + (direction * (slotsUsed + ((slotsInThisBranch + 1) / 2)) * pixelsPerSlot);

			// draw this node
			graphics.FillEllipse(nodeFill, nodeCenterX - (NODE_DIAMETER / 2), ValueToYCoordinate(branch.value) - (NODE_DIAMETER / 2), NODE_DIAMETER, NODE_DIAMETER);

			// draw lines from the branch node back to its parent
			graphics.DrawLine(pen, parentPositionX, ValueToYCoordinate(branch.GetParents()[0].value), nodeCenterX, ValueToYCoordinate(branch.GetParents()[0].value));
			graphics.DrawLine(pen, nodeCenterX, ValueToYCoordinate(branch.GetParents()[0].value), nodeCenterX, ValueToYCoordinate(branch.value));

			int slotsUsedAfterChild = slotsUsed;
			foreach (TreeNode parent in branch.GetChildren()) {
				drawBranch(parent, graphics, pen, nodeFill, direction, slotsUsedAfterChild, pixelsPerSlot, nodeCenterX);
				slotsUsedAfterChild += parent.GetNumChildlessDescendents(); }
		}

	}
}
