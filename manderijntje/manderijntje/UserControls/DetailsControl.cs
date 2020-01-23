using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace manderijntje
{
    public partial class DetailsControl : UserControl
    {
        private string _departureTime;
        private string _destinationTime;
        private string _totalTime;
        private List<Node> _shortestPath;
        public string departureTime
        {
            get { return _departureTime; }
            set { _departureTime = value; }
        }

        public string destinationTime
        {
            get { return _destinationTime; }
            set { _destinationTime = value; timesLBL.Text = _departureTime + " - " + value; }
        }

        public string totalTime
        {
            get { return _totalTime; }
            set { _totalTime = value; totaltimeLBL.Text = value;
                try
                {
                    if (value.Length == 7)
                    {
                        totaltimeLBL.Location = new Point(335, totaltimeLBL.Location.Y);
                        clockIcon.Location = new Point(totaltimeLBL.Location.X - 27, clockIcon.Location.Y);
                    }
                    else
                    {
                        totaltimeLBL.Location = new Point(310, totaltimeLBL.Location.Y);
                        clockIcon.Location = new Point(totaltimeLBL.Location.X - 27, clockIcon.Location.Y);
                    }
                }
                catch {}
                
            }
        }

        public List<Node> shortestPath
        {
            get { return _shortestPath; }
            set { _shortestPath = value; }
        }

        public DetailsControl()
        {
            InitializeComponent();
        }

        // Save route as text file
        private void saveAsTextFile()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    using (StreamWriter sw = File.CreateText(saveFileDialog.FileName + ".txt"))
                    {
                        sw.WriteLine("Depature Time: " + _departureTime);
                        sw.WriteLine("Arrival Time: " + _destinationTime);
                        sw.WriteLine("Total Time: " + _totalTime + "\n\n");
                        sw.WriteLine("Transfers:");
                        foreach (Node node in shortestPath)
                        {
                            sw.WriteLine("Station: " + node.stationname);
                            sw.WriteLine("Departure Time: " + _departureTime + "\n");
                        }
                    }
                    myStream.Close();
                }
            }
        }


        private void saveRoute_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();

            //saveAsTextFile();
        }

        // Make print document
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Way2Go Trip Details", new Font("Arial", 30, FontStyle.Bold), Brushes.Orange, 50, 50);
            e.Graphics.DrawString("Depature Time: " + _departureTime, new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 50, 120);
            e.Graphics.DrawString("Arrival Time: " + _destinationTime, new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 50, 170);
            e.Graphics.DrawString("Total Time: " + _totalTime, new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 50, 220);
            e.Graphics.DrawString("Transfers:", new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 50, 300);
            int j = 300;
            for(int i = 0; i < shortestPath.Count; i++)
            {
                j += 50;
                e.Graphics.DrawString("Station: " + shortestPath[i].stationname, new Font("Arial", 15, FontStyle.Regular), Brushes.Black, 50, j);
                j += 50;
                e.Graphics.DrawString("Departure Time: " + shortestPath[i].minCostToStart.ToShortTimeString(), new Font("Arial", 15, FontStyle.Regular), Brushes.Black, 50, j);
            }
        }
    }
}
