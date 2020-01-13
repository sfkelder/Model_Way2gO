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
        private string _departureTime;
        private string _stationName;
        private string _platform;
        private string _toStation;
        private string _typeTransport;
        private bool _last;
        private bool _first;
        private bool _mid;

        public string departureTime
        {
            get { return _departureTime; }
            set { _departureTime = value; departuretimeLBL.Text = value; }
        }
        public string stationName
        {
            get { return _stationName; }
            set { _stationName = value; stationLBL.Text = value; }
        }
        public string platform
        {
            get { return _platform; }
            set { _platform = value; platformLBL.Text = "Platform " + value; }
        }
        public string toStation
        {
            get { return _toStation; }
            set { _toStation = value; directionLBL.Text = "To " + value; }
        }

        public string typeTransport
        {
            get { return _typeTransport; }
            set { _typeTransport = value;
                
                if(_typeTransport == "train")
                {
                    typetransportIcon.Image = Properties.Resources.OrangeTrain;
                }
                else if (_typeTransport == "bus")
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
