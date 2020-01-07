using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class autoSuggestModel
    {
        public string stationNaam { get; set; }
        public string stationType { get; set; }

        public autoSuggestModel(string stationNaam, string stationType)
        {
            this.stationNaam = stationNaam;
            this.stationType = stationType;
        }
    }
}
