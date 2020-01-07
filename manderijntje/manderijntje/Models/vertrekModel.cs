using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class Vertrek
    {
        public string beginLocatie { get; set; }
        public string eindLocatie { get; set; }
        public string vertrekTijd { get; set; }

        public Vertrek(string beginLocatie, string eindLocatie, string vertrekTijd)
        {
            this.beginLocatie = beginLocatie;
            this.eindLocatie = eindLocatie;
            this.vertrekTijd = vertrekTijd;
        }
        public Vertrek()
        {
        }

        //
        // Geeft een vertrekModel model terug.
        //
        public Vertrek vertrekModel(string bLocatie, string eLocatie, string vTijd)
        {
            return new Vertrek(bLocatie, eLocatie, vTijd);
        }

    }
}
