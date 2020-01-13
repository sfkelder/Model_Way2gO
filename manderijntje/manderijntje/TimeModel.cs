using System;
using System.IO;
using System.Net;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace manderijntje
{
    class TimeModel
    {
        #region Parameters: Links and Keys
        const string errorMessage = "404";

        //NS API:
        private const string getStationsURLNetherlands = @"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/stations";
        private const string keyNameNS = "Ocp-Apim-Subscription-Key";
        private const string apiKeyNS = "9e3dcf30932a4e98bba202a3f02b658d";

        //IRail API:
        private const string getStationsURLBelgium = @"http://api.irail.be/stations/?format=json&lang=en";

        //France API:
        private const string apiKeySCNF = "08432b68-9586-4381-85df-44672219de59";

        //UK API:        
        private const string appIDHERE = "4a34f260";
        private const string apiKeyHERE = "ebf96ffa065575d5fdd8f49bbf74e72f";

        //Germany API:
        private const string apiKeyGermany = "bd48301499211870c8b6ed686b7404f4";
        #endregion
        #region Tools
        //Processes an UNIX Timestamp into a standard DateTime
        static DateTime UnixToDateTime(string timestamp)
        {
            double seconds = double.Parse(timestamp);
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(seconds).ToLocalTime();
            return time;
        }

        //Changes strings that can't easily be converted to a datetime to a proper datetime
        static DateTime ConvertToDateTime(string time)
        {
            //Year-Month-DayTHour-Minute-Seconds to DateTime if there aren't any -'s between it.
            int year = (Int32.Parse(time.Substring(0, 4)));
            int month = (Int32.Parse(time.Substring(4, 2)));
            int day = (int.Parse(time.Substring(6, 2)));
            int hour = (int.Parse(time.Substring(9, 2)));
            int minute =  (int.Parse(time.Substring(11, 2)));
            int seconds = (int.Parse(time.Substring(13)));
            return new DateTime(year, month, day, hour, minute, seconds);
        }

        //Sends a Request to a website and receives data from it
        static string Request(string url, string key, string keyName, bool authorization)
        {
            //Creates a HTTP Request for the required site
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(url);
            getRequest.Method = "GET";

            //Adds username/password if API uses WWW-Authentication
            if (authorization == true) {
                string username = key;
                string password = "";
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                getRequest.Headers.Add("Authorization", "Basic " + svcCredentials);
            }
            //Adds an API-key, if it's needed to reach the site
            if (key != "" && keyName == "") getRequest.Headers["Authorization"] = key;
            //Adds a header, if it's needed to send the Request
            if (keyName != "" && authorization == false) getRequest.Headers.Add(keyName, key);

            //Sends the HTTP Request to the site and handles 404 errors   
            try
            {
                HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
                Stream getResponseStream = getResponse.GetResponseStream();
                StreamReader response = new StreamReader(getResponseStream);

                //Returns the text received from the site  
                string text = response.ReadToEnd();
                response.Close();
                return text; //Json or HTML text
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        //Sends a Request without key
        static string Request(string url)
        {
            return Request(url, "", "", false);
        }

        //Sends a Request with a key
        static string Request(string url, string key)
        {
            return Request(url, key, "", false);
        }

        //Sends a Request (with a key and a header if added) to the site to receive the JSON/HTML
        static string Request(string url, string key, string keyname)
        {
            return Request(url, key, keyname, false);
        }

        //Sends a Request with a single key without any headers
        static string Request(string url, string key, bool authentication)
        {
            if (authentication == true) return Request(url, key, "", true);
            return Request(url, key, "", false);
        }
        #endregion
        #region GetParameter
        //Gets the UIC Code from the requested station
        static string GetUICCode(string stationName)
        {
            if (!File.Exists("GetUICCODE.txt"))
                CreateFileUICCODE();

            using (System.IO.StreamReader stationList = new System.IO.StreamReader("GetUICCODE.txt"))
            {
                string uicCode = "";
                string line;
                while ((line = stationList.ReadLine()) != null)
                {
                    if (line.EndsWith(stationName)) 
                    {
                        uicCode = line.Split()[0]; return uicCode;
                    }
                }
            }
            return errorMessage; //In case the required station isn't found.
        }
        //The following functions get the Station ID from the requested station
        static string GetStationIDBelgium(string stationName)
        {
            if (!File.Exists("GetStationIDBelgium.txt"))
                CreateStationIDBelgium();

            using (System.IO.StreamReader stationList = new System.IO.StreamReader("GetStationIDBelgium.txt"))
            {
                string stationID = "";
                string line;
                while ((line = stationList.ReadLine()) != null)
                {
                    if (line.EndsWith(stationName))
                    {
                        stationID = line.Split()[0]; return stationID;
                    }
                }
            }
            return errorMessage; //In case the required station isn't found. 
        }
        static string GetStationIDGermany(string stationName) 
        {
            const string linkBase = @"https://api.deutschebahn.com/freeplan/v1/location/";
            var data = JArray.Parse(Request(linkBase + stationName.Replace(" ", "%20")));
            for(int i = 0; i < data.Count; i++)
            {
                string name = (string)data[i]["name"];
                if (name.Replace('(', ' ').Replace(')', ' ').Trim() == stationName) 
                {
                    return (string)data[i]["id"];                   
                }
            }
            return errorMessage; //In case the required station isn't found.
        } 
        static string GetStationIDFrance(string cityName) 
        {
            if (!File.Exists("GetStationIDFrance.txt"))
                CreateStationIDFrance();

            using (System.IO.StreamReader stationList = new System.IO.StreamReader("GetStationIDFrance.txt"))
            {
                string stationID = "";
                string line;
                while ((line = stationList.ReadLine()) != null)
                {
                    if (line.EndsWith(cityName))
                    {
                        stationID = line.Split()[0]; return stationID;
                    }
                }
            }
            return errorMessage; //In case the required station isn't found.
        }
        #endregion
        #region GetTravelCost

        //Gets the travelcost + other needed variables (which are in the TravelInformation object) between two stations
        static TravelInformation GetTravelCost(DateTime time, string departureStation, string arrivalStation) 
        {
            if ((GetUICCode(departureStation) != errorMessage) && (GetUICCode(departureStation) != errorMessage)) return GetTravelCostNS(time, departureStation, arrivalStation);
            if ((GetStationIDBelgium(departureStation) != errorMessage) && (GetStationIDBelgium(arrivalStation) != errorMessage)) return GetTravelCostBelgium(time, departureStation, arrivalStation);
            if ((GetStationIDFrance(departureStation) != errorMessage) && (GetStationIDFrance(arrivalStation) != errorMessage)) return GetTravelCostFrance(time, departureStation, arrivalStation);
            if ((GetStationIDGermany(departureStation) != errorMessage) && (GetStationIDGermany(arrivalStation) != errorMessage)) return GetTravelCostGermany(time, departureStation, arrivalStation);           
            return null;
        }

        //The following functions get the difference between the departuretime and the arrivaltime of two stations
        static TravelInformation GetTravelCostNS(DateTime time, string departureStation, string arrivalStation)
        {
            //Sets up the links
            string timeOfChoice = time.ToString("yyyy" + "MM" + "dd" + @"T" + "hh" + @":" + "mm");
            const string linkBase = @"https://gateway.apiportal.ns.nl/reisinformatie-api/api/v2/";
            string linkDeparture = linkBase + @"departures?dateTime=" + timeOfChoice + @"maxJourneys=25&lang=nl&uicCode=" + GetUICCode(departureStation);
            string linkArrival = linkBase + @"arrivals?dateTime=" + timeOfChoice + @"maxJourneys=25&lang=nl&uicCode=" + GetUICCode(arrivalStation);

            //Gets the data and converts it
            var dataDeparture = (JObject)JsonConvert.DeserializeObject(Request(linkDeparture, apiKeyNS, keyNameNS));
            if (dataDeparture == null) return null;
            var departureInfo = dataDeparture["payload"]["departures"].Children();    
            var dataArrival = (JObject)JsonConvert.DeserializeObject(Request(linkArrival, apiKeyNS, keyNameNS));
            if (dataArrival == null) return null;
            var arrivalInfo = dataArrival["payload"]["arrivals"].Children();

            foreach (var arrival in arrivalInfo)
            {
                foreach (var departure in departureInfo)
                {                   
                    if (JToken.DeepEquals(departure["name"], arrival["name"])) //Checks whether the train at the arrival station and the train at the departure station are the same train
                    {
                        DateTime arrivalTime = arrival["actualDateTime"].Value<DateTime>();
                        DateTime departureTime = departure["actualDateTime"].Value<DateTime>();
                        if (arrivalTime > departureTime)
                        {
                            string carrier = (string)departure["operatorName"];
                            string departurePlatform = (string)departure["plannedTrack"];
                            string arrivalPlatform = (string)arrival["plannedTrack"];
                            string direction = (string)departure["direction"];
                            string vehicleType = (string)departure["longCategoryName"];
                            int travelCost = (int)((arrivalTime - departureTime).TotalMinutes);           
                            return new TravelInformation(carrier, departureStation, departurePlatform, departureTime, arrivalStation, arrivalPlatform, arrivalTime, travelCost, vehicleType);
                        }
                        break;
                    }
                }
            }
            return null; //In case of error
        }
        static TravelInformation GetTravelCostBelgium(DateTime time, string departureStation, string arrivalStation)
        {
            //Sets up the links
            string linkBase = @"http://api.irail.be/liveboard/?id=";
            string dateOfChoice = time.Date.ToString("dd" + "MM" + "yy");
            string timeOfChoice = time.ToString("HH" + "mm");
            string linkDeparture = linkBase + GetStationIDBelgium(departureStation) + @"&date=" + dateOfChoice + @"&time" + timeOfChoice + @"&arrdep=departure&lang=en&format=json&fast=false&alerts=false";
            string linkArrival = linkBase + GetStationIDBelgium(arrivalStation) + @"&date=" + dateOfChoice + @"&time" + timeOfChoice + @"&arrdep=arrival&lang=en&format=json&fast=false&alerts=false";

            //Gets the data and converts it
            var dataDeparture = (JObject)JsonConvert.DeserializeObject(Request(linkDeparture));
            if (dataDeparture == null) return null;
            var departureInfo = dataDeparture["departures"]["departure"].Children();
            var dataArrival = (JObject)JsonConvert.DeserializeObject(Request(linkArrival));
            if (dataArrival == null) return null;
            var arrivalInfo = dataArrival["arrivals"]["arrival"].Children();

            foreach (var arrival in arrivalInfo)
            {
                foreach (var departure in departureInfo)
                {
                    if (JToken.DeepEquals(departure["vehicle"], arrival["vehicle"]))
                    {
                        DateTime arrivalTime = UnixToDateTime(arrival["time"].Value<string>());
                        DateTime departureTime = UnixToDateTime(departure["time"].Value<string>());
                        if (arrivalTime > departureTime)
                        {
                            string carrier = "NMBS/SNCB"; //NMBS is the same carrier as SNCB, but the names are different
                            string departurePlatform = (string)departure["platform"];
                            string arrivalPlatform = (string)arrival["platform"];
                            string vehicleType = (string)departure["vehicle"];
                            int travelCost = (int)((arrivalTime - departureTime).TotalMinutes);
                            return new TravelInformation(carrier, departureStation, departurePlatform, departureTime, arrivalStation, arrivalPlatform, arrivalTime, travelCost, vehicleType);                           
                        }
                        break;
                    }
                }
            }
            return null;
        }
        static TravelInformation GetTravelCostGermany(DateTime time, string departureStation, string arrivalStation) 
        {
            //Sets up the link
            const string linkBase = @"https://api.deutschebahn.com/freeplan/v1/departureBoard/";
            string timeOfChoice = time.ToString("yyyy-MM-dd");
            string linkDeparture = linkBase + GetStationIDGermany(departureStation) + "?date=" + timeOfChoice;
            string linkArrival = linkBase + GetStationIDGermany(arrivalStation) + "?date=" + timeOfChoice;

            //Gets the data and converts it
            var dataDeparture = JArray.Parse(Request(linkDeparture, "Bearer " + apiKeyGermany));
            if (dataDeparture == null) return null;
            var dataArrival = JArray.Parse(Request(linkArrival, "Bearer " + apiKeyGermany));
            if (dataArrival == null) return null;
            for (int i = 0; i < dataArrival.Count; i++)
            {
                for (int j = 0; j < dataDeparture.Count; j++)
                {
                    if (JToken.DeepEquals(dataArrival[i]["name"], dataDeparture[j]["name"]))
                    {
                        DateTime arrivalTime = dataArrival[i]["dateTime"].Value<DateTime>();
                        DateTime departureTime = dataDeparture[j]["dateTime"].Value<DateTime>();
                        if (arrivalTime > departureTime)
                        {
                            string carrier = "Deutsche Bahn";
                            string departurePlatform = (string)dataDeparture[j]["track"];
                            string arrivalPlatform = (string)dataArrival[i]["track"];
                            string vehicleType = (string)dataDeparture[j]["type"];
                            int travelCost = (int)((arrivalTime - departureTime).TotalMinutes);
                            return new TravelInformation(carrier, departureStation, departurePlatform, departureTime, arrivalStation, arrivalPlatform, arrivalTime, travelCost, vehicleType);                           
                        }
                        break;
                    }
                }
            }
            return null;
        } 
        static TravelInformation GetTravelCostFrance(DateTime time, string departureStation, string arrivalStation) 
        {
            //Sets up the link
            const string linkBase = @"https://api.sncf.com/v1/coverage/sncf/journeys?from=";
            string timeOfChoice = time.ToString("yyyy" + "MM" + "dd" + @"T" + "hh" + "mm");
            string link = linkBase + GetStationIDFrance(departureStation) + @"&to=" + GetStationIDFrance(arrivalStation) + "&datetime=" + timeOfChoice + "&datetime_represents=departure";

            //Gets the data and converts it
            var data = (JObject)JsonConvert.DeserializeObject(Request(link, apiKeySCNF, true));
            if (data == null) return null;
            foreach (JObject info in data["journeys"])
            {
                DateTime departureTime = ConvertToDateTime((string)info["departure_date_time"]);
                DateTime arrivalTime = ConvertToDateTime((string)info["arrival_date_time"]);
                if (arrivalTime > departureTime)
                {
                    string carrier = "SNCF";
                    string departurePlatform = null; //API doesn't show this.
                    string arrivalPlatform = null; //API doesn't show this.
                    string vehicleType = "train"; //API doesn't show this.
                    int travelCost = (int)((arrivalTime - departureTime).TotalMinutes);
                    return new TravelInformation(carrier, departureStation, departurePlatform, departureTime, arrivalStation, arrivalPlatform, arrivalTime, travelCost, vehicleType);
                }
            }
            return null;
        }
        #endregion
        #region CreateOfflineFiles
        static void CreateFileUICCODE() //Creates a file so all station IDs of France can be stored locally for future use of the API.
        {
            //Gets the JSON from the web and converts it
            var data = (JObject)JsonConvert.DeserializeObject(Request(getStationsURLNetherlands, apiKeyNS, keyNameNS));
            var payload = data["payload"].Children();

            //Streamwriter variables
            StreamWriter stationList = new StreamWriter("GetUICCODE.txt");

            //Takes the station and their UICcodes and writes them to a file
            foreach (var station in payload)
            {
                var uiccode = station["UICCode"].Value<string>();
                var stationsname = station["namen"]["lang"].Value<string>();

                stationList.WriteLine(uiccode + " " + stationsname);
            }
            stationList.Close();
        }
        static void CreateStationIDBelgium() //Creates a file so all station IDs of France can be stored locally for future use of the API. 
        { 
            //Gets the JSON from the web and converts it
            var data = (JObject)JsonConvert.DeserializeObject(Request(getStationsURLBelgium));
            var payload = data["station"].Children();

            //Streamwriter variables
            StreamWriter stationList = new StreamWriter("GetStationIDBelgium.txt");

            //Takes the station and their ID's and writes them to a file
            foreach (var station in payload)
            {
                var stationID = station["id"].Value<string>();
                var stationsnames = station["standardname"].Value<string>();
                //In case a station has two names, like Brussel-Noord/Bruxelles-Nord, both are written to the file
                foreach (var stationsname in stationsnames.Split('/'))
                    stationList.WriteLine(stationID + " " + stationsname);
            }
            stationList.Close();
        }
        static void CreateStationIDFrance() //Creates a file so all station IDs of France can be stored locally for future use of the API. 
        {
            //Parameters:
            StreamWriter stationList = new StreamWriter("GetStationIDFrance.txt");
            string linkBase = @"https://api.sncf.com/v1/coverage/sncf/stop_areas";

            for (int page = 0; page < 171; page++) //171 pages to get all stations
            {
                //Sets data
                var data = (JObject)JsonConvert.DeserializeObject(Request(linkBase + "?start_page=" + page, apiKeySCNF, true));

                //Takes the stations and their ID's and writes them to a file
                foreach (JObject payload in data["stop_areas"])
                {
                    var stationID = (string)payload["id"];
                    var stationsname = payload["name"].Value<string>();
                    stationList.WriteLine(stationID + " " + stationsname);
                }
            }
            stationList.Close();
        }
        #endregion
    }

    class TravelInformation
    {
        string Carrier { get; set; }
        string DepartureStation { get; set; }
        string DeparturePlatform { get; set; }
        DateTime DepartureTime { get; set; }
        string ArrivalStation { get; set; }
        string ArrivalPlatform { get; set; }
        DateTime ArrivalTime { get; set; }
        int TravelCost { get; set; }
        string VehicleType { get; set; }

        public TravelInformation(string c, string dStation, string dPlatform, DateTime dTime, string aStation, string aPlatform, DateTime aTime, int tC, string vT) 
        {
            Carrier = c;
            DepartureStation = dStation;
            DeparturePlatform = dPlatform;
            DepartureTime = dTime;
            ArrivalStation = aStation;
            ArrivalPlatform = aPlatform; 
            ArrivalTime = aTime;
            TravelCost = tC;
            VehicleType = vT;
        }
    }
}