using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manderijntje
{
    public partial class tripOptionsCell : UserControl
    {
        private string _departureTime;
        private string _destinationTime;
        private string _nameTransport;
        private string _totalTime;
        private string _transfersCount;
        private List<Node> _shortestPath = new List<Node>();
        Form1 _parent;

        public string departureTime 
        {
            get { return _departureTime; }
            set { _departureTime = value; }
        }

        public string destinationTime
        {
            get { return _destinationTime; }
            set { _destinationTime = value; TimeLBL.Text = _departureTime + " - " + value; }
        }

        public string nameTransport
        {
            get { return _nameTransport; }
            set { _nameTransport = value; nameTransportLBL.Text = value; }
        }

        public string totalTime
        {
            get { return _totalTime; }
            set { _totalTime = value; 
                clockIcon.Image = Properties.Resources.OrangeClock;
                totaltimeLBL.Text = value; }
        }

        public List<Node> shortestPath
        {
            get { return _shortestPath; }
            set { _shortestPath = value; }
        }

        public tripOptionsCell(Form1 parent)
        {
            InitializeComponent();
            this._parent = parent;
        }

        public tripOptionsCell(string departureTime, string destinationTime, string nameTransport,
           string totalTime, string transferCount, List<Node> shortestPath)
        {
            this._departureTime = departureTime;
            this._destinationTime = destinationTime;
            this._nameTransport = nameTransport;
            this._totalTime = totalTime;
            this._transfersCount = transferCount;
            this._shortestPath = shortestPath;
        }

        // Gives a tripOptionsCell back
        public static tripOptionsCell getCellDetails(string departureTime, string destinationTime, string nameTransport
            , string totalTime, string transferCount, List<Node> shortestPath)
        {
            tripOptionsCell c = new tripOptionsCell(departureTime, destinationTime, nameTransport, totalTime, transferCount, shortestPath);
            return c;
        }

        // Calls methods in the form call when clicked on a label.

        private void click()
        {
            this._parent.tripOptionscell = getCellDetails(_departureTime, _destinationTime, _nameTransport, _totalTime, _transfersCount, _shortestPath);
            this._parent.setupTripDetails();
        }

        // Label Clicked call "click" method
        private void eindTijdLBL_Click(object sender, EventArgs e)
        {
            click();
        }

        private void carrierLBL_Click(object sender, EventArgs e)
        {
            click();
        }

        private void totaltimeLBL_Click(object sender, EventArgs e)
        {
            click();
        }

        private void transferLBL_Click(object sender, EventArgs e)
        {
            click();
        }

        private void platformLBL_Click(object sender, EventArgs e)
        {
            click();
        }
    }
}
