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
    public partial class autoSuggestionCell : UserControl
    {
        private string _stationName;
        private string _stationType;
        private bool _departureInput;
        autoSuggestion _auto;
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
        public autoSuggestionCell(autoSuggestion auto, Form1 form)
        {
            InitializeComponent();
            this._auto = auto;
            this._form = form;
        }
        public autoSuggestionCell(string stationName, string stationType, bool departureInput)
        {
            this._stationName = stationName;
            this._stationType = stationType;
            this._departureInput = departureInput;
        }

        // Gives a autoSuggesCell back
        public static autoSuggestionCell getautoSuggestDetails(string stationName, string stationType, bool departureInput)
        {
            autoSuggestionCell auto = new autoSuggestionCell(stationName, stationType, departureInput);
            return auto;
        }

        // Calls methods in the form call when clicked on a label.
        private void stationLBL_Click(object sender, EventArgs e)
        {
            autoSuggestion a = new autoSuggestion(_form);
            a.fillTextbox(_stationName, _departureInput);
        }
    }
}
