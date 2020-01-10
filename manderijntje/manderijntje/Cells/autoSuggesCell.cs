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
    public partial class autoSuggesCell : UserControl
    {
        private string _stationName;
        private string _stationType;
        private bool _departureInput;
        autoSuggestie _auto;
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
        //
        // Zorgt ervoor dat de cell weet wie de parent is en dus waar die de methodes moet aanroepen wanneer er op de label wordt geklikt.
        //
        public autoSuggesCell(autoSuggestie auto, Form1 form)
        {
            InitializeComponent();
            this._auto = auto;
            this._form = form;
        }

        //
        // Geeft de variabelen een waarde die wordt meegegeven als parameters.
        //
        public autoSuggesCell(string stationName, string stationType, bool departureInput)
        {
            this._stationName = stationName;
            this._stationType = stationType;
            this._departureInput = departureInput;
        }

        //
        // Pakt alle variabelen in en geeft een nieuwe autoSuggesCell terug. Dit hebben we nodig voor de autosuggescell methode in de Form.
        //
        public static autoSuggesCell getautoSuggestDetails(string stationName, string stationType, bool departureInput)
        {
            autoSuggesCell auto = new autoSuggesCell(stationName, stationType, departureInput);
            return auto;
        }

        //
        // Triggerd methodes (bij de From class) wanneer er op een label wordt geklikt.
        //
        private void stationLBL_Click(object sender, EventArgs e)
        {
            autoSuggestie a = new autoSuggestie(_form);
            a.fillTextbox(_stationName, _departureInput);
        }
    }
}
