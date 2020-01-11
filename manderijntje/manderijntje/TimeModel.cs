using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace manderijntje
{
    class TimeModel
    {
        #region Parameters: Links and Keys
        const string errorMessage = "404";
        const int error = -404;

        //NS API:
        private const string getStationsURLNetherlands = @"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/stations";
        private const string keyNameNS = "Ocp-Apim-Subscription-Key";
        private const string apiKeyNS = "9e3dcf30932a4e98bba202a3f02b658d";

        //IRail API:
        private const string getStationsURLBelgium = @"http://api.irail.be/stations/?format=json&lang=en";

        //France API:
        private static string getStationsURLFrench = @"https://api.sncf.com/v1/coverage/sncf/";
        private static string apiKeySCNF = "08432b68-9586-4381-85df-44672219de59";

        //UK API:        
        private const string appIDHERE = "4a34f260";
        private const string apiKeyHERE = "ebf96ffa065575d5fdd8f49bbf74e72f";

        //Germany API:
        private const string apiKeyGermany = "bd48301499211870c8b6ed686b7404f4";
        #endregion
        #region Tools
        //Verwerkt Unix Timestamps naar een DateTime object
        static DateTime UnixToDateTime(string timestamp)
        {
            double seconds = double.Parse(timestamp);
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(seconds).ToLocalTime();
            return time;
        }
        //Stuurt een Request (met een sleutel en indien toegevoegd, een extra header die nodig is in de HTTP Request) naar een site om de JSON op te halen
        static string Request(string url, string key, string sleutelNaam)
        {
            //Creates the HTTP Request to the required site
            string html = string.Empty;
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(url);
            GETRequest.Method = "GET";

            //Voegt een API-sleutel toe, indien deze nodig is om de site te kunnen bereiken.
            if (key != "" && sleutelNaam == "") GETRequest.Headers["Authorization"] = key;
            //Voegt een header toe, als dat nodig is om de Request te sturen
            if (sleutelNaam != "") GETRequest.Headers.Add(sleutelNaam, key);

            //Stuurt de HTTP Request door naar de site
            HttpWebResponse GETResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream GETResponseStream = GETResponse.GetResponseStream();
            StreamReader Response = new StreamReader(GETResponseStream);

            //Returns de tekst verkregen van de site
            return Response.ReadToEnd(); //Json text
        }
        //Stuurt Request zonder sleutel
        static string Request(string url)
        {
            return Request(url, "", "");
        }
        //Stuurt Request met één enkele sleutel, waarbij geen extra Header toegevoegd moet worden
        static string Request(string url, string key)
        {
            return Request(url, key, "");
        }
        #endregion
        #region GetParameter
        static string GetUICCode(string stadsnaam)
        {
            if (!File.Exists("GetUICCODE.txt"))
                CreateFileUICCODE();

            using (System.IO.StreamReader stationList = new System.IO.StreamReader("GetUICCODE.txt"))
            {
                string uicCode = "";
                string regel;
                while ((regel = stationList.ReadLine()) != null)
                {
                    if (regel.EndsWith(stadsnaam))
                    {
                        uicCode = regel.Split()[0]; return uicCode;
                    }
                }
            }
            return errorMessage; //In case of error
        }
        static string GetStationIDBelgium(string stadsnaam)
        {
            if (!File.Exists("GetStationIDBelgium.txt"))
                CreateStationIDBelgium();

            using (System.IO.StreamReader stationList = new System.IO.StreamReader("GetStationIDBelgium.txt"))
            {
                string stationID = "";
                string regel;
                while ((regel = stationList.ReadLine()) != null)
                {
                    if (regel.EndsWith(stadsnaam))
                    {
                        stationID = regel.Split()[0]; return stationID;
                    }
                }
            }
            return errorMessage; //In case of error 
        }
        static string GetStationIDGermany(string stadsnaam)
        {
            const string linkBase = @"https://api.deutschebahn.com/freeplan/v1/location/";
            var data = JArray.Parse(Request(linkBase + stadsnaam.Replace(" ", "%20")));
            string name = "";
            for (int i = 0; i < data.Count; i++)
            {
                name = (string)data[i]["name"];
                if (name.Replace('(', ' ').Replace(')', ' ').Trim() == stadsnaam)
                {
                    return (string)data[i]["id"];
                }
            }
            return errorMessage;
        }
        #endregion
        #region GetTravelCost
        static int GetTravelCost(DateTime time, string vertrekStation, string aankomstStation)
        {
            if ((GetUICCode(vertrekStation) != errorMessage) && (GetUICCode(vertrekStation) != errorMessage)) return GetTravelCostNS(time, vertrekStation, aankomstStation);
            if ((GetStationIDBelgium(vertrekStation) != errorMessage) && (GetStationIDBelgium(aankomstStation) != errorMessage)) return GetTravelCostBelgium(time, vertrekStation, aankomstStation);
            //if ((GetStationCODE_UK(vertrekStation) != errorMessage) && (GetStationCODE_UK(aankomstStation) != errorMessage)) return GetTravelCostUK(time, vertrekStation, aankomstStation);
            //if ((GetStationCoordsFrance(vertrekStation) != errorMessage) && (GetStationCoordsFrance(aankomstStation) != errorMessage)) return GetTravelCostFrance(time, vertrekStation, aankomstStation);
            if ((GetStationIDGermany(vertrekStation) != errorMessage) && (GetStationIDGermany(aankomstStation) != errorMessage)) return GetTravelCostGermany(time, vertrekStation, aankomstStation);
            return error;
        }

        //De volgende functies halen het verschil tussen de vertrektijd en aankomsttijd van twee stations op. DateTime is disfunctional omdat de NS geen API's kan maken.
        static int GetTravelCostNS(DateTime time, string vertrekStation, string aankomstStation)
        {
            //Variables:
            

            //Stelt de links op
            string timeOfChoice = time.ToString("yyyy" + "MM" + "dd" + @"T" + "hh" + @":" + "mm");
            const string linkBase = @"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/";

            string linkVertrek = linkBase + @"departures?dateTime=" + timeOfChoice + @"maxJourneys=25&lang=nl&uicCode=" + GetUICCode(vertrekStation);
            string linkAankomst = linkBase + @"arrivals?dateTime=" + timeOfChoice + @"maxJourneys=25&lang=nl&uicCode=" + GetUICCode(aankomstStation);

            //Zet DateTime om Error op te kunnen halen indien er geen DateTime wordt opgehaald
            DateTime aankomstTijd = DateTime.Now;
            DateTime vertrekTijd = aankomstTijd;

            var dataVertrek = (JObject)JsonConvert.DeserializeObject(Request(linkVertrek, apiKeyNS, keyNameNS));
            var vertrekInfo = dataVertrek["payload"]["departures"].Children();

            var dataAankomst = (JObject)JsonConvert.DeserializeObject(Request(linkAankomst, apiKeyNS, keyNameNS));
            var aankomstInfo = dataAankomst["payload"]["arrivals"].Children();

            foreach (var aankomst in aankomstInfo)
            {
                foreach (var vertrek in vertrekInfo)
                {
                    if (JToken.DeepEquals(vertrek["name"], aankomst["name"])) //Kijkt of de trein vanaf het vertrek station detzelfde trein is als bij het aankomst station
                    {
                        aankomstTijd = aankomst["actualDateTime"].Value<DateTime>();
                        vertrekTijd = vertrek["actualDateTime"].Value<DateTime>();
                        if (aankomstTijd > vertrekTijd)
                        {
                            int travelcost = (int)(aankomstTijd - vertrekTijd).TotalMinutes;
                            return travelcost; //TravelInformation(vertrekStation, vertrekParron, vertrekTijd, aankomstStation, aankomstParron, aankomstTijd, travelcost, vehicleID, vervoerder);
                        }
                        break;
                    }
                }
            }
            return (aankomstTijd == vertrekTijd) ? error : (int)((aankomstTijd - vertrekTijd).TotalMinutes);
        }
        static int GetTravelCostBelgium(DateTime time, string vertrekStation, string aankomstStation)
        {

            string linkBase = @"http://api.irail.be/liveboard/?id=";
            string dateOfChoice = time.Date.ToString("dd" + "MM" + "yy");
            string timeOfChoice = time.ToString("HH" + "mm");

            string linkVertrek = linkBase + GetStationIDBelgium(vertrekStation) + @"&date=" + dateOfChoice + @"&time" + timeOfChoice + @"&arrdep=departure&lang=en&format=json&fast=false&alerts=false";
            string linkAankomst = linkBase + GetStationIDBelgium(aankomstStation) + @"&date=" + dateOfChoice + @"&time" + timeOfChoice + @"&arrdep=arrival&lang=en&format=json&fast=false&alerts=false";

            DateTime aankomstTijd = DateTime.Now;
            DateTime vertrekTijd = aankomstTijd;

            var dataVertrek = (JObject)JsonConvert.DeserializeObject(Request(linkVertrek));
            var vertrekInfo = dataVertrek["departures"]["departure"].Children();

            var dataAankomst = (JObject)JsonConvert.DeserializeObject(Request(linkAankomst));
            var aankomstInfo = dataAankomst["arrivals"]["arrival"].Children();

            foreach (var aankomst in aankomstInfo)
            {
                foreach (var vertrek in vertrekInfo)
                {
                    if (JToken.DeepEquals(vertrek["vehicle"], aankomst["vehicle"]))
                    {
                        aankomstTijd = UnixToDateTime(aankomst["time"].Value<string>());
                        vertrekTijd = UnixToDateTime(vertrek["time"].Value<string>());
                        if (aankomstTijd > vertrekTijd)
                        {
                            return (int)((aankomstTijd - vertrekTijd).TotalMinutes);
                        }
                        break;
                    }
                }
            }
            return error;
        }
        static int GetTravelCostGermany(DateTime time, string vertrekStation, string aankomstStation)
        {
            const string linkBase = @"https://api.deutschebahn.com/freeplan/v1/departureBoard/";

            string timeOfChoice = time.ToString("yyyy-MM-dd");
            string linkVertrek = linkBase + GetStationIDGermany(vertrekStation) + "?date=" + timeOfChoice;
            string linkAankomst = linkBase + GetStationIDGermany(aankomstStation) + "?date=" + timeOfChoice;

            DateTime aankomstTijd = DateTime.Now;
            DateTime vertrekTijd = aankomstTijd;

            var dataVertrek = JArray.Parse(Request(linkVertrek, "Bearer " + apiKeyGermany));
            var dataAankomst = JArray.Parse(Request(linkAankomst, "Bearer " + apiKeyGermany));
            Console.WriteLine(Request(linkVertrek, apiKeyGermany));
            Console.WriteLine(Request(linkAankomst, apiKeyGermany));

            for (int i = 0; i < dataAankomst.Count; i++)
            {
                for (int j = 0; j < dataVertrek.Count; j++)
                {
                    if (JToken.DeepEquals(dataAankomst[i]["name"], dataVertrek[j]["name"]))
                    {
                        aankomstTijd = dataAankomst[i]["dateTime"].Value<DateTime>();
                        vertrekTijd = dataVertrek[j]["dateTime"].Value<DateTime>();
                        if (aankomstTijd > vertrekTijd)
                        {
                            return (int)((aankomstTijd - vertrekTijd).TotalMinutes);
                        }
                        break;
                    }
                }
            }
            return (aankomstTijd == vertrekTijd) ? error : (int)((aankomstTijd - vertrekTijd).TotalMinutes);
        }
        #endregion
        #region CreateOfflineFiles
        static void CreateFileUICCODE() //Creërt een file zodat alle UICCODE's van Nederland offline op te halen zijn voor gebruik NS API
        {
            //Streamwriter variabelen
            StreamWriter StationList = new StreamWriter("GetUICCODE.txt");

            //Haalt de benodigde informatie uit de JSON file
            var data = (JObject)JsonConvert.DeserializeObject(Request(getStationsURLNetherlands, apiKeyNS, keyNameNS));
            var payload = data["payload"].Children();

            //Pakt de stations en UICCODE's bij elkaar en schrijft ze naar de file
            foreach (var station in payload)
            {
                var uiccode = station["UICCode"].Value<string>();
                var stationsnaam = station["namen"]["lang"].Value<string>();

                StationList.WriteLine(uiccode + " " + stationsnaam);
            }
            StationList.Close();
        }
        static void CreateStationIDBelgium()//Creërt een file zodat alle stationID's van België offline op te halen zijn voor gebruik API 
        {
            //Streamwriter variabelen
            StreamWriter StationList = new StreamWriter("GetStationIDBelgium.txt");

            //Haalt de benodigde informatie uit de JSON file
            var data = (JObject)JsonConvert.DeserializeObject(Request(getStationsURLBelgium));
            var payload = data["station"].Children();

            //Pakt de stations en ID's bij elkaar en schrijft ze naar een file
            foreach (var station in payload)
            {
                var stationID = station["id"].Value<string>();
                var stationsnamen = station["standardname"].Value<string>();
                //Indien er sprake is van twee namen voor één station, worden ze beide toegevoegd
                foreach (var stationsnaam in stationsnamen.Split('/'))
                    StationList.WriteLine(stationID + " " + stationsnaam);
            }
            StationList.Close();
        }
        #endregion
    }
    /*
    class TravelInformation
    {
        public TravelInformation()
        {
            string from;
            string vertrekParron;
            DateTime vertrekTijd;
            string to;
            string aankomstParron;
            DateTime aankomstTijd;
            string origin;
            int travelcost;
            string vehicleID;
            string vervoerder;
        }
    }
    */
}