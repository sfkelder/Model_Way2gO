using System;
using System.Windows.Forms;
using System.IO;

namespace manderijntje
{
    static class Program
    {
        public const string baseDir = "C:/Way2Go/";
        public const bool reimport = true; // deze waarde is een override als er wel een file op disk staat maar je wenst toch de gegevens opnieuw in te lezen van disk

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()  
        {
            if (!Directory.Exists(baseDir)) {
                Directory.CreateDirectory(baseDir);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
