using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace manderijntje
{
    public partial class DetailsControl : UserControl
    {
        private string _departureTime;
        private string _destinationTime;
        private string _totalTime;
        private string _transfers;
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
            }
        }

        public string transfers
        {
            get { return _transfers; }
            set { _transfers = value; transfersLBL.Text = value + "x"; }
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
                        sw.WriteLine("Total Time: " + _totalTime);
                        sw.WriteLine("Transfer Count: " + _transfers);
                        sw.WriteLine("Transfers: " + _transfers + "\n");
                        sw.WriteLine("Transfers Stations: \n");

                        foreach (Node node in shortestPath)
                        {
                            sw.WriteLine("Station: " + node.stationnaam);
                            sw.WriteLine("Departure Time: ");
                            sw.WriteLine("Platform: ");
                            sw.WriteLine("To Station: " + "" + "\n");
                        }
                    }
                    myStream.Close();
                }
            }
        }

        private void saveRoute_Click(object sender, EventArgs e)
        {
            saveAsTextFile();
        }
    }
}
