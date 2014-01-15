using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp
{
    class BlobOfNodes : List<TreeNode>
    {
        //===================================================================//
        //                            Variables                              //
        //===================================================================//

        /// <summary>
        /// the TreeNode that was most recently added to this BlobOfNodes
        /// </summary>
        private TreeNode mostRecentlyAddedNode;

        //===================================================================//
        //                          Constructors                             //
        //===================================================================//

        /// <summary>
        /// Create a new BlobOfNodes.
        /// </summary>
        /// <param name="node">the first node to add to this BlobOfNodes</param>
        public BlobOfNodes(TreeNode node) : base()
        {
            this.Add(node);
        }

        /// <summary>
        /// Merges a list of blobs into a new blob.
        /// </summary>
        /// <param name="node">
        /// the node that will be the ne blob's most recently added node
        /// </param>
        /// <param name="otherBlobs">the list of blobs to merge</param>
        public BlobOfNodes(TreeNode recentNode, List<BlobOfNodes> otherBlobs) : base()
        {
            foreach (BlobOfNodes blob in otherBlobs) {
                foreach (TreeNode node in blob) {
                    ((List<TreeNode>)this).Add(node); } }

            this.Add(recentNode);
        }

        //===================================================================//
        //                             Getters                               //
        //===================================================================//

        /// <summary>
        /// Accessor for the most recently added node.
        /// </summary>
        /// <returns>the most recently added node</returns>
        public TreeNode GetMostRecentlyAddedNode()
        {
            return this.mostRecentlyAddedNode;
        }

        //===================================================================//
        //                             Booleans                              //
        //===================================================================//

        /// <summary>
        /// Checks whether this BlobOfNodes is adjacent to the given TreeNode.
        /// </summary>
        /// <param name="other">the given TreeNode</param>
        /// <returns>true if they are adjacent; false otherwise</returns>
        public bool IsAdjacentTo(TreeNode other)
        {
            foreach (TreeNode node in this)
            {
                if (node.IsAdjacentTo(other))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this blob contains a TreeNode that matches the
        /// given node.
        /// </summary>
        /// <param name="other">the node to check for a match</param>
        /// <returns>true if this blob contains a match; false otherwise</returns>
        public bool ContainsMatch(TreeNode other)
        {
            foreach (TreeNode node in this)
            {
                if (node.Matches(other)) {
                    return true;
                }
            }
            return false;
        }

        //===================================================================//
        //                             Actions                               //
        //===================================================================//

        /// <summary>
        /// Add the node to this BlobOfNodes and update the mostRecentlyAddedNode.
        /// </summary>
        /// <param name="node">the node to add</param>
        public new void Add(TreeNode node)
        {
            this.mostRecentlyAddedNode = node;
            ((List<TreeNode>)this).Add(node);
        }

    }
}
