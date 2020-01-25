using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class AutoSuggestion : UserControl
    {
        public List<AutoSuggestionModel> stationList = new List<AutoSuggestionModel>();
        public List<Node> nodeList = new List<Node>();
        public List<AutoSuggestionModel> suggestionsList = new List<AutoSuggestionModel>();
        Form1 _parent;

        public AutoSuggestion(Form1 parent)
        {
            InitializeComponent();
            this._parent = parent;  
        }

        // fill the stationList with correct stations
        public void SetList(List<Node> nodes)
        {
            nodeList = nodes;
            foreach (Node node in nodeList)
                stationList.Add(new AutoSuggestionModel(node.stationname, "Train"));
        }
        public AutoSuggestion()
        {
            InitializeComponent();
        }
        // Clears autosuggtions of the view and makes autosuggestionList empty
        public void ClearAutosuggest()
        {
            _parent.AutosuggesInVisible();
            suggestionsList.Clear();
            autoSuggestFlowControl.Controls.Clear();
        }

        // Fill the stationName of the autosuggestions to the right TextBox
        public void FillTextbox(string stationNaam, bool bInput)
        {
            if (bInput)
                _parent.departureInput.Text = stationNaam;
            else
                _parent.destinationInput.Text = stationNaam;

            ClearAutosuggest();
        }

        // Looks if there is a station that begin with the string parameter, if so it will put it in a suggestion list.
        public void CheckInput(string toCheck)
        {
            suggestionsList.Clear();

            // This reduces lagg of the program when typing a station name
            if (toCheck.Length < 3)
                return;

            foreach (AutoSuggestionModel station in stationList)
            {
                if (station.stationName.ToLower().StartsWith(toCheck.ToLower()))
                    suggestionsList.Add(station);
            }
        }

        // If there is no autosuggestion result, show the last know autosuggestion
        public void ShowBackupList(List<AutoSuggestionModel> list)
        {
            suggestionsList = list;
        }

        // Calls the needed methods to display the suggestion Usercontrol
        public void SetupSuggestions(object sender)
        {
            bool bInput = false;
            if (sender.Equals(_parent.departureInput))
            {
                bInput = true;
                _parent.SetLocationAutosuggestion(_parent.departureInput.Location.Y + _parent.departureInput.Height + _parent.textboxPanel.Location.Y + _parent.inputPanel.Location.Y, suggestionsList.Count);
            }
            else
                _parent.SetLocationAutosuggestion(_parent.destinationInput.Location.Y + _parent.destinationInput.Height + _parent.textboxPanel.Location.Y + _parent.inputPanel.Location.Y, suggestionsList.Count);

            _parent.ClearFlowControl(autoSuggestFlowControl);
            _parent.FillAutosuggestion(new AutoSuggestionCell[suggestionsList.Count()], bInput, suggestionsList);
        }
    }
}
