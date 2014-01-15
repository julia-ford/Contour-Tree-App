using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp
{
    /// <summary>
    /// Represents a node in a tree.
    /// </summary>
    class TreeNode
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;
		public readonly int t;

		/// <summary>
		/// the value of the node
		/// </summary>
        public readonly float value;

        private List<TreeNode> parents;
        private List<TreeNode> children;

        /// <summary>
        /// Create a new tree node.
        /// </summary>
        /// <param name="point">the grid point to create the node from</param>
        public TreeNode(GridPoint point)
        {
            this.x = point.x;
            this.y = point.y;
            this.z = point.z;
			this.t = point.t;
            this.value = point.value;

            this.parents = new List<TreeNode>();
            this.children = new List<TreeNode>();
        }

        /// <summary>
        /// Copy constructor for tree node.
        /// Copies x, y, z, and value.
        /// </summary>
        /// <param name="other">the node to copy.</param>
        public TreeNode(TreeNode other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
			this.t = other.t;
            this.value = other.value;

            this.parents = new List<TreeNode>();
            this.children = new List<TreeNode>();
        }

		/// <summary>
		/// Determines how many parentless ancestors this node has.
		/// </summary>
		/// <returns>
		/// the number of parentless ancestors this node has,
		/// if this node has ancestors; 1 if this node is parentless
		/// </returns>
		public int GetNumParentlessAncestors()
		{
			if (this.parents.Count == 0) { return 1; }

			int numParentlessAncestors = 0;

			List<TreeNode> currentLevel = new List<TreeNode>(this.parents);

			while (currentLevel.Count > 0) {

				List<TreeNode> nextLevel = new List<TreeNode>();

				foreach (TreeNode node in currentLevel) {
					if (node.parents.Count == 0) {
						numParentlessAncestors += 1; }
					else {
						nextLevel = nextLevel.Union(node.parents).ToList(); } }

				currentLevel = nextLevel; }

			return numParentlessAncestors;
		}
		
		/// <summary>
		/// Determines how many childless descendents this node has.
		/// </summary>
		/// <returns>
		/// the number of childless descendents this node has,
		/// if this node has ancestors; 1 if this node is parentless
		/// </returns>
		public int GetNumChildlessDescendents()
		{
			if (this.children.Count == 0) { return 1; }

			int numChildlessDescendents = 0;

			List<TreeNode> currentLevel = new List<TreeNode>(this.children);

			while (currentLevel.Count > 0) {

				List<TreeNode> nextLevel = new List<TreeNode>();

				foreach (TreeNode node in currentLevel) {
					if (node.children.Count == 0) {
						numChildlessDescendents += 1; }
					else {
						nextLevel = nextLevel.Union(node.children).ToList(); } }

				currentLevel = nextLevel; }

			return numChildlessDescendents;
		}

		/// <summary>
		/// Accessor for the parents of this node.
		/// </summary>
		/// <returns></returns>
		public List<TreeNode> GetParents()
		{
			return new List<TreeNode>(this.parents);
		}

		/// <summary>
		/// Accessor for the children of this node.
		/// </summary>
		/// <returns></returns>
		public List<TreeNode> GetChildren()
		{
			return new List<TreeNode>(this.children);
		}

        /// <summary>
        /// Determines if this node's x, y, z, and value values match another node's values.
        /// </summary>
        /// <param name="other">the node to check against</param>
        /// <returns>true if they match; false otherwise</returns>
        public bool Matches(TreeNode other)
        {
            return (this.x == other.x) && (this.y == other.y) && (this.z == other.z) && (this.t == other.t);
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

        /// <summary>
        /// Check if two points are considered adjacent to each other.
        /// </summary>
        /// <param name="other">the other point</param>
        /// <returns>true if they're adjacent; false otherwise</returns>
        public bool IsAdjacentTo(GridPoint other)
        {
            
			int diff_x = this.x - other.x;
			int diff_y = this.y - other.y;
			int diff_z = this.z - other.z;
			int diff_t = this.t - other.t;

			if (Math.Abs(diff_x) > 1 || Math.Abs(diff_y) > 1 ||
				Math.Abs(diff_z) > 1 || Math.Abs(diff_z) > 1) { return false; }

			if (diff_x < 0 || diff_y < 0 || diff_z < 0 || diff_t < 0) {
				diff_x *= -1;
				diff_y *= -1;
				diff_z *= -1;
				diff_t *= -1; }

			if (diff_x < 0 || diff_y < 0 || diff_z < 0 || diff_t < 0) {
				return false; }

			return true;
        }
        
        /// <summary>
        /// Check if two tree nodes are considered adjacent to each other.
        /// </summary>
        /// <param name="other">the other point</param>
        /// <returns>true if they're adjacent; false otherwise</returns>
        public bool IsAdjacentTo(TreeNode other)
        {
            
			int diff_x = this.x - other.x;
			int diff_y = this.y - other.y;
			int diff_z = this.z - other.z;
			int diff_t = this.t - other.t;

			if (Math.Abs(diff_x) > 1 || Math.Abs(diff_y) > 1 ||
				Math.Abs(diff_z) > 1 || Math.Abs(diff_z) > 1) { return false; }

			if (diff_x < 0 || diff_y < 0 || diff_z < 0 || diff_t < 0) {
				diff_x *= -1;
				diff_y *= -1;
				diff_z *= -1;
				diff_t *= -1; }

			if (diff_x < 0 || diff_y < 0 || diff_z < 0 || diff_t < 0) {
				return false; }

			return true;
        }

        /// <summary>
        /// Determines whether or not this node is critical.
        /// </summary>
        /// <returns>false if the node is critical; true if it is not</returns>
        public bool IsNonCritical()
        {
            return this.parents.Count == 1 && this.children.Count == 1;
        }

        /// <summary>
        /// Adds the specified node as a parent of this node.
        /// </summary>
        /// <param name="other">the node to make a parent of this node</param>
        public void AddParent(TreeNode other)
        {
            this.parents.Add(other);
        }

        /// <summary>
        /// Adds the specified node as a child of this node.
        /// </summary>
        /// <param name="other">the node to make a child of this node</param>
        public void AddChild(TreeNode other)
        {
            this.children.Add(other);
        }

        /// <summary>
        /// Removes this node from whatever tree it appears in by attatching
        /// its parent to its child.
        /// </summary>
        public void RemoveFromTree()
        {
			foreach (TreeNode parent in this.parents) {
				parent.children.Remove(this);
				foreach (TreeNode child in this.children) {
					parent.children.Add(child); } }

			foreach (TreeNode child in this.children) {
				child.parents.Remove(this);
				foreach (TreeNode parent in this.parents) {
					child.parents.Add(parent); } }

            this.parents = null;
            this.children = null;
        }

    }
}
