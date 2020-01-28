using System;
using System.Windows.Forms;
using System.IO;

namespace Manderijntje
{
    static class Program
    {
        public const string baseDir = "C:/Way2Go/";

        public const string nodes = "stationEurope.csv";
        public const string links = "linkEurope.csv";
        public const string routes = "routeEurope.csv";

        private const string binary = "visueelmodel_binary.txt";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()  
        {
            string homeDir = Path.GetDirectoryName(Application.ExecutablePath);

            string homeNodes = Path.Combine(homeDir, nodes);
            string homeLinks = Path.Combine(homeDir, links);
            string homeRoutes = Path.Combine(homeDir, routes);
            string homeBinary = Path.Combine(homeDir, binary);

            string baseNodes = Path.Combine(baseDir, nodes);
            string baseLinks = Path.Combine(baseDir, links);
            string baseRoutes = Path.Combine(baseDir, routes);
            string baseBinary = Path.Combine(baseDir, binary);

            if (!File.Exists(baseNodes) || !File.Exists(baseLinks) || !File.Exists(baseRoutes))
            {
                if (File.Exists(homeNodes) && File.Exists(homeLinks) && File.Exists(homeRoutes))
                {
                    if (!Directory.Exists(baseDir)) 
                    {
                        Directory.CreateDirectory(baseDir);
                    }

                    File.Copy(homeNodes, baseNodes, true);
                    File.Copy(homeLinks, baseLinks, true);
                    File.Copy(homeRoutes, baseRoutes, true);

                    File.Delete(homeNodes);
                    File.Delete(homeLinks);
                    File.Delete(homeRoutes);
                }
                else
                {
                    MessageBox.Show("One of the data files coudn't be opened or coudnt be found", "Error", MessageBoxButtons.OK);
                }
            }

            if (!File.Exists(baseBinary))
            {
                if (!Directory.Exists(baseDir))
                {
                    Directory.CreateDirectory(baseDir);
                }

                if (File.Exists(homeBinary))
                {
                    File.Copy(homeBinary, baseBinary, true);
                    File.Delete(homeBinary);
                } else
                {
                    MessageBox.Show("One of the data files coudn't be opened or coudnt be found", "Error", MessageBoxButtons.OK);
                }
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
