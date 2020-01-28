using System.Windows.Forms;

namespace Manderijntje
{
    public partial class TransferCell : UserControl
    {
        private string _departureTime;
        private string _stationName;
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

        public string typeTransport
        {
            get { return _typeTransport; }
            set { _typeTransport = value;
                typetransportIcon.Image = manderijntje.Properties.Resources.OrangeTrain;
            }
        }

        public bool last
        {
            get { return _last; }
            set { _last = value;
                // Checks wich image it need to have.
                if (_last)
                {
                    lineImage.Image = manderijntje.Properties.Resources.endTrack;
                    typetransportIcon.Visible = false;
                }    
            }
        }
        public bool first
        {
            get { return _first; }
            set { _first = value;
                // Checks wich image it need to have.
                if (_first)
                {
                    lineImage.Image = manderijntje.Properties.Resources.startTrack;
                }
            }
        }

        public bool mid
        {
            get { return _mid; }
            set { _mid = value;
                // Checks wich image it need to have.
                if (_mid)
                {
                    lineImage.Image = manderijntje.Properties.Resources.midTrack;
                }
            }
        }

        public TransferCell()
        {
            InitializeComponent();
        }

    }
}
