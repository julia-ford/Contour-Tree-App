using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp
{
    /// <summary>
    /// This class represents a value in a data file.
    /// </summary>
    class GridPoint
	{
		//===================================================================//
		//                           Variables                               //
		//===================================================================//

        /// <summary>
        /// the x coordinate of the point in the file
        /// </summary>
        public readonly int x;
        /// <summary>
        /// the y coordinate of the point in the file
        /// </summary>
        public readonly int y;
        /// <summary>
        /// the z coordinate of the point in the file
        /// </summary>
        public readonly int z;
		/// <summary>
		/// the time coordinate of the point; indicates what file it came from
		/// </summary>
		public readonly int t;
        /// <summary>
        /// the value of the point at the specified coordinates
        /// </summary>
        public readonly float value;

		//===================================================================//
		//                          Constructors                             //
		//===================================================================//

        /// <summary>
        /// Creates a new GridPoint.
        /// </summary>
        /// <param name="x">the x coordinate of the point in the file</param>
		/// <param name="y">the y coordinate of the point in the file</param>
		/// <param name="z">the z coordinate of the point in the file</param>
		/// <param name="t">the time coordinate of the point; indicates what file it came from</param>
        /// <param name="value">the value of the point</param>
        public GridPoint(int x, int y, int z, int t, float value)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.t = t;
            this.value = value;
        }

		//===================================================================//
		//                            Getters                                //
		//===================================================================//

        /// <summary>
        /// Compares two AbstractGridPoints. Used for sorting a list of them.
        /// </summary>
        /// <param name="p1">the first point</param>
        /// <param name="p2">the second point</param>
        /// <returns>
        /// -1 if p1's value is greater;
        ///  1 if p2's value is greater;
        ///  0 if they are the same
        ///  </returns>
        public static int Compare(GridPoint p1, GridPoint p2)
        {
            if (p1.value > p2.value) { return -1; }
            if (p1.value < p2.value) { return  1; }
            return 0;
        }

		//===================================================================//
		//                            Booleans                               //
		//===================================================================//

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
        /// Check if two points are considered adjacent to each other.
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

    }
}
