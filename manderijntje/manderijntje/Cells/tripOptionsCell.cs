using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class TripOptionsCell : UserControl
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
                clockIcon.Image = manderijntje.Properties.Resources.OrangeClock;
                totaltimeLBL.Text = value; }
        }

        public List<Node> shortestPath
        {
            get { return _shortestPath; }
            set { _shortestPath = value; }
        }

        public TripOptionsCell(Form1 parent)
        {
            InitializeComponent();
            this._parent = parent;
        }

        public TripOptionsCell(string departureTime, string destinationTime, string nameTransport,
           string totalTime, string transferCount, List<Node> shortestPath)
        {
            this._departureTime = departureTime;
            this._destinationTime = destinationTime;
            this._nameTransport = nameTransport;
            this._totalTime = totalTime;
            this._transfersCount = transferCount;
            this._shortestPath = shortestPath;
        }

        // Gives a tripOptionsCell back.
        public static TripOptionsCell GetCellDetails(string departureTime, string destinationTime, string nameTransport
            , string totalTime, string transferCount, List<Node> shortestPath)
        {
            TripOptionsCell c = new TripOptionsCell(departureTime, destinationTime, nameTransport, totalTime, transferCount, shortestPath);
            return c;
        }

        // Calls methods in the form call when clicked on a label.

        private new void Click()
        {
            this._parent.tripOptionscell = GetCellDetails(_departureTime, _destinationTime, _nameTransport, _totalTime, _transfersCount, _shortestPath);
            this._parent.SetupTripDetails();
        }

        // Label Clicked call "click" method.
        private void EindTijdLBLClick(object sender, EventArgs e)
        {
            Click();
        }

        private void CarrierLBLClick(object sender, EventArgs e)
        {
            Click();
        }

        private void TotaltimeLBLClick(object sender, EventArgs e)
        {
            Click();
        }

        private void TransferLBLClick(object sender, EventArgs e)
        {
            Click();
        }

        private void PlatformLBLClick(object sender, EventArgs e)
        {
            Click();
        }
    }
}
