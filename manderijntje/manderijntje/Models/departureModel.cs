using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class departureModel
    {
        public string departureLocation { get; set; }
        public string destinationLocation { get; set; }
        public string departureTijd { get; set; }

        public departureModel(string departureLocation, string destinationLocation, string departureTijd)
        {
            this.departureLocation = departureLocation;
            this.destinationLocation = destinationLocation;
            this.departureTijd = departureTijd;
        }
        public departureModel()
        {
        }

        // Gives vertrekModel model back
        public departureModel vertrekModel(string departureLocation, string destinationLocation, string departureTijd)
        {
            return new departureModel(departureLocation, destinationLocation, departureTijd);
        }

    }
}
