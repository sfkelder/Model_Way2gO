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
        public string perron { get; set; }
        public string aankomstTijd { get; set; }
        public string vertrekTijd { get; set; }
        public string typeVervoer { get; set; }
        public string richtingVervoer { get; set; }

        public transferModel(string station, string perron, string aankomstTijd, string vertrekTijd, string typeVervoer, string richtingVervoer)
        {
            this.station = station;
            this.perron = perron;
            this.aankomstTijd = aankomstTijd;
            this.vertrekTijd = vertrekTijd;
            this.typeVervoer = typeVervoer;
            this.richtingVervoer = richtingVervoer;
        }

        public transferModel()
        { 
        }

        // Gives tussenStops model back
        public transferModel tussenstopsModel(string station, string perron, string aankomstTijd, string vertrekTijd, string typeVervoer, string richtingVervoer)
        {
            return new transferModel(station, perron, aankomstTijd, vertrekTijd, typeVervoer, richtingVervoer);
        }
    }
}
