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
    public partial class Cell : UserControl
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
        private List<tussenStops> _tussenstop = new List<tussenStops>();
        Form1 _parent;
        public string beginTijd 
        {
            get { return _beginTijd; }
            set { _beginTijd = value; }
        }
        public string eindTijd
        {
            get { return _eindTijd; }
            set { _eindTijd = value; eindTijdLBL.Text = _beginTijd + " - " + value; }
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
            set { _naamVervoer = value; VervoerderLBL.Text = vervoerder + " - " + value; }
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
                clockIcon.Image = manderijntje.Properties.Resources.OrangeClock;
                totaleTijdLBL.Text = value; }
        }
        public string aantalOverstappen
        {
            get { return _aantalOverstappen; }
            set { _aantalOverstappen = value;
                this.overstappenIcon.Image = manderijntje.Properties.Resources.OverstappenOrange;
                aantalOverstappenLBL.Text = value + "x"; }
        }
        public string perron
        {
            get { return _perron; }
            set { _perron = value; PerronLBL.Text = value; }
        }
        public List<tussenStops> tussenstop {
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


        //
        // Zorgt ervoor dat de cell weet wie de parent is en dus waar die de methodes moet aanroepen wanneer er op de label wordt geklikt.
        //
        public Cell(Form1 parent)
        {
            InitializeComponent();
            this._parent = parent;
        }

        //
        // Geeft de variabelen een waarde die wordt meegegeven als parameters.
        //
        public Cell(string beginTijd, string eindTijd, string vervoerder, string typeVervoer, string naamVervoer,
            string busLijn, string totaleTijd, string aantalOverstappen, string perron, List<tussenStops> tussenstop, bool orange)
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
        }

        //
        // Pakt alle variabelen in en geeft een nieuwe Cell terug. Dit hebben we nodig voor de cell in de Form.
        //
        public static Cell getCellDetails(string beginTijd, string eindTijd, string vervoerder, string typeVervoer, string naamVervoer,
            string busLijn, string totaleTijd, string aantalOverstappen, string perron, List<tussenStops> tussenstop, bool orange)
        {
            Cell c = new Cell(beginTijd, eindTijd, vervoerder, typeVervoer, naamVervoer, busLijn, totaleTijd, aantalOverstappen, perron, tussenstop, orange);
            return c;
        }

        //
        // Triggerd methodes (bij de From class) wanneer er op een label wordt geklikt.
        //
        private void eindTijdLBL_Click(object sender, EventArgs e)
        {
            orange = true;
            this._parent.cell = getCellDetails(_beginTijd, _eindTijd, _vervoerder, _typeVervoer, _naamVervoer, _busLijn, _totaleTijd, _aantalOverstappen, _perron, _tussenstop, _orange);
            this._parent.setupReisDetails();
        }

        private void changeColor(bool Orange)
        {
            if (Orange)
            {
                this.BackColor = Color.FromArgb(255,122,0);
                this.eindTijdLBL.ForeColor = Color.White;
                this.VervoerderLBL.ForeColor = Color.White;
                this.aantalOverstappenLBL.ForeColor = Color.White;
                this.totaleTijdLBL.ForeColor = Color.White;
                this.PerronLBL.ForeColor = Color.White;
                this.overstappenIcon.Image = manderijntje.Properties.Resources.OverstappenWhite;
                this.overstappenIcon.Image = manderijntje.Properties.Resources.WhiteClock;
            }
            else
            {
                this.BackColor = Color.White;
                this.eindTijdLBL.ForeColor = Color.Black;
                this.VervoerderLBL.ForeColor = Color.DimGray;
                this.aantalOverstappenLBL.ForeColor = Color.FromArgb(255, 122, 0);
                this.totaleTijdLBL.ForeColor = Color.FromArgb(255, 122, 0);
                this.PerronLBL.ForeColor = Color.FromArgb(255, 122, 0);
                this.overstappenIcon.Image = manderijntje.Properties.Resources.OverstappenOrange;
                this.clockIcon.Image = manderijntje.Properties.Resources.OrangeClock;
            } 
        }
    }
}
