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
    public partial class DetailsControl : UserControl
    {
        private string _beginTijd;
        private string _eindTijd;
        private string _totaleTijd;
        private int _aantalOverstappen;
        private string _perron;
        private List<tussenStops> _tussenstop;
        private List<Node> _shortestPath;

        public string beginTijd
        {
            get { return _beginTijd; }
            set { _beginTijd = value; }
        }
        public string eindTijd
        {
            get { return _eindTijd; }
            set { _eindTijd = value; tijdenLBL.Text = _beginTijd + " - " + value; }
        }
        public string totaleTijd
        {
            get { return _totaleTijd; }
            set { _totaleTijd = value; }
        }
        public int aantalOverstappen
        {
            get { return _aantalOverstappen; }
            set { _aantalOverstappen = value; }
        }
        public string perron
        {
            get { return _perron; }
            set { _perron = value; }
        }

        public List<tussenStops> tussenstop
        {
            get { return _tussenstop; }
            set { _tussenstop = value; }
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
    }
}
