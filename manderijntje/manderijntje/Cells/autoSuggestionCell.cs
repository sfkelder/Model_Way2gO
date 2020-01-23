using System;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class autoSuggestionCell : UserControl
    {
        private string _stationName;
        private string _stationType;
        private bool _departureInput;
        Form1 _form;

        public string stationName
        {
            get { return _stationName; }
            set { _stationName = value; stationLBL.Text = value; }
        }
        public string stationType
        {
            get { return _stationType; }
            set { _stationType = value; 
                if (_stationType == "Trein")
                {
                    stationTypeIcon.Image = Properties.Resources.OrangeTrain;
                }
                else if (_stationType == "Bus")
                {
                    stationTypeIcon.Image = Properties.Resources.busIcon;
                } 
            }
        }
        public bool departureInput
        {
            get { return _departureInput; }
            set { _departureInput = value; }
        }

        public autoSuggestionCell(Form1 form)
        {
            InitializeComponent();
            this._form = form;
        }

        // Calls methods in the form call when clicked on a label.
        private void stationLBL_Click(object sender, EventArgs e)
        {
            autoSuggestion a = new autoSuggestion(_form);
            a.fillTextbox(_stationName, _departureInput);
        }

        private void stationTypeIcon_Click(object sender, EventArgs e)
        {
            autoSuggestion a = new autoSuggestion(_form);
            a.fillTextbox(_stationName, _departureInput);
        }
    }
}
