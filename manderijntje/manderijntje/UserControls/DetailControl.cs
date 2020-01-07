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
        private string _aantalOverstappen;
        private string _perron;
        private List<tussenStops> _tussenstop;

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
        public string aantalOverstappen
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


        public DetailsControl()
        {
            InitializeComponent();
        }

        public void setupView()
        {
            tijdenLBL.Text = _beginTijd + " - " + _eindTijd;
            totaleTijdLBL.Text = _totaleTijd;
            aantalOverstappenLBL.Text = _aantalOverstappen + "x";
            PerronLBL.Text = perron;

            tussenstopCell[] listItems = new tussenstopCell[tussenstop.Count()]; ;
            for (int i = 0; i < tussenstop.Count; i++)
            {
                listItems[i] = new tussenstopCell();
                listItems[i].vertrekTijd = tussenstop[i].vertrekTijd;
                listItems[i].stationNaam = tussenstop[i].station;
                listItems[i].perron = tussenstop[i].perron;
                listItems[i].richting = tussenstop[i].richtingVervoer;
                listItems[i].typeVervoer = tussenstop[i].typeVervoer;

                if (tussenstopsPanel.Controls.Count < 0)
                    tussenstopsPanel.Controls.Clear();
                else
                    tussenstopsPanel.Controls.Add(listItems[i]);
            }
        }
    }
}
