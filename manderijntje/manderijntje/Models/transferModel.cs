using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class transferModel
    {
        public string station { get; set; }
        public string platform { get; set; }
        public string arrivalTime { get; set; }
        public string departureTime { get; set; }
        public string typeTranssport { get; set; }
        public string toStation { get; set; }

        public transferModel(string station, string platform, string arrivalTime, string departureTime, string typeTranssport, string toStation)
        {
            this.station = station;
            this.platform = platform;
            this.arrivalTime = arrivalTime;
            this.departureTime = departureTime;
            this.typeTranssport = typeTranssport;
            this.toStation = toStation;
        }
    }
}
