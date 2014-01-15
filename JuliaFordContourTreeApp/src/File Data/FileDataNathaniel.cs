using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp.File_Data
{
	/// <summary>
	/// This file format is read in the same order as I0:
	/// <code>for(z) { for(y) { for(x) { read value; } } }</code>
	/// However, unlike I0, this format has no header at all. Dimensions must
	/// be calculated by traversing the data.
	/// A given line in the file contains a series of values separated by 
	/// spaces. Each line ends in a trailing space.
	/// Lines are separated by newline characters, and 'blocks' are separated
	/// by two newline characters. The file ends with two newline characters.
	/// </summary>
	class FileDataNathaniel : FileDataAbstract
	{
		public FileDataNathaniel(string fileData, int timestep) : base(fileData, timestep, FileType.Nathaniel)
		{
			// For each block,
			for (int z = 0; z < this.Zdim; z++) {
				// For each line in the block,
                for (int y = 0; y < this.Ydim; y++) {
					// For each value in the line,
                    for (int x = 0; x < this.Xdim; x++) {
						// get the next value,
                        String nextFloat = fileData.Substring(0, fileData.IndexOf(' '));
                        this.values[x, y, z] = Convert.ToSingle(nextFloat);
						// TODO: remove debugging statement.
						//Console.WriteLine("Value at (" + x + ", " + y + ", " + z + "): " + this.values[x, y, z] + ";");
						// x offset
						if (x + 1 < this.Xdim) {
							fileData = fileData.Substring(fileData.IndexOf(' ') + 1); }
						// y offset
						else if (y + 1 < this.Ydim) {
							fileData = fileData.Substring(fileData.IndexOf('\n') + 1); }
						// z offset
						else if (z + 1 < this.Zdim) {
							fileData = fileData.Substring(fileData.IndexOf('\n') + 3); }

                    }
                }
            }

            List<GridPoint> tempList = this.GetSortedGridPointList();
            this.maxValue = tempList[0].value;
            this.minValue = tempList.Last().value;
		}
	}
}
