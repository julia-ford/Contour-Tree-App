using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp
{
	/// <summary>
	/// A simpler node than TreeNode, ContourNode does not track the 
	/// "direction" of connections between nodes, ie child-to parent,
	/// parent-to-child. It only knows its "trunk" node and the branch(es)
	/// that come off of itself.
	/// </summary>
	/// <remarks>
	/// The reason I needed this class in addition to TreeNode is that contour
	/// tree branches can go in all kind of crazy directions. Therefore,
	/// tracking parents and children gets really difficult.
	/// </remarks>
	class ContourNode
	{
		//===================================================================//
		//                           Variables                               //
		//===================================================================//

		public readonly int x;
		public readonly int y;
		public readonly int z;
		public readonly int t;

		/// <summary>
		/// the value of the node
		/// </summary>
		public readonly float value;

		/// <summary>
		/// The trunk node from which this node branches off.
		/// </summary>
		private ContourNode trunk;

		/// <summary>
		/// The nodes that branch off from this node.
		/// </summary>
		private List<ContourNode> branches;

		//===================================================================//
		//                          Constructors                             //
		//===================================================================//

		/// <summary>
		/// Create a new ContourNode based on an existing TreeNode.
		/// </summary>
		/// <param name="node">the node to copy</param>
		public ContourNode(TreeNode node)
		{
			this.x = node.x;
			this.y = node.y;
			this.z = node.z;
			this.t = node.t;
			this.value = node.value;

			this.branches = new List<ContourNode>();
		}

		//===================================================================//
		//                            Getters                                //
		//===================================================================//

		/// <summary>
		/// Accessor for the trunk node of this node.
		/// </summary>
		/// <returns>the trunk node</returns>
		public ContourNode GetTrunk()
		{
			return this.trunk;
		}

		/// <summary>
		/// Accessor for the list of branches that diverge from this node.
		/// </summary>
		/// <returns>the list of branches that diverge from this node</returns>
		public List<ContourNode> GetBranches()
		{
			return new List<ContourNode>(this.branches);
		}

		/// <summary>
		/// Gets the number of nodes that branch off from this node and have
		/// no branches of their own.
		/// </summary>
		/// <returns>the number of leaves coming from this node and its branches</returns>
		public int GetNumLeaves()
		{
			if (this.branches.Count == 0) { return 1; }
			
			int numLeaves = 0;

			List<ContourNode> currentLevel = new List<ContourNode>(this.branches);

			while (currentLevel.Count > 0) {

				List<ContourNode> nextLevel = new List<ContourNode>();

				foreach (ContourNode node in currentLevel) {
					if (node.branches.Count == 0) {
						numLeaves += 1; }
					else {
						nextLevel = nextLevel.Union(node.branches).ToList(); } }

				currentLevel = nextLevel; }

			return numLeaves;
		}

		//===================================================================//
		//                            Booleans                               //
		//===================================================================//

		/// <summary>
		/// Checks if this node matches another TreeNode.
		/// </summary>
		/// <param name="other">the TreeNode to check</param>
		/// <returns>true if they match; false otherwise</returns>
		public bool Matches(TreeNode other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z && this.t == other.t;
		}

		/// <summary>
		/// Checks if this node matches another ContourNode.
		/// </summary>
		/// <param name="other">the ContourNode to check</param>
		/// <returns>true if they match; false otherwise</returns>
		public bool Matches(ContourNode other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z && this.t == other.t;
		}

		//===================================================================//
		//                            Actions                                //
		//===================================================================//

		/// <summary>
		/// Setter for the trunk node of this node.
		/// </summary>
		/// <param name="other">the node to set as the trunk</param>
		public void SetTrunk(ContourNode other)
		{
			this.trunk = other;
		}

		/// <summary>
		/// Adds a given node to the list of branches that diverge from this node.
		/// </summary>
		/// <param name="other">the node to add to the list of branches</param>
		public void AddBranch(ContourNode other)
		{
			this.branches.Add(other);
		}

	}
}
