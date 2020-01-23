 using System;
using System.Collections.Generic;
 using System.Linq;
 using System.IO;
using System.Globalization;

namespace manderijntje
{
    class DataControl
    {
        DataModel dataModel = new DataModel();
        private const string nodes = "C:/Way2Go/stationEurope.csv";
        private const string links = "C:/Way2Go/linkEurope.csv";
        private const string routes = "C:/Way2Go/routeEurope.csv";

        //Creating the DataControl and DataModel dataset.
        public DataControl()
        {
            Read_Data_from_file(nodes, links, routes, dataModel);
        }

        
        //Writing the dataModel
        public static void Read_Data_from_file(string nodes, string links, string routes, DataModel datamodel)
        {
            Read_Data_Nodes(nodes, datamodel);
            Read_Data_Links(links, datamodel);
            Read_Data_Routes(routes, datamodel);
        }

        //Writing nodes datamodel
        public static void Read_Data_Nodes(string nodes, DataModel datamodel)
        {
            var documentnodes = new StreamReader(new FileStream(nodes, FileMode.Open, FileAccess.Read));
            string line;
            line = documentnodes.ReadLine();
            while ((line = documentnodes.ReadLine()) != null)
            {
                string[] parametersnode = line.Split(',');
                datamodel.nodes.Add(new Node(double.Parse(parametersnode[1], CultureInfo.InvariantCulture), double.Parse(parametersnode[2], CultureInfo.InvariantCulture),
                    parametersnode[3], parametersnode[4], int.Parse(parametersnode[0], CultureInfo.InvariantCulture)));
            }
            documentnodes.Close();
        }

        //Writing links datamodel
        public static void Read_Data_Links(string links, DataModel datamodel)
        {
            var documentlinks = new StreamReader(new FileStream(links, FileMode.Open, FileAccess.Read));
            string line;
            line = documentlinks.ReadLine();
            while ((line = documentlinks.ReadLine()) != null)
            {
                string[] parameterslink = line.Split(',');
                Node node1 = datamodel.nodes[int.Parse(parameterslink[1], CultureInfo.InvariantCulture)];
                Node node2 = datamodel.nodes[int.Parse(parameterslink[2], CultureInfo.InvariantCulture)];
                datamodel.links.Add(new Link(node1, node2, parameterslink[0]));
                node1.neighbours.Add(node2);
                node2.neighbours.Add(node1);
                node1.Connections.Add(new Link(node1, node2, parameterslink[0]));
                node2.Connections.Add(new Link(node2, node1, parameterslink[0]));
            }
            documentlinks.Close();
        }

        //writing routes to datamodel
        public static void Read_Data_Routes(string routes, DataModel datamodel)
        {
            
            var documentroutes = new StreamReader(new FileStream(routes, FileMode.Open, FileAccess.Read));
            string line;
            line = documentroutes.ReadLine();
            while ((line = documentroutes.ReadLine()) != null)
            {
                string[] parametersroute = line.Split(';');
                DateTime start = DateTime.Today + new TimeSpan(int.Parse(parametersroute[0]), int.Parse(parametersroute[1]), 1);
                DateTime end = DateTime.Today + new TimeSpan(int.Parse(parametersroute[2]), int.Parse(parametersroute[3]), 1);
                int delay = int.Parse(parametersroute[4]);
                int i = 5;
                while (i + 1 < parametersroute.Length && parametersroute[i + 1] != "")
                {
                    Link link = DataModel.GetLink(int.Parse(parametersroute[i]),
                        int.Parse(parametersroute[i + 1]), datamodel.links);
                    DateTime temptime = start;
                    TimeSpan timedelay = new TimeSpan(0, delay, 0);
                    while (temptime <= end)
                    {
                        try
                        {
                            link.times.Add(temptime);
                            link.times.Add(temptime + new TimeSpan(1, 0, 0, 0));
                            link.times.Add(temptime + new TimeSpan(2, 0, 0, 0));
                            temptime = temptime.Add(timedelay);
                        }
                        catch
                        {
                            break;
                        }
                    }

                    try
                    {
                        link.times = link.times.OrderBy(x => x.Day).ToList();
                    }
                    catch { }
                    i++;
                }
            }
            documentroutes.Close();
        }

        //returns the dataModel
        public DataModel GetDataModel()
        {
            return dataModel;
        }
    }


    public class DataModel
    {
        public List<Node> nodes = new List<Node>();
        public List<Link> links = new List<Link>();

        public Node GetNodeName(string name, List<Node> list)
        {
            foreach (Node node in list)
            {
                if (name == node.stationname)
                    return node;
            }
            return null;
        }

        public static Link GetLink(int start, int end, List<Link> list)
        {
            foreach (Link link in list)
            {
                if ((link.Start.number == start && link.End.number == end) ||
                    (link.Start.number == end && link.End.number == start))
                    return link;
            }

            return null;
        }

    }


    [Serializable]
    public class Node
    {
        // array met pointers naar alle andere nodes waarmee deze verbonden is
        public List<Node> neighbours;

        // array met pointers naar alle links die verbonden zijn met deze node
        public List<Link> Connections;

        // 'echte' coordinaten
        public double x, y;

        public int number;

        public string country;

        // unieke indentifier, naam in de vorm van een string
        public string stationname;
        public Node NearestToStart;
        public DateTime MinCostToStart = DateTime.MaxValue;
        public bool Visited = false;

        public Node(double coordx, double coordy, string NameStation, string countryStation, int i)
        {
            number = i;
            x = coordx;
            y = coordy;
            country = countryStation;
            stationname = NameStation;
            neighbours = new List<Node>();
            Connections = new List<Link>();
        }
    }

    [Serializable]
    public class Link
    {
        // twee pointers die wijzen naar de twee nodes die deze link verbind
        public Node Start, End;
        public string RouteName;
        public TimeSpan Weight = new TimeSpan(0, Int32.MaxValue, 0); //the weight will be variable based on time and place but not for now
        public List<DateTime> times = new List<DateTime>();

        public Link(Node starting, Node ending, string RouteNames)
        {
            Start = starting;
            End = ending;
            RouteName = RouteNames;
            Weight = GetWeight(starting, ending);
        }

        private TimeSpan GetWeight(Node start, Node end)
        {
            double dX = Math.Abs(Start.x - End.x);
            double dY = Math.Abs(Start.y - End.y);
            double KmX = dX * 110.574;
            double KmY = dY * (111.320 * Math.Cos(DegToRad(Start.x)));

            double Distance = KmX * KmX + KmY * KmY;
            double speed = 5.333333;
            double trackWeight = Math.Sqrt(Distance / speed);

            return new TimeSpan(0, (int)trackWeight, 0);
        }

        private double DegToRad(double x)
        {
            return (x * Math.PI / 180);
        }

        public static DateTime GetDepartTime(Link link, DateTime Arrival)
        {
            if (link.times.Count == 0)
                return Arrival;
            if (link.times.Last() < Arrival)
                return link.times.First();
            for (int i = 0; i < link.times.Count; i++)
            {
                if (Arrival <= link.times[i])
                    return link.times[i];
            }

            return Arrival;
        }
    }
}
