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
            Read_Data_From_File(nodes, links, routes, dataModel);
        }


        //Writing the dataModel
        public static void Read_Data_From_File(string nodes, string links, string routes, DataModel dataModel)
        {
            Read_Data_Nodes(nodes, dataModel);
            Read_Data_Links(links, dataModel);
            Read_Data_Routes(routes, dataModel);
        }

        //Writing nodes for the dataModel
        public static void Read_Data_Nodes(string nodes, DataModel dataModel)
        {
            var documentNodes = new StreamReader(new FileStream(nodes, FileMode.Open, FileAccess.Read));
            string line = documentNodes.ReadLine();
            while ((line = documentNodes.ReadLine()) != null)
            {
                string[] parametersNode = line.Split(',');
                dataModel.nodes.Add(new Node(double.Parse(parametersNode[1], CultureInfo.InvariantCulture), double.Parse(parametersNode[2], CultureInfo.InvariantCulture),
                    parametersNode[3], parametersNode[4], int.Parse(parametersNode[0], CultureInfo.InvariantCulture)));
            }
            documentNodes.Close();
        }

        //Writing links for the dataModel
        public static void Read_Data_Links(string links, DataModel dataModel)
        {
            var documentLinks = new StreamReader(new FileStream(links, FileMode.Open, FileAccess.Read));
            string line = documentLinks.ReadLine();
            while ((line = documentLinks.ReadLine()) != null)
            {
                string[] parametersLink = line.Split(',');
                Node node1 = dataModel.nodes[int.Parse(parametersLink[1], CultureInfo.InvariantCulture)];
                Node node2 = dataModel.nodes[int.Parse(parametersLink[2], CultureInfo.InvariantCulture)];
                dataModel.links.Add(new Link(node1, node2, parametersLink[0]));
                node1.neighbours.Add(node2);
                node2.neighbours.Add(node1);
                node1.connections.Add(new Link(node1, node2, parametersLink[0]));
                node2.connections.Add(new Link(node2, node1, parametersLink[0]));
            }
            documentLinks.Close();
        }

        //Writing routes for the dataModel
        public static void Read_Data_Routes(string routes, DataModel dataModel)
        {

            var documentRoutes = new StreamReader(new FileStream(routes, FileMode.Open, FileAccess.Read));
            string line;
            line = documentRoutes.ReadLine();
            while ((line = documentRoutes.ReadLine()) != null)
            {
                string[] parametersRoute = line.Split(';');
                DateTime start = DateTime.Today + new TimeSpan(int.Parse(parametersRoute[0]), int.Parse(parametersRoute[1]), 1);
                DateTime end = DateTime.Today + new TimeSpan(int.Parse(parametersRoute[2]), int.Parse(parametersRoute[3]), 1);
                int delay = int.Parse(parametersRoute[4]);
                int i = 5;
                while (i + 1 < parametersRoute.Length && parametersRoute[i + 1] != "")
                {
                    Link link = DataModel.GetLink(int.Parse(parametersRoute[i]),
                        int.Parse(parametersRoute[i + 1]), dataModel.links);
                    DateTime tempTime = start;
                    TimeSpan timeDelay = new TimeSpan(0, delay, 0);
                    while (tempTime <= end)
                    {
                        try
                        {
                            link.times.Add(tempTime);
                            link.times.Add(tempTime + new TimeSpan(1, 0, 0, 0));
                            link.times.Add(tempTime + new TimeSpan(2, 0, 0, 0));
                            tempTime = tempTime.Add(timeDelay);
                        }
                        catch { break; }
                    }

                    try
                    {
                        link.times = link.times.OrderBy(x => x.Day).ToList();
                    }
                    catch { Console.WriteLine("Link is empty"); }
                    i++;
                }
            }
            documentRoutes.Close();
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

        //method to search for a node in dataModel.nodes based on the name
        public Node GetNodeName(string name, List<Node> list)
        {
            foreach (Node node in list)
            {
                if (name == node.stationName)
                    return node;
            }

            return null;
        }

        //method to search for a link in dataModel.links
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
        //Name of the station
        public string stationName;
        //Station with lowest cost to return to the start (used in routing)
        public Node nearestToStart;
        //Minimum cost from this node to the start (used in routing)
        public DateTime minCostToStart = DateTime.MaxValue;
        //Checks if the routing algorithm already checked this node
        public bool visited = false;

        //constructor method for a node
        public Node(double coordX, double coordY, string nameStation, string countryStation, int i)
        {
            number = i;
            x = coordX;
            y = coordY;
            country = countryStation;
            stationName = nameStation;
            neighbours = new List<Node>();
            connections = new List<Link>();
        }
    }

    [Serializable]
    public class Link
    {
        // twee pointers die wijzen naar de twee nodes die deze link verbind
        public Node start, end;
        //unique ID of a link
        public string routeID;
        //Time needed to travel this link (used in routing)
        public TimeSpan weight = new TimeSpan(0, Int32.MaxValue, 0);
        //List of times at which a train departs from the station (used in routing)
        public List<DateTime> times = new List<DateTime>();

        //constructor method for a link
        public Link(Node starting, Node ending, string routeIds)
        {
            start = starting;
            end = ending;
            routeID = routeIds;
            weight = GetWeight(starting, ending);
        }

        //method to determine the weight of a link based on its distance and the speed of a train
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

        //mathematical method to convert a degree to a radian
        private double DegToRad(double x)
        {
            return (x * Math.PI / 180);
        }

        //method to get the first time a train departs over this link after a given time
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
