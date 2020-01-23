﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class autoSuggestion : UserControl
    {
        public List<autoSuggestionModel> stationList = new List<autoSuggestionModel>();
        public List<Node> nodeList = new List<Node>();
        public List<autoSuggestionModel> suggestionsList = new List<autoSuggestionModel>();
        Form1 _parent;

        public autoSuggestion(Form1 parent)
        {
            InitializeComponent();
            this._parent = parent;  
        }

        // fill the stationList with correct stations
        public void setList(List<Node> nodes)
        {
            nodeList = nodes;
            foreach (Node node in nodeList)
                stationList.Add(new autoSuggestionModel(node.stationname, "Train"));
        }
        public autoSuggestion()
        {
            InitializeComponent();
        }
        // Clears autosuggtions of the view and makes autosuggestionList empty
        public void clearAutosuggest()
        {
            _parent.autosuggesInVisible();
            suggestionsList.Clear();
            autoSuggestFlowControl.Controls.Clear();
        }

        // Fill the stationName of the autosuggestions to the right TextBox
        public void fillTextbox(string stationNaam, bool bInput)
        {
            if (bInput)
                _parent.departureInput.Text = stationNaam;
            else
                _parent.destinationInput.Text = stationNaam;

            clearAutosuggest();
        }

        // Looks if there is a station that begin with the string parameter, if so it will put it in a suggestion list.
        public void checkInput(string toCheck)
        {
            suggestionsList.Clear();

            // This reduces lagg of the program when typing a station name
            if (toCheck.Length < 3)
                return;

            foreach (autoSuggestionModel station in stationList)
            {
                if (station.stationName.ToLower().StartsWith(toCheck.ToLower()))
                    suggestionsList.Add(station);
            }
        }

        // If there is no autosuggestion result, show the last know autosuggestion
        public void showBackupList(List<autoSuggestionModel> list)
        {
            suggestionsList = list;
        }

        // Calls the needed methods to display the suggestion Usercontrol
        public void setupSuggesties(object sender)
        {
            bool bInput = false;
            if (sender.Equals(_parent.departureInput))
            {
                bInput = true;
                _parent.setLocationAutosuggestion(_parent.departureInput.Location.Y + _parent.departureInput.Height + _parent.textboxPanel.Location.Y + _parent.inputPanel.Location.Y, suggestionsList.Count);
            }
            else
                _parent.setLocationAutosuggestion(_parent.destinationInput.Location.Y + _parent.destinationInput.Height + _parent.textboxPanel.Location.Y + _parent.inputPanel.Location.Y, suggestionsList.Count);

            _parent.clearFlowControl(autoSuggestFlowControl);
            _parent.fillAutosuggestie(new autoSuggestionCell[suggestionsList.Count()], bInput, suggestionsList);
        }
    }
}
