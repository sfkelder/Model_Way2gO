namespace Manderijntje
{
    public class TransferModel
    {
        public string station { get; set; }
        public string platform { get; set; }
        public string arrivalTime { get; set; }
        public string departureTime { get; set; }
        public string typeTranssport { get; set; }
        public string toStation { get; set; }

        public TransferModel(string station, string platform, string arrivalTime, string departureTime, string typeTranssport, string toStation)
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
