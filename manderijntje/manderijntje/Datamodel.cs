using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace Manderijntje
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
        public static void Read_Data_from_file(string nodes, string links, string routes, DataModel dataModel)
        {
            Read_Data_Nodes(nodes, dataModel);
            Read_Data_Links(links, dataModel);
            Read_Data_Routes(routes, dataModel);
        }

        //Writing nodes dataModel
        public static void Read_Data_Nodes(string nodes, DataModel dataModel)
        {
            var documentnodes = new StreamReader(new FileStream(nodes, FileMode.Open, FileAccess.Read));
            string line = documentnodes.ReadLine();
            while ((line = documentnodes.ReadLine()) != null)
            {
                string[] parametersnode = line.Split(',');
                dataModel.nodes.Add(new Node(double.Parse(parametersnode[1], CultureInfo.InvariantCulture), double.Parse(parametersnode[2], CultureInfo.InvariantCulture),
                    parametersnode[3], parametersnode[4], int.Parse(parametersnode[0], CultureInfo.InvariantCulture)));
            }
            documentnodes.Close();
        }

        //Writing links dataModel
        public static void Read_Data_Links(string links, DataModel dataModel)
        {
            var documentlinks = new StreamReader(new FileStream(links, FileMode.Open, FileAccess.Read));
            string line = documentlinks.ReadLine();
            while ((line = documentlinks.ReadLine()) != null)
            {
                string[] parameterslink = line.Split(',');
                Node node1 = dataModel.nodes[int.Parse(parameterslink[1], CultureInfo.InvariantCulture)];
                Node node2 = dataModel.nodes[int.Parse(parameterslink[2], CultureInfo.InvariantCulture)];
                dataModel.links.Add(new Link(node1, node2, parameterslink[0]));
                node1.neighbours.Add(node2);
                node2.neighbours.Add(node1);
                node1.connections.Add(new Link(node1, node2, parameterslink[0]));
                node2.connections.Add(new Link(node2, node1, parameterslink[0]));
            }
            documentlinks.Close();
        }

        //writing routes to dataModel
        public static void Read_Data_Routes(string routes, DataModel dataModel)
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
                        int.Parse(parametersroute[i + 1]), dataModel.links);
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
                    catch
                    {
                        Console.WriteLine("Link is empty");
                    }
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
                if ((link.start.number == start && link.end.number == end) ||
                    (link.start.number == end && link.end.number == start))
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
        public List<Link> connections;
        //real Coördinates
        public double x, y;
        //id of a Node
        public int number;
        //Country a node is located
        public string country;
        // unieke indentifier, naam in de vorm van een string
        public string stationname;
        public Node nearestToStart;
        public DateTime minCostToStart = DateTime.MaxValue;
        public bool visited = false;

        public Node(double coordX, double coordY, string nameStation, string countryStation, int i)
        {
            number = i;
            x = coordX;
            y = coordY;
            country = countryStation;
            stationname = nameStation;
            neighbours = new List<Node>();
            connections = new List<Link>();
        }
    }

    [Serializable]
    public class Link
    {
        // twee pointers die wijzen naar de twee nodes die deze link verbind
        public Node start, end;
        public string routeID;
        public TimeSpan weight = new TimeSpan(0, Int32.MaxValue, 0);
        public List<DateTime> times = new List<DateTime>();

        public Link(Node starting, Node ending, string routeIds)
        {
            start = starting;
            end = ending;
            routeID = routeIds;
            weight = GetWeight(starting, ending);
        }

        private TimeSpan GetWeight(Node start, Node end)
        {
            double dX = Math.Abs(start.x - end.x);
            double dY = Math.Abs(start.y - end.y);
            double kmX = dX * 110.574;
            double kmY = dY * (111.320 * Math.Cos(DegToRad(start.x)));

            double distance = kmX * kmX + kmY * kmY;
            double speed = 5.333333;
            double trackWeight = Math.Sqrt(distance / speed);

            return new TimeSpan(0, (int)trackWeight, 0);
        }

        private double DegToRad(double x)
        {
            return (x * Math.PI / 180);
        }

        public static DateTime GetDepartTime(Link link, DateTime arrival)
        {
            if (link.times.Count == 0)
                return arrival;
            if (link.times.Last() < arrival)
                return link.times.First();
            for (int i = 0; i < link.times.Count; i++)
            {
                if (arrival <= link.times[i])
                    return link.times[i];
            }

            return arrival;
        }
    }
}
