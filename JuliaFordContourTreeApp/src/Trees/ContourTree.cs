using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp.Trees
{
	class ContourTree
	{
		//===================================================================//
		//                            Variables                              //
		//===================================================================//

		public readonly float maxValue;
		public readonly float minValue;

		private ContourNode root;

		private ContourNode topMostLeaf;
		private ContourNode botMostLeaf;

		//===================================================================//
		//                          Constructors                             //
		//===================================================================//

		/// <summary>
		/// Create a new ContourTree from the specified JoinTree and SplitTree.
		/// </summary>
		/// <remarks>
		/// A small warning: this will not work properly if there are only two
		/// nodes in both the split and join trees.
		/// </br></br>
		/// A different small warning: at present, the contour tree constructor
		/// does not actually fill the "merging" list with its merge nodes.
		/// I didn't need it filled out for my purposes, so I didn't make it work.
		/// </remarks>
		/// <param name="joinTree">the join tree</param>
		/// <param name="splitTree">the split tree</param>
		public ContourTree(JoinTree joinTree, SplitTree splitTree)
		{
			this.maxValue = joinTree.maxValue;
			this.minValue = joinTree.minValue;

			 JoinTree cloneJoinTree  = new  JoinTree( joinTree);
			SplitTree cloneSplitTree = new SplitTree(splitTree);

			List<TreeNode>   upLeaves = new List<TreeNode>( cloneJoinTree.GetParentless());
			List<TreeNode> downLeaves = new List<TreeNode>(cloneSplitTree.GetChildless());

			List<BlobOfContourNodes> blobList = new List<BlobOfContourNodes>();

			//===============================================================//
			// Add parentless nodes from join tree.
			//===============================================================//

			// For each of the parentless nodes in the join tree,
			foreach (TreeNode upLeaf in upLeaves)
			{
				// copy the node and add it to the contour tree.
				ContourNode newNode = new ContourNode(upLeaf);
				if (this.topMostLeaf == null) { this.topMostLeaf = newNode; }

				// Check if the child of the parentless node has already been
				// added to the contour tree.
				bool matchFound = false;
				int tempIndex = 0;
				while (!matchFound && tempIndex < blobList.Count) {
					// If a match is found,
					if (blobList[tempIndex].ContainsMatch(upLeaf.GetChildren()[0])) {
						// set the flag,
						matchFound = true;
						// make the parentless node a branch of the match,
						blobList[tempIndex].GetMostRecentlyAddedNode().AddBranch(newNode);
						// make the match the trunk of the parentless node,
						newNode.SetTrunk(blobList[tempIndex].GetMostRecentlyAddedNode());
						// and add the parentless node to the correct blob.
						((List<ContourNode>)blobList[tempIndex]).Add(newNode); }
					tempIndex++; }

				// If a match was not found, make a new blob.
				if (!matchFound) {
					// Create a BlobOfContourNodesnew blob and a new node for the trunk.
					BlobOfContourNodes newBlob = new BlobOfContourNodes(newNode);
					ContourNode newTrunk = new ContourNode(upLeaf.GetChildren()[0]);
					// Set up branch / trunk relationships.
					newNode.SetTrunk(newTrunk);
					newTrunk.AddBranch(newNode);
					// Add the new node to the new blob.
					newBlob.Add(newTrunk);
					// Add the new blob to the blob list.
					blobList.Add(newBlob); }

				// Remove the parentless node from both the split tree and the join tree.
				cloneSplitTree.RemoveMatch(upLeaf);
				 cloneJoinTree.RemoveMatch(upLeaf);
			} // end of adding parentless nodes to the contour tree

			//===============================================================//
			// Add childless nodes from split tree.
			//===============================================================//

			foreach (TreeNode downLeaf in downLeaves)
			{
				// Copy the node and add it to the contour tree.
				ContourNode newNode = new ContourNode(downLeaf);
				if (this.botMostLeaf == null) { this.botMostLeaf = newNode; }
				
				// Check if the parent of the childless node has already been
				// added to the contour tree.
				bool matchFound = false;
				int tempIndex = 0;
				while (!matchFound && tempIndex < blobList.Count) {
					// If a match is found,
					if (blobList[tempIndex].ContainsMatch(downLeaf.GetParents()[0])) {
						// set the flag,
						matchFound = true;
						// make the childless node a branch of the match,
						blobList[tempIndex].GetMostRecentlyAddedNode().AddBranch(newNode);
						// make the match the trunk of the childless node,
						newNode.SetTrunk(blobList[tempIndex].GetMostRecentlyAddedNode());
						// and add the childless node to the correct blob.
						((List<ContourNode>)blobList[tempIndex]).Add(newNode); }
					tempIndex++; }

				// If a match was not found, make a new blob.
				if (!matchFound) {
					// Create a new blob and a new node for the branch.
					BlobOfContourNodes newBlob = new BlobOfContourNodes(newNode);
					ContourNode newTrunk = new ContourNode(downLeaf.GetParents()[0]);
					// Set up trunk / branch relationships.
					newNode.SetTrunk(newTrunk);
					newTrunk.AddBranch(newNode);
					// Add the new node to the new blob.
					newBlob.Add(newTrunk);
					// Add the new blob to the blob list.
					blobList.Add(newBlob); }

				// Remove the childless node from both the split tree and the join tree.
				cloneSplitTree.RemoveMatch(downLeaf);
				 cloneJoinTree.RemoveMatch(downLeaf);
			} // end of adding childless nodes to the contour tree

			//===============================================================//
			// Add additional nodes from join tree.
			//===============================================================//

			// Add nodes from the Join tree until the join tree has no more
			// branches to add and only the trunk remains.
			// Please note: this means the node currently being examined will
			// always have a valid, non-null child.
			while (cloneJoinTree.GetParentless().Count > 1)
			{
				// Store the nodes we'll be working with.
				List<TreeNode> oldLevel = cloneJoinTree.GetParentless();

				foreach (TreeNode oldNode in oldLevel)
				{
					// Store the child of the node being worked with.
					TreeNode oldChild = oldNode.GetChildren()[0];

					// The blob that contains the current node.
					BlobOfContourNodes containingBlob = null;
					// The blob that contains the current node's child.
					BlobOfContourNodes trunkBlob = null;

					// Look through the blobs until both the containingBlob and
					// trunkBlob are found, or all of the blobs have been examined.
					int index = 0;
					while ((containingBlob == null || trunkBlob == null) && index < blobList.Count)
					{
						// Store current blob and its most recently added node in
						// temporary variables to avoid extra function calls.
						BlobOfContourNodes currentBlob = blobList[index];
						ContourNode mostRecent = currentBlob.GetMostRecentlyAddedNode();

						// Check if current blob is containingBlob.
						if (mostRecent.Matches(oldNode)) {
							containingBlob = currentBlob; }

						// Check if current blob is childBlob.
						else if (oldChild.Matches(mostRecent)) {
							trunkBlob = currentBlob; }

						index++;
					}

					// If trunkBlob was not found, make a new node for the trunk
					// and add it to the contour tree and the containingBlob.
					if (trunkBlob == null)
					{
						ContourNode newNode = containingBlob.GetMostRecentlyAddedNode();
						ContourNode newTrunk = new ContourNode(oldChild);

						// Set trunk / branch relations.
						newNode.SetTrunk(newTrunk);
						newTrunk.AddBranch(newNode);

						// Add newTrunk to the blob.
						containingBlob.Add(newTrunk);
					}

					// If the trunkBlob was found, merge the trunkBlob and
					// containingBlob blobs.
					else
					{
						ContourNode newNode = containingBlob.GetMostRecentlyAddedNode();
						ContourNode newTrunk = trunkBlob.GetMostRecentlyAddedNode();

						// Set branch / trunk relations.
						newNode.SetTrunk(newTrunk);
						newTrunk.AddBranch(newNode);

						// Make a list of the two blobs so the blob merge
						// constructor can be used.
						List<BlobOfContourNodes> paramList = new List<BlobOfContourNodes>();
						paramList.Add(containingBlob);
						paramList.Add(trunkBlob);

						// This prevents the newTrunk from being double-listed
						// in the new blob.
						trunkBlob.Remove(newTrunk);

						// Make the new blob, containing all the nodes from the
						// trunk and containing blobs, with newTrunk as root.
						BlobOfContourNodes newBlob = new BlobOfContourNodes(newTrunk, paramList);

						// Update the blob list.
						blobList.Remove(containingBlob);
						blobList.Remove(trunkBlob);
						blobList.Add(newBlob);
					}

					// Either way, remove the node from both trees.
					cloneSplitTree.RemoveMatch(oldNode);
					cloneJoinTree.RemoveMatch(oldNode);
				}
			} // end of adding additional nodes from the join tree

			//===============================================================//
			// Add remaining nodes from split tree.
			//===============================================================//

			// Now just add all the remaining nodes in the split tree, from bottom to top.
			while (cloneSplitTree.GetChildless().Count > 0)
			{
				// Store the node we'll be working with.
				TreeNode oldNode = cloneSplitTree.GetChildless()[0];
				// Also store the parent of the node being worked with, unless
				// it has no parent, in which case we do different stuff later.
				TreeNode oldParent = oldNode.GetParents().Count == 0 ? null : oldNode.GetParents()[0];

				// The blob that contains the current node.
				BlobOfContourNodes containingBlob = null;
				// The blob that contains the current node's parent,
				// unless there was no parent.
				BlobOfContourNodes trunkBlob = null;

				// Look through the blobs until both the containingBlob and
				// trunkBlob are found, or all the blobs have been examined,
				// or containingBlob is found and there was no parent.
				int index = 0;
				while ((containingBlob == null || (trunkBlob == null && oldParent != null)) && index < blobList.Count)
				{
					// Store current blob and its most recently added node in
					// temporary variables to avoid extra function calls.
					BlobOfContourNodes currentBlob = blobList[index];
					ContourNode mostRecent = currentBlob.GetMostRecentlyAddedNode();

					// Check if current blob is containingBlob.
					if (mostRecent.Matches(oldNode)) {
						containingBlob = currentBlob; }

					// Check if current blob is trunkBlob.
					else if (oldParent != null && oldParent.Matches(mostRecent)){
						trunkBlob = currentBlob; }

					index++; }

				// If trunkBlob was not found and a parent node exists,
				// make a new node for the parent and add it to the contour
				// tree and the containingBlob.
				if (trunkBlob == null && oldParent != null) {
					ContourNode newNode = containingBlob.GetMostRecentlyAddedNode();
					ContourNode newTrunk = new ContourNode(oldParent);

					// Set trunk / branch relations.
					newNode.SetTrunk(newTrunk);
					newTrunk.AddBranch(newNode);

					// Add newParent to the blob.
					containingBlob.Add(newTrunk); }

				// If the trunkBlob was found, merge the trunkBlob and
				// containingBlob blobs.
				else if (trunkBlob != null) {
					ContourNode newNode = containingBlob.GetMostRecentlyAddedNode();
					ContourNode newTrunk = trunkBlob.GetMostRecentlyAddedNode();

					// Set branch / trunk relations.
					newNode.SetTrunk(newTrunk);
					newTrunk.AddBranch(newNode);

					// Make a list of the two blobs so the blob merge
					// constructor can be used.
					List<BlobOfContourNodes> paramList = new List<BlobOfContourNodes>();
					paramList.Add(containingBlob);
					paramList.Add(    trunkBlob);

					// This prevents the newParent from being double-listed
					// in the new blob.
					trunkBlob.Remove(newTrunk);

					// Make the new blob, containing all the nodes from the
					// trunk and containing blobs, with newTrunk as root.
					BlobOfContourNodes newBlob = new BlobOfContourNodes(newTrunk, paramList);

					// Update the blob list.
					blobList.Remove(containingBlob);
					blobList.Remove(trunkBlob);
					blobList.Add(newBlob); }

				// Otherwise, if there was no parent node, store the current
				// node as the "root" of the contour tree.
				else { this.root = containingBlob.GetMostRecentlyAddedNode(); }

				// No matter what, remove the node from both trees.
				cloneSplitTree.RemoveMatch(oldNode);
				cloneJoinTree.RemoveMatch(oldNode);
			}
		}

		//===================================================================//
		//                             Getters                               //
		//===================================================================//

		/// <summary>
		/// Accessor for the root of the tree.
		/// </summary>
		/// <returns>the root of the tree</returns>
		public ContourNode GetRoot()
		{
			return this.root;
		}

		/// <summary>
		/// Accessor for the leaf with the greatest value.
		/// </summary>
		/// <returns>the ContourNode with the greatest value in the tree</returns>
		public ContourNode GetTopMostLeaf()
		{
			return this.topMostLeaf;
		}

		/// <summary>
		/// Accessor for the leaf with the smallest value.
		/// </summary>
		/// <returns>the ContourNode with the smallest value in the tree</returns>
		public ContourNode GetBotMostLeaf()
		{
			return this.botMostLeaf;
		}

		/// <summary>
		/// Accessor for the maximum value in the tree.
		/// </summary>
		/// <returns>the maximum value in the tree</returns>
		public float GetMaxValue()
		{
			return this.maxValue;
		}

		/// <summary>
		/// Accessor for the minimum value in the tree.
		/// </summary>
		/// <returns>the minimum value in the tree</returns>
		public float GetMinValue()
		{
			return this.minValue;
		}

		/// <summary>
		/// Determine how many horizontal "slots" this contour tree needs to
		/// draw all its nodes.
		/// </summary>
		/// <returns>the number of slots needed</returns>
		public int GetNumSlotsNeeded()
		{
			// slots needed on specific sides
			int topLeSlotsNeeded = 0;
			int topRiSlotsNeeded = 0;
			int botLeSlotsNeeded = 0;
			int botRiSlotsNeeded = 0;

			ContourNode currentTopMainPathNode = this.topMostLeaf;
			ContourNode currentBotMainPathNode = this.botMostLeaf;

			int currentTopDir = -1;
			int currentBotDir =  1;

			while (currentTopMainPathNode.GetTrunk() != this.root || currentBotMainPathNode.GetTrunk() != this.root)
			{
				// Top half of contour tree:
				if (currentTopMainPathNode.GetTrunk() != this.root) {
					ContourNode nextTopMainPathNode = currentTopMainPathNode.GetTrunk();
					foreach (ContourNode branch in nextTopMainPathNode.GetBranches()) {
						if (branch != currentTopMainPathNode) {
							if (currentTopDir == -1) {
								topLeSlotsNeeded += branch.GetNumLeaves(); }
							else { topRiSlotsNeeded += branch.GetNumLeaves(); }
							// toggle which side the next branch will get added to
							currentTopDir *= -1; } }
					currentTopMainPathNode = nextTopMainPathNode; }

				// Bottom half of contour tree:
				if (currentBotMainPathNode.GetTrunk() != this.root) {
					ContourNode nextBotMainPathNode = currentBotMainPathNode.GetTrunk();
					foreach (ContourNode branch in nextBotMainPathNode.GetBranches()) {
						if (branch != currentBotMainPathNode) {
							if (currentBotDir == -1) {
								botLeSlotsNeeded += branch.GetNumLeaves(); }
							else { botRiSlotsNeeded += branch.GetNumLeaves(); }
							// toggle which side the next branch will get added to
							currentBotDir *= -1; } }
					currentBotMainPathNode = nextBotMainPathNode; }
			}

			// Deal with any additional branches that come off the root.
			foreach (ContourNode branch in this.root.GetBranches()) {
				// Make sure this branch isn't one of the ones we already looked at.
				if (branch != currentTopMainPathNode && branch != currentBotMainPathNode) {
					// If branch is above the root,
					if (branch.value > root.value) {
						if (currentTopDir == -1) {
							topLeSlotsNeeded += branch.GetNumLeaves(); }
						else {
							topRiSlotsNeeded += branch.GetNumLeaves(); } }
					// If branch is below the root,
					else {
						if (currentBotDir == -1) {
							botLeSlotsNeeded += branch.GetNumLeaves(); }
						else {
							botRiSlotsNeeded += branch.GetNumLeaves(); } } } }

			return Math.Max(Math.Max(topLeSlotsNeeded, topRiSlotsNeeded), Math.Max(botLeSlotsNeeded, botRiSlotsNeeded)) * 2 + 1;
		}

		/// <summary>
		/// Determines how many "layers" of nodes there are in this tree.
		/// </summary>
		/// <returns>the number of layers</returns>
		public int GetNumLayers()
		{
			// Counter.
			int numLayers = 0;

			// Create a list of nodes in the first "layer" of the tree.
			List<ContourNode> currentLayer = new List<ContourNode>();
			currentLayer.Add(this.root);

			while (currentLayer.Count > 0)
			{
				// Construct the next layer of the tree.
				List<ContourNode> nextLayer = new List<ContourNode>();
				foreach (ContourNode node in currentLayer) {
					nextLayer = nextLayer.Union(node.GetBranches()).ToList(); }

				// Count this layer.
				numLayers++;

				// Look at the next layer.
				currentLayer = nextLayer;
			}

			return numLayers;
		}

		// A point is wirtten like this: P(pos=(1,2,3), val=47.2, trunk=(4,5,6), branches=((7,8,9), (10,11,12), (13,14,15))),
		public string[] GetStringOutput()
		{
			string[] output = new string[GetNumLayers()];
			
			// Counter.
			int currentIndex = 0;

			// Create a list of nodes in the first "layer" of the tree.
			List<ContourNode> currentLayer = new List<ContourNode>();
			currentLayer.Add(this.root);

			while (currentLayer.Count > 0)
			{
				// Make sure the current string is initializaed ok.
				output[currentIndex] = "";

				// Construct the next layer of the tree.
				List<ContourNode> nextLayer = new List<ContourNode>();

				// Look at each node.
				foreach (ContourNode node in currentLayer) {
					// Write about the node.
					output[currentIndex] += "P(pos=(" + node.x + "," + node.y + "," + node.z + "), val=" + node.value + ", "
						+ "trunk=(" + ((node.GetTrunk() == null) ? "null" : node.GetTrunk().x + "," + node.GetTrunk().y + "," + node.GetTrunk().z) + "), "
						+ "branches=(";

					// if the node has no branches, write "none"
					if (node.GetBranches().Count == 0) { output[currentIndex] += "none), "; }

					// otherwise, write about each branch
					else {
						// first branch is a special case
						output[currentIndex] += "(" + node.GetBranches()[0].x + "," + node.GetBranches()[0].y + "," + node.GetBranches()[0].z + ")";
						List<ContourNode> otherBranches = new List<ContourNode>(node.GetBranches());
						otherBranches.Remove(node.GetBranches()[0]);
						foreach (ContourNode otherBranch in otherBranches) {
							output[currentIndex] += ", (" + otherBranch.x + "," + otherBranch.y + "," + otherBranch.z + ")"; }
						output[currentIndex] += "), "; }

					// Add the node's children to the next layer.
					nextLayer = nextLayer.Union(node.GetBranches()).ToList(); }

				// Count this layer.
				currentIndex++;

				// Look at the next layer.
				currentLayer = nextLayer;
			}

			return output;
		}

	}
}
