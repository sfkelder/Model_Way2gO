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
        private bool _laatste;
        private bool _eerste;
        private bool _midden;

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
                    typeVervoerIcon.Image = manderijntje.Properties.Resources.OrangeTrain;
                }
                else if (_typeVervoer == "Bus")
                {
                    typeVervoerIcon.Image = manderijntje.Properties.Resources.busIcon;
                }
            }
        }

        public bool laatste
        {
            get { return _laatste; }
            set { _laatste = value;
                // Kijkt welke Image die moet gebruiken.
                if (_laatste)
                {
                    lijnImage.Image = manderijntje.Properties.Resources.eindSpoor;
                    typeVervoerIcon.Visible = false;
                }    
            }
        }
        public bool eerste
        {
            get { return _eerste; }
            set { _eerste = value; 
                // Kijkt welke Image die moet gebruiken.
                if (_eerste)
                {
                    lijnImage.Image = manderijntje.Properties.Resources.beginSpoor;
                }
            }
        }

        public bool midden
        {
            get { return _midden; }
            set { _midden = value;
                // Kijkt welke Image die moet gebruiken.
                if (_midden)
                {
                    lijnImage.Image = manderijntje.Properties.Resources.middenSpoor;
                }
            }
        }

        public tussenstopCell()
        {
            InitializeComponent();
        }

    }
}
