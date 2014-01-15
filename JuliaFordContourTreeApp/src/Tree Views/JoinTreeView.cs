using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using JuliaFordContourTreeApp.Trees;

namespace JuliaFordContourTreeApp.Tree_Views
{
	class JoinTreeView : AbstractTreeView
	{
		/// <summary>
		/// Create a new JoinTreeView.
		/// </summary>
		public JoinTreeView() : base() { }

		/// <summary>
		/// Draw a particular join tree.
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
			graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(tree.maxValue) - half_Diameter, NODE_DIAMETER, NODE_DIAMETER);

			TreeNode currentTrunkNode = tree.GetParentless()[0];
			int      currentDirection = -1;

			while (currentTrunkNode.GetChildren().Count > 0) {
				// look ahead at next trunk node
				TreeNode nextTrunkNode = currentTrunkNode.GetChildren()[0];

				// draw next node
				graphics.FillEllipse(nodeFill, screenCenterX - half_Diameter, this.ValueToYCoordinate(nextTrunkNode.value) - half_Diameter, NODE_DIAMETER, NODE_DIAMETER);

				// draw path to next trunk node
				graphics.DrawLine(pen, screenCenterX, this.ValueToYCoordinate(currentTrunkNode.value),
									   screenCenterX, this.ValueToYCoordinate(   nextTrunkNode.value));

				// make a list of branches from the next trunk node
				List<TreeNode> branches = new List<TreeNode>(nextTrunkNode.GetParents());

				// remove the trunk node from the list of branches
				branches.Remove(currentTrunkNode);

				foreach (TreeNode branch in branches) {
					drawBranch(branch, graphics, pen, nodeFill, currentDirection, currentDirection == -1 ? leftSlotsUsed : rightSlotsUsed, pixelsPerSlot, screenCenterX);
					if (currentDirection == -1) {
						leftSlotsUsed += branch.GetNumParentlessAncestors(); }
					else {
						rightSlotsUsed += branch.GetNumParentlessAncestors(); }
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
		/// <param name="childPositionX">the x-position of this branch's child</param>
		private void drawBranch(TreeNode branch, Graphics graphics, Pen pen, Brush nodeFill, int direction, int slotsUsed, float pixelsPerSlot, float childPositionX)
		{
			// calculate some useful numbers
			int slotsInThisBranch = branch.GetNumParentlessAncestors();
			float nodeCenterX = (this.Width / 2) + (direction * (slotsUsed + ((slotsInThisBranch + 1) / 2)) * pixelsPerSlot);

			// draw this node
			graphics.FillEllipse(nodeFill, nodeCenterX - (NODE_DIAMETER / 2), ValueToYCoordinate(branch.value) - (NODE_DIAMETER / 2), NODE_DIAMETER, NODE_DIAMETER);

			// draw lines from the branch node back to its child
			graphics.DrawLine(pen, childPositionX, ValueToYCoordinate(branch.GetChildren()[0].value), nodeCenterX, ValueToYCoordinate(branch.GetChildren()[0].value));
			graphics.DrawLine(pen, nodeCenterX, ValueToYCoordinate(branch.GetChildren()[0].value), nodeCenterX, ValueToYCoordinate(branch.value));

			int slotsUsedAfterParent = slotsUsed;
			foreach (TreeNode parent in branch.GetParents()) {
				drawBranch(parent, graphics, pen, nodeFill, direction, slotsUsedAfterParent, pixelsPerSlot, nodeCenterX);
				slotsUsedAfterParent += parent.GetNumParentlessAncestors(); }
		}

	}
}
