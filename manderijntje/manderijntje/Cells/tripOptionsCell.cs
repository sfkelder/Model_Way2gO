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
        private string _beginTijd;
        private string _eindTijd;
        private string _vervoerder;
        private string _typeVervoer;
        private string _naamVervoer;
        private string _busLijn;
        private string _totaleTijd;
        private string _aantalOverstappen;
        private string _perron;
        private bool _orange;
        private List<transferModel> _tussenstop = new List<transferModel>();
        private List<Node> _shortestPath = new List<Node>();
        Form1 _parent;
        public string beginTijd 
        {
            get { return _beginTijd; }
            set { _beginTijd = value; }
        }
        public string eindTijd
        {
            get { return _eindTijd; }
            set { _eindTijd = value; TimeLBL.Text = _beginTijd + " - " + value; }
        }
        public string vervoerder
        {
            get { return _vervoerder; }
            set { _vervoerder = value; }
        }
        public string typeVervoer
        {
            get { return _typeVervoer; }
            set { _typeVervoer = value; }
        }
        public string naamVervoer
        {
            get { return _naamVervoer; }
            set { _naamVervoer = value; carrierLBL.Text = vervoerder + " - " + value; }
        }
        public string busLijn
        {
            get { return _busLijn; }
            set { _busLijn = value; }
        }
        public string totaleTijd
        {
            get { return _totaleTijd; }
            set { _totaleTijd = value; 
                clockIcon.Image = Properties.Resources.OrangeClock;
                totaltimeLBL.Text = value; }
        }
        public string aantalOverstappen
        {
            get { return _aantalOverstappen; }
            set { _aantalOverstappen = value;
                this.transferIcon.Image = Properties.Resources.OverstappenOrange;
                transferLBL.Text = value + "x";
                Console.WriteLine(value);
            }
        }
        public string perron
        {
            get { return _perron; }
            set { _perron = value; platformLBL.Text = value; }
        }
        public List<transferModel> tussenstop {
            get { return _tussenstop; }
            set { _tussenstop = value; }
        }

        public bool orange
        { 
            get { return _orange; }
            set { _orange = value;
                //changeColor(_orange);
            }
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
        public tripOptionsCell(string beginTijd, string eindTijd, string vervoerder, string typeVervoer, string naamVervoer,
            string busLijn, string totaleTijd, string aantalOverstappen, string perron, List<transferModel> tussenstop, bool orange, List<Node> shortestPath)
        {
            this._beginTijd = beginTijd;
            this._eindTijd = eindTijd;
            this._vervoerder = vervoerder;
            this._typeVervoer = typeVervoer;
            this._naamVervoer = naamVervoer;
            this._busLijn = busLijn;
            this._totaleTijd = totaleTijd;
            this._aantalOverstappen = aantalOverstappen;
            this._perron = perron;
            this._tussenstop = tussenstop;
            this._orange = orange;
            this._shortestPath = shortestPath;
        }

        //
        // Gives a tripOptionsCell back
        //
        public static tripOptionsCell getCellDetails(string beginTijd, string eindTijd, string vervoerder, string typeVervoer, string naamVervoer,
            string busLijn, string totaleTijd, string aantalOverstappen, string perron, List<transferModel> tussenstop, bool orange, List<Node> shortestPath)
        {
            tripOptionsCell c = new tripOptionsCell(beginTijd, eindTijd, vervoerder, typeVervoer, naamVervoer, busLijn, totaleTijd, aantalOverstappen, perron, tussenstop, orange, shortestPath);
            return c;
        }

        // Calls methods in the form call when clicked on a label.

        private void click()
        {
            orange = true;
            this._parent.tripOptionscell = getCellDetails(_beginTijd, _eindTijd, _vervoerder, _typeVervoer, _naamVervoer, _busLijn, _totaleTijd, _aantalOverstappen, _perron, _tussenstop, _orange, _shortestPath);
            this._parent.setupTripDetails();
        }

        private void changeColor(bool Orange)
        {
            if (Orange)
            {
                this.BackColor = Color.FromArgb(255,122,0);
                this.TimeLBL.ForeColor = Color.White;
                this.carrierLBL.ForeColor = Color.White;
                this.transferLBL.ForeColor = Color.White;
                this.totaltimeLBL.ForeColor = Color.White;
                this.platformLBL.ForeColor = Color.White;
                this.transferIcon.Image = Properties.Resources.OverstappenWhite;
                this.transferIcon.Image = Properties.Resources.WhiteClock;
            }
            else
            {
                this.BackColor = Color.White;
                this.TimeLBL.ForeColor = Color.Black;
                this.carrierLBL.ForeColor = Color.DimGray;
                this.transferLBL.ForeColor = Color.FromArgb(255, 122, 0);
                this.totaltimeLBL.ForeColor = Color.FromArgb(255, 122, 0);
                this.platformLBL.ForeColor = Color.FromArgb(255, 122, 0);
                this.transferIcon.Image = manderijntje.Properties.Resources.OverstappenOrange;
                this.clockIcon.Image = manderijntje.Properties.Resources.OrangeClock;
            } 
        }

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
