using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp
{
    class BlobOfContourNodes : List<ContourNode>
    {
        //===================================================================//
        //                            Variables                              //
        //===================================================================//

        /// <summary>
		/// the ContourNode that was most recently added to this BlobOfContourNodes
        /// </summary>
        private ContourNode mostRecentlyAddedNode;

        //===================================================================//
        //                          Constructors                             //
        //===================================================================//

        /// <summary>
		/// Create a new BlobOfContourNodes.
        /// </summary>
		/// <param name="node">the first node to add to this BlobOfContourNodes</param>
        public BlobOfContourNodes(ContourNode node) : base()
        {
            this.Add(node);
        }

        /// <summary>
        /// Merges a list of blobs into a new blob.
        /// </summary>
        /// <param name="node">
        /// the node that will be the new blob's most recently added node
        /// </param>
        /// <param name="otherBlobs">the list of blobs to merge</param>
        public BlobOfContourNodes(ContourNode recentNode, List<BlobOfContourNodes> otherBlobs) : base()
        {
            foreach (BlobOfContourNodes blob in otherBlobs) {
                foreach (ContourNode node in blob) {
                    ((List<ContourNode>)this).Add(node); } }

            this.Add(recentNode);
        }

        //===================================================================//
        //                             Getters                               //
        //===================================================================//

        /// <summary>
        /// Accessor for the most recently added node.
        /// </summary>
        /// <returns>the most recently added node</returns>
		public ContourNode GetMostRecentlyAddedNode()
        {
            return this.mostRecentlyAddedNode;
        }

        //===================================================================//
        //                             Booleans                              //
        //===================================================================//

        /// <summary>
		/// Determines whether this blob contains a ContourNode that matches
        /// the given TreeNode.
        /// </summary>
        /// <param name="other">the node to check for a match</param>
        /// <returns>true if this blob contains a match; false otherwise</returns>
        public bool ContainsMatch(TreeNode other)
        {
			foreach (ContourNode node in this) {
                if (node.Matches(other)) {
                    return true; } }
            return false;
        }
		
        /// <summary>
		/// Determines whether this blob contains a ContourNode that matches
		/// the given ContourNode.
        /// </summary>
        /// <param name="other">the node to check for a match</param>
        /// <returns>true if this blob contains a match; false otherwise</returns>
		public bool ContainsMatch(ContourNode other)
        {
			foreach (ContourNode node in this) {
                if (node.Matches(other)) {
                    return true; } }
            return false;
        }

        //===================================================================//
        //                             Actions                               //
        //===================================================================//

        /// <summary>
		/// Add the node to this BlobOfContourNodes and update the mostRecentlyAddedNode.
        /// </summary>
        /// <param name="node">the node to add</param>
		public new void Add(ContourNode node)
        {
            this.mostRecentlyAddedNode = node;
			((List<ContourNode>)this).Add(node);
        }

    }
}
