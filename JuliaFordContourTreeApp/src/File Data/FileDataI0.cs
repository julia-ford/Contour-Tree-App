using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp.File_Data
{
    /// <summary>
    /// I0 format reads data in the order:
    /// <code>for(z) { for(y) { for(x) { read data; } } }</code>
    /// </summary>
    class FileDataI0 : FileDataAbstract
    {
        /// <summary>
        /// Create a new FileData object in I0 format.
        /// </summary>
        /// <param name="fileData">the data from the file</param>
		/// <param name="timestep">the time coordinate of the file data</param>
        public FileDataI0(string fileData, int timestep) : base(fileData, timestep, FileType.I0)
        {
            // ignore the dimensions
            fileData = fileData.Substring(fileData.IndexOf('\n') + 1);

            for (int z = 0; z < this.Zdim; z++) {
                for (int y = 0; y < this.Ydim; y++) {
                    for (int x = 0; x < this.Xdim; x++) {
                        String nextFloat = fileData.Substring(0, fileData.IndexOf(','));
                        this.values[x, y, z] = Convert.ToSingle(nextFloat);
                        if (fileData.IndexOf(',') + 1 < fileData.Length) {
                            fileData = fileData.Substring(fileData.IndexOf(',') + 1);
                        }
                    }
                }
            }

            List<GridPoint> tempList = this.GetSortedGridPointList();
            this.maxValue = tempList[0].value;
            this.minValue = tempList.Last().value;
        }
    }
}
