using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class Vertrek
    {
        public string departureLocation { get; set; }
        public string destinationLocation { get; set; }
        public string departureTijd { get; set; }

        public Vertrek(string departureLocation, string destinationLocation, string departureTijd)
        {
            this.departureLocation = departureLocation;
            this.destinationLocation = destinationLocation;
            this.departureTijd = departureTijd;
        }
        public Vertrek()
        {
        }

        // Gives vertrekModel model back
        public Vertrek vertrekModel(string departureLocation, string destinationLocation, string departureTijd)
        {
            return new Vertrek(departureLocation, destinationLocation, departureTijd);
        }

    }
}
