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
    public partial class transferCell : UserControl
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
            set { _vertrekTijd = value; departuretimeLBL.Text = value; }
        }
        public string stationNaam
        {
            get { return _stationNaam; }
            set { _stationNaam = value; stationLBL.Text = value; }
        }
        public string perron
        {
            get { return _perron; }
            set { _perron = value; platformLBL.Text = "Platform " + value; }
        }
        public string richting
        {
            get { return _richting; }
            set { _richting = value; directionLBL.Text = "To " + value; }
        }

        public string typeVervoer
        {
            get { return _typeVervoer; }
            set { _typeVervoer = value;
                
                if(_typeVervoer == "train")
                {
                    typetransportIcon.Image = Properties.Resources.OrangeTrain;
                }
                else if (_typeVervoer == "bus")
                {
                    typetransportIcon.Image = Properties.Resources.busIcon;
                }
            }
        }

        public bool last
        {
            get { return _last; }
            set { _last = value;
                // Checks wich image it need to have
                if (_last)
                {
                    lineImage.Image = Properties.Resources.eindSpoor;
                    typetransportIcon.Visible = false;
                }    
            }
        }
        public bool first
        {
            get { return _first; }
            set { _first = value;
                // Checks wich image it need to have
                if (_first)
                {
                    lineImage.Image = Properties.Resources.beginSpoor;
                }
            }
        }

        public bool mid
        {
            get { return _mid; }
            set { _mid = value;
                // Checks wich image it need to have
                if (_mid)
                {
                    lineImage.Image = Properties.Resources.middenSpoor;
                }
            }
        }

        public transferCell()
        {
            InitializeComponent();
        }

    }
}
