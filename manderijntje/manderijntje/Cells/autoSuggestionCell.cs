using System;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class AutoSuggestionCell : UserControl
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
            set {
                _stationType = value;
                stationTypeIcon.Image = Properties.Resources.OrangeTrain;
            }
        }
        public bool departureInput
        {
            get { return _departureInput; }
            set { _departureInput = value; }
        }

        public AutoSuggestionCell(Form1 form)
        {
            InitializeComponent();
            this._form = form;
        }

        // Calls methods in the form call when clicked on a label.
        private void StationLBLClick(object sender, EventArgs e)
        {
            AutoSuggestion a = new AutoSuggestion(_form);
            a.FillTextbox(_stationName, _departureInput);
        }

        private void StationTypeIconClick(object sender, EventArgs e)
        {
            AutoSuggestion a = new AutoSuggestion(_form);
            a.FillTextbox(_stationName, _departureInput);
        }
    }
}
