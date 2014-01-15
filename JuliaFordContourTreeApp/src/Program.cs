using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuliaFordContourTreeApp
{
    /// <summary>
    /// This class creates an instance of the GUI and runs the program.
    /// It is also able to print debugging messages to the console if
    /// necessary.
    /// </summary>
    class Program
    {
        /// <summary>
        /// This is the function that is run when the program starts. It
        /// instantiates the GUI.
        /// </summary>
        /// <param name="args">
        /// command line arguments; I don't use any at the moment
        /// </param>
        [STAThread] static void Main(string[] args)
        {
            GUI gui = new GUI();
            gui.ShowDialog();
        }
    }
}
