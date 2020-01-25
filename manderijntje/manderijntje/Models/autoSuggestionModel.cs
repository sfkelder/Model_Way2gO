namespace Manderijntje
{
    public class AutoSuggestionModel
    {
        public string stationName { get; set; }
        public string stationType { get; set; }

        public AutoSuggestionModel(string stationName, string stationType)
        {
            this.stationName = stationName;
            this.stationType = stationType;
        }
    }
}
