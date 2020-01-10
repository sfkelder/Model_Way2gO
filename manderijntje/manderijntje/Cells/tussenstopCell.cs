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
    public partial class tussenstopCell : UserControl
    {
        private string _vertrekTijd;
        private string _stationNaam;
        private string _perron;
        private string _richting;
        private string _typeVervoer;
        private bool _last;
        private bool _first;
        private bool _mid;

        public string vertrekTijd
        {
            get { return _vertrekTijd; }
            set { _vertrekTijd = value; vertrekTijdLBL.Text = value; }
        }
        public string stationNaam
        {
            get { return _stationNaam; }
            set { _stationNaam = value; stationLBL.Text = value; }
        }
        public string perron
        {
            get { return _perron; }
            set { _perron = value; perronLBL.Text = "Spoor " + value; }
        }
        public string richting
        {
            get { return _richting; }
            set { _richting = value; richtingLBL.Text = "Richting " + value; }
        }

        public string typeVervoer
        {
            get { return _typeVervoer; }
            set { _typeVervoer = value;
                
                if(_typeVervoer == "Trein")
                {
                    typeVervoerIcon.Image = Properties.Resources.OrangeTrain;
                }
                else if (_typeVervoer == "Bus")
                {
                    typeVervoerIcon.Image = Properties.Resources.busIcon;
                }
            }
        }

        public bool last
        {
            get { return _last; }
            set { _last = value;
                // Kijkt welke Image die moet gebruiken.
                if (_last)
                {
                    lijnImage.Image = Properties.Resources.eindSpoor;
                    typeVervoerIcon.Visible = false;
                }    
            }
        }
        public bool first
        {
            get { return _first; }
            set { _first = value; 
                // Kijkt welke Image die moet gebruiken.
                if (_first)
                {
                    lijnImage.Image = Properties.Resources.beginSpoor;
                }
            }
        }

        public bool mid
        {
            get { return _mid; }
            set { _mid = value;
                // Kijkt welke Image die moet gebruiken.
                if (_mid)
                {
                    lijnImage.Image = Properties.Resources.middenSpoor;
                }
            }
        }

        public tussenstopCell()
        {
            InitializeComponent();
        }

    }
}
