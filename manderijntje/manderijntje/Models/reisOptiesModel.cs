using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class reisOpties
    {
        public string beginTijd { get; set; }
        public string eindTijd { get; set; }
        public string vervoerder { get; set; }
        public string typeVervoer { get; set; }
        public string naamVervoer { get; set; }
        public string busLijn { get; set; }
        public string totaleTijd { get; set; }
        public int aantalOverstappen { get; set; }
        public string perron { get; set; }
        public bool orange { get; set; }
        public List<tussenStops> tussenstop { get; set; }
 

        public reisOpties(string beginTijd, string eindTijd, string vervoerder, string typeVervoer, string naamVervoer,
            string busLijn, string totaleTijd, int aantalOverstappen, string perron, List<tussenStops> tussenstop, bool orange)
        {
            this.beginTijd = beginTijd;
            this.eindTijd = eindTijd;
            this.vervoerder = vervoerder;
            this.typeVervoer = typeVervoer;
            this.naamVervoer = naamVervoer;
            this.busLijn = busLijn;
            this.totaleTijd = totaleTijd;
            this.aantalOverstappen = aantalOverstappen;
            this.perron = perron;
            this.tussenstop = tussenstop;
            this.orange = orange;
        }

        public reisOpties()
        {
        }

        //
        // Geeft een reisOptie model terug.
        //
        public reisOpties reisOptiesModel(string beginTijd, string eindTijd, string vervoerder, string typeVervoer, string naamVervoer,
            string busLijn, string totaleTijd, int aantalOverstappen, string perron, List<tussenStops> tussenstop, bool orange)
        {
            return new reisOpties(beginTijd, eindTijd, vervoerder, typeVervoer, naamVervoer, busLijn, totaleTijd, aantalOverstappen, perron, tussenstop, orange);
        }
    }
}
