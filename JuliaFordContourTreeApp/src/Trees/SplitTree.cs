using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp.Trees
{
    class SplitTree : AbstractTree
	{
		//===================================================================//
		//                          Constructors                             //
		//===================================================================//

        /// <summary>
        /// Create a new SplitTree.
        /// </summary>
        /// <param name="maxValue">the maximum value of a node in the tree</param>
        /// <param name="minValue">the minimum value of a node in the tree</param>
        /// <param name="points">the grid points from which to construct the tree</param>
        public SplitTree(float maxValue, float minValue, List<GridPoint> points) : base(maxValue, minValue)
        {
			// reverse th points list
			points.Reverse();

            // create a list to store all the blobs in
            List<BlobOfNodes> blobList = new List<BlobOfNodes>();

            // Turn each point into a tree node.
            foreach (GridPoint point in points)
            {
                // Turn the next point into a node.
                TreeNode newNode = new TreeNode(point);

                // Make a list of all the blobs to which the node is adjacent.
                List<BlobOfNodes> adjacentBlobs = new List<BlobOfNodes>();
                // Check each blob in the list.
                foreach (BlobOfNodes blob in blobList) {
                    // If it is adjacent...
                    if (blob.IsAdjacentTo(newNode)) {
                        // ...add it to the list of adjacent blobs.
                        adjacentBlobs.Add(blob); } }

                // If the node is not adjacent to any blobs,
                // create a new blob for it.
                if (adjacentBlobs.Count == 0) {
                    BlobOfNodes newBlob = new BlobOfNodes(newNode);
                    blobList.Add(newBlob);
                    this.childless.Add(newNode); }

                // If the node is adjacent to exactly one blob,
                // add it to that blob.
                else if (adjacentBlobs.Count == 1) {
                    adjacentBlobs[0].GetMostRecentlyAddedNode().AddParent(newNode);
                    newNode.AddChild(adjacentBlobs[0].GetMostRecentlyAddedNode());
                    adjacentBlobs[0].Add(newNode); }

                // If the node is adjacent to more than one blob,
                // merge the blobs.
                else {
                    foreach (BlobOfNodes blob in adjacentBlobs) {
						blob.GetMostRecentlyAddedNode().AddParent(newNode);
						newNode.AddChild(blob.GetMostRecentlyAddedNode());
                        blobList.Remove(blob); }
                    this.merging.Add(newNode);
                    blobList.Add(new BlobOfNodes(newNode, adjacentBlobs)); }
            } // end of adding all the gridpoints

            // At this point, there will be exactly one blob. Its most recently
            // added node is the bottom of the split tree.
            this.parentless.Add(blobList[0].GetMostRecentlyAddedNode());
        }

		/// <summary>
		/// This is a copy constructor for SplitTree.
		/// </summary>
		/// <param name="other">the SplitTree to copy</param>
		public SplitTree(SplitTree other) : base(other.maxValue, other.minValue)
		{
			other.Clone(this.parentless, this.merging, this.childless);
		}

		//===================================================================//
		//                             Getters                               //
		//===================================================================//

		/// <summary>
		/// The SplitTree implementation of GetNumSlotsNeeded().
		/// </summary>
		/// <returns>the number of slots needed to draw this SplitTree</returns>
		/// <seealso cref="AbstractTree.GetNumSlotsNeeded"/>
		public override int GetNumSlotsNeeded()
		{
			// total number of slots needed
			int numSlotsNeeded = 1;

			// slots needed on left side of tree
			int leftSlotsNeeded = 0;
			// slots needed on right side of tree
			int rightSlotsNeeded = 0;

			TreeNode currentTrunkNode = this.childless[0];

			// branches alternate sides
			int branchDirection = -1;

			while (currentTrunkNode.GetParents().Count > 0) {
				TreeNode nextTrunkNode = currentTrunkNode.GetParents()[0];

				List<TreeNode> branches = nextTrunkNode.GetChildren();
				branches.Remove(currentTrunkNode);

				foreach (TreeNode branch in branches) {
					if (branchDirection == -1) {
						leftSlotsNeeded  += branch.GetNumChildlessDescendents(); }
					else {
						rightSlotsNeeded += branch.GetNumChildlessDescendents(); } }

				// toggle branch direction
				branchDirection *= -1;

				currentTrunkNode = nextTrunkNode; }

			// number of slots needed on each side is the same; it is the
			// number of slots needed by the larger side of the tree
			numSlotsNeeded += Math.Max(leftSlotsNeeded, rightSlotsNeeded) * 2;

			return numSlotsNeeded;
		}

		/// <summary>
		/// Searches the SplitTree for a node that matches the given TreeNode.
		/// </summary>
		/// <param name="other">the node to match</param>
		/// <returns>the TreeNode in this tree that matches the given node</returns>
		/// <exception cref="Exception">if the tree does not contain a match for the given node</exception>
		public TreeNode GetMatch(TreeNode other)
		{
			if (this.parentless[0].Matches(other)) {
				return this.parentless[0]; }

			List<TreeNode> currentLevel = new List<TreeNode>();
			currentLevel.Add(this.parentless[0]);

			while (currentLevel.Count > 0) {
				List<TreeNode> nextLevel = new List<TreeNode>();

				foreach (TreeNode currentNode in currentLevel) {
					foreach (TreeNode nextNode in currentNode.GetChildren()) {
						if (other.Matches(nextNode)) {
							return nextNode; }
						nextLevel.Add(nextNode); } }

				currentLevel = nextLevel; }

			throw new Exception("Error: attempted to find a node in a split tree matching a given node. The join tree did not contain a match.");
		}

		//===================================================================//
		//                             Actions                               //
		//===================================================================//

		/// <summary>
		/// Clones all of the nodes in the tree and duplicates their connections.
		/// </summary>
		/// <param name="parentless">the list in which to store the parentless nodes of the new tree</param>
		/// <param name="merging">the list in which to store the merging nodes of the new tree</param>
		/// <param name="childless">the list in which to store the childless nodes of the new tree</param>
		public void Clone(List<TreeNode> parentless, List<TreeNode> merging, List<TreeNode> childless)
		{
			TreeNode newTreeRoot = new TreeNode(this.GetParentless()[0]);
			parentless.Add(newTreeRoot);

			List<TreeNode> currentLevel = new List<TreeNode>();
			currentLevel.Add(this.GetParentless()[0]);
			List<TreeNode> currentCloneLevel = new List<TreeNode>();
			currentCloneLevel.Add(newTreeRoot);

			while (currentLevel.Count > 0)
			{
				List<TreeNode> nextLevel = new List<TreeNode>();
				List<TreeNode> nextCloneLevel = new List<TreeNode>();

				for (int id = 0; id < currentLevel.Count; id++)
				{
					foreach (TreeNode child in currentLevel[id].GetChildren())
					{
						TreeNode newNode = new TreeNode(child);
						newNode.AddParent(currentCloneLevel[id]);
						currentCloneLevel[id].AddChild(newNode);

						nextLevel.Add(child);
						nextCloneLevel.Add(newNode);

						if (this.merging.Contains(child)) {
							merging.Add(newNode); }
						if (this.childless.Contains(child)) {
							childless.Add(newNode); }
					}
				}
				currentLevel = nextLevel;
				currentCloneLevel = nextCloneLevel;
			}
		}

		/// <summary>
		/// The SplitTree implementation of Simplify().
		/// </summary>
		public override void Simplify()
		{
			List<TreeNode> currentLevel = new List<TreeNode>();
			currentLevel.Add(this.parentless[0]);

			while (currentLevel.Count > 0) {
				List<TreeNode> nextLevel = new List<TreeNode>();

				foreach (TreeNode node in currentLevel) {
					nextLevel = nextLevel.Union(node.GetChildren()).ToList();
					if (node.IsNonCritical()) {
						node.RemoveFromTree(); } }

				currentLevel = nextLevel; }
		}
		
		/// <summary>
		/// The SplitTree implementation of SimplifyExceptFor().
		/// </summary>
		/// <param name="exemptions">the list of nodes to leave alone</param>
		public override void SimplifyExceptFor(List<TreeNode> exemptions)
		{
			List<TreeNode> currentLevel = new List<TreeNode>();
			currentLevel.Add(this.parentless[0]);

			while (currentLevel.Count > 0) {
				List<TreeNode> nextLevel = new List<TreeNode>();

				// Look at each node in the current level.
				foreach (TreeNode node in currentLevel) {
					nextLevel = nextLevel.Union(node.GetChildren()).ToList();
					// If the current node is not critical, 
					if (node.IsNonCritical()) {
						// check if it matches any of the "exempt" nodes.
						bool matchFound = false;
						int tempIndex = 0;
						while (!matchFound && tempIndex < exemptions.Count) {
							if (exemptions[tempIndex].Matches(node)) {
								matchFound = true;
								exemptions.RemoveAt(tempIndex); }
							tempIndex++; }
						if (!matchFound) {
							node.RemoveFromTree(); }
					}
				} // end foreach

				currentLevel = nextLevel; }
		}
		
		/// <summary>
		/// Finds a node in this tree that matches the given node and removes
		/// the match from this tree.
		/// </summary>
		/// <param name="other">the node to match</param>
		public void RemoveMatch(TreeNode other)
		{
			// Find the node.
			TreeNode match = this.GetMatch(other);

			// If the node is the root of the tree,
			if (this.parentless[0] == match) {
				// if the root has a child,
				if (match.GetChildren().Count > 0) {
					// make the child the new root.
					this.parentless[0] = match.GetChildren()[0]; }
				// if the root is childless,
				else {
					// empty the "parentless" and "childless" lists,
					// because the tree is empty now.
					this.parentless.Remove(match);
					this.childless.Remove(match); } }

			// If the node is a leaf node,
			else if (this.childless.Contains(match)) {
				// if the node's parent has two children,
				if (match.GetParents()[0].GetChildren().Count == 2) {
					// remove the parent from the list of merge nodes.
					merging.Remove(match.GetParents()[0]); }
				// if the node's parent has only one child,
				else if (match.GetParents()[0].GetChildren().Count == 1) {
					// add the parent to the list of childless nodes.
					this.childless.Add(match.GetParents()[0]); }
				// Remove the node from the list of childless nodes.
				this.childless.Remove(match); }

			// If the node is a merge node,
			else if (this.merging.Contains(match)) {
				// remove it from the merging list.
				this.merging.Remove(match); }

			// Remove the node.
			match.RemoveFromTree();
		}

    }
}
