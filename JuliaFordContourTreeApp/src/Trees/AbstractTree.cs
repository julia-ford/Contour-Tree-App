using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp.Trees
{
    /// <summary>
    /// Implements some basic functionality that JoinTree, SplitTree, and ContourTree share.
    /// </summary>
    abstract class AbstractTree
    {
        public readonly float maxValue;
        public readonly float minValue;

        protected List<TreeNode> parentless;
        protected List<TreeNode> merging;
        protected List<TreeNode> childless;

		/// <summary>
		/// Does some initialization that is required for all trees.
		/// </summary>
		/// <param name="maxValue">the maximum value of a node in the tree</param>
		/// <param name="minValue">the minimum value of a node in the tree</param>
        public AbstractTree(float maxValue, float minValue)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;

            this.parentless = new List<TreeNode>();
            this.merging    = new List<TreeNode>();
            this.childless  = new List<TreeNode>();
        }

		/// <summary>
		/// Determines how many "slots" are needed to draw the tree.
		/// </summary>
		/// <returns>the number of slots needed to draw this tree</returns>
		public abstract int GetNumSlotsNeeded();

		/// <summary>
		/// Accessor for all of the parentless nodes in the tree.
		/// </summary>
		/// <returns>a list of the parentless nodes</returns>
		public List<TreeNode> GetParentless()
		{
			return new List<TreeNode>(this.parentless);
		}

		/// <summary>
		/// Accessor for all of the childless nodes in the tree.
		/// </summary>
		/// <returns>a list of the childless nodes</returns>
		public List<TreeNode> GetChildless()
		{
			return new List<TreeNode>(this.childless);
		}

		/// <summary>
		/// Accessor for all of the merging nodes in the tree.
		/// </summary>
		/// <returns>a list of the merge nodes</returns>
		public List<TreeNode> GetMerging()
		{
			return new List<TreeNode>(this.merging);
		}

		/// <summary>
		/// Accessor for all of the critical nodes in the tree.
		/// </summary>
		/// <returns>a list of the critical nodes in the tree</returns>
		public List<TreeNode> GetCritical()
		{
			List<TreeNode> list = new List<TreeNode>();
			list = list.Union(this.parentless).ToList();
			list = list.Union(this.childless ).ToList();
			list = list.Union(this.merging   ).ToList();

			return list;
		}

		/// <summary>
		/// Remove all non-critical nodes from the tree.
		/// </summary>
		public abstract void Simplify();

		/// <summary>
		/// Remove all non-critical nodes from the tree, except for the ones
		/// listed as being exempt.
		/// </summary>
		/// <param name="exemptions">the list of nodes to leave alone</param>
		public abstract void SimplifyExceptFor(List<TreeNode> exemptions);

    }
}
