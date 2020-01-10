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
    public partial class autoSuggestie : UserControl
    {
        public List<autoSuggestModel> stationList = new List<autoSuggestModel>();
        public List<autoSuggestModel> suggestionsList = new List<autoSuggestModel>();
        Form1 _parent;

        public autoSuggestie(Form1 parent)
        {
            InitializeComponent();
            stationList = stationSuggestions();
            this._parent = parent;
        }

        public autoSuggestie()
        {
            InitializeComponent();
        }

        //
        // Haalt de autosuggestie weg van het scherm en maakt de list van autosuggesties leeg.
        //
        public void clearAutosuggest()
        {
            _parent.autosuggesInVisible();
            suggestionsList.Clear();
            autosuggestFlowControl.Controls.Clear();
        }

        //
        // Zorgt ervoor dat de geklikte autosuggestie in de juiste TextBox als text komt te staan.
        //
        public void fillTextbox(string stationNaam, bool bInput)
        {
            if (bInput)
                _parent.beginInput.Text = stationNaam;
            else
                _parent.eindInput.Text = stationNaam;

            clearAutosuggest();
        }

        //
        // Kijk of er een station, bushalte ect. begint met de meegegeven parameter, zo ja, dan wordt het in een suggestie list gestopt.
        //
        public void checkInput(string toCheck)
        {
            suggestionsList.Clear();
            foreach (autoSuggestModel item in stationList)
            {
                if (item.stationName.ToLower().StartsWith(toCheck.ToLower()))
                {
                    suggestionsList.Add(item);
                }
            }
        }

        //
        //  Zorgt ervoor dat de juiste methodes worden aangeroepen waardoor de suggestie userControl juist wordt weergegeven.
        //
        public void setupSuggesties(object sender)
        {
            bool bInput = false;
            if (sender.Equals(_parent.beginInput))
            {
                bInput = true;
                _parent.setFlowControl(_parent.beginInput.Location.Y + _parent.beginInput.Height + _parent.textboxPanel.Location.Y + _parent.inputPanel.Location.Y, suggestionsList.Count);
            }
            else
                _parent.setFlowControl(_parent.eindInput.Location.Y + _parent.eindInput.Height + _parent.textboxPanel.Location.Y + _parent.inputPanel.Location.Y, suggestionsList.Count);

            _parent.clearFlowControl(autosuggestFlowControl);
            _parent.fillAutosuggestie(new autoSuggesCell[suggestionsList.Count()], bInput, suggestionsList);
        }


        //
        // FAKE DATA
        //
        private List<autoSuggestModel> stationSuggestions()
        {
            List<autoSuggestModel> l = new List<autoSuggestModel>();
            for (int i = 0; i < 20; i++)
            {
                l.Add(new autoSuggestModel("twst", "Trein"));
            }
            l.Add(new autoSuggestModel("Nijmegen", "Trein"));
            l.Add(new autoSuggestModel("Utrecht Centraal", "Trein"));
            l.Add(new autoSuggestModel("Nijmegen centraal", "Trein"));
            l.Add(new autoSuggestModel("Arnhem Centraal", "Trein"));
            l.Add(new autoSuggestModel("Arnhem", "Bus"));
            l.Add(new autoSuggestModel("Arnhem", "Trein"));
            l.Add(new autoSuggestModel("Arnhem", "Trein"));
            l.Add(new autoSuggestModel("Arnhem", "Bus"));
            l.Add(new autoSuggestModel("Arnhem", "Bus"));
            l.Add(new autoSuggestModel("Arnhem", "Trein"));
            l.Add(new autoSuggestModel("Arnhem Centraal", "Bus"));
            l.Add(new autoSuggestModel("Den Haag Centraal", "Trein"));
            return l;
        }

    }
}
