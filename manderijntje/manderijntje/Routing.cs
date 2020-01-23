using System.Collections.Generic;
using System;
using System.Linq;

namespace manderijntje
{
//Route is an object to show all the information
class Route
{
    public List<Node> shortestPath = new List<Node>(); //list of the shortest path
    public DateTime startTime; //time the train will depart from the first station
    public DateTime endTime; //time the train should arrive at the destination

    public Route(string startName, string endName, DateTime time, DataModel dataModel)
    {
        List<Node>  tempshortestpath = Routing.GetShortestPathDijkstra(startName, endName, time, dataModel);
        foreach (Node node in tempshortestpath)
        {
            Node tempnode = new Node(node.x, node.y, node.stationname, node.country, node.number);
            tempnode.minCostToStart = node.minCostToStart;
            tempnode.connections = node.connections;
            tempnode.neighbours = node.neighbours;
            tempnode.nearestToStart = node.nearestToStart;
            shortestPath.Add(tempnode);
        }
        startTime = time;
        endTime = shortestPath.Last().minCostToStart;
    }
}

class Routing
    {
        //starts the dijkstra search algorithm and returns the fastest route through the datamodel
        public static List<Node> GetShortestPathDijkstra(string startName, string endName, DateTime time,
            DataModel dataModel)
        {
            Node start = dataModel.GetNodeName(startName, dataModel.nodes);
            Node end = dataModel.GetNodeName(endName, dataModel.nodes);
            DijkstraSearch(start, end, time, dataModel);
            var shortestPath = new List<Node> { end };
            BuildShortestPath(shortestPath, end);
            shortestPath.Reverse();
            return shortestPath;
        }

        //After DijkstraSearch BuildShortestPath does make the shortest path to the end so GetShortestPathDijkstra can return a list of the shortest path.
        private static void BuildShortestPath(List<Node> list, Node node)
        {
            if (node.nearestToStart == null)
                return;
            list.Add(node.nearestToStart);
            BuildShortestPath(list, node.nearestToStart);
        }
        
        /*DijkstraSearch searches for the fastest route through the network by arranging the nodes and links from fastest to slowest.
         If DijkstaSearch finds the end, it will stop searching and the fastest route is found*/
        private static void DijkstraSearch(Node start, Node end, DateTime time, DataModel dataModel)
        {
            start.minCostToStart = time;
            var prioQueue = new List<Node> { start };
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.minCostToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var link in node.connections.OrderBy(x => Link.GetDepartTime(DataModel.GetLink(x.start.number, x.end.number, dataModel.links), node.minCostToStart) + x.weight))
                {
                    DateTime tempCost = Link.GetDepartTime(DataModel.GetLink(link.start.number, link.end.number, dataModel.links), node.minCostToStart) + link.weight;
                    var childNode = link.end;
                    if (childNode.visited)
                        continue;
                    if (tempCost < childNode.minCostToStart)
                    {
                        childNode.minCostToStart = tempCost;
                        childNode.nearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }
                node.visited = true;
                if (node == end)
                    return;
            }
        }

        //Method to ask for the 20 fastest routes
        public static List<Route> GetRoute(string startName, string endName, DateTime time, DataModel dataModel)
        {
            List<Route> listRoute = new List<Route>();
            DateTime starttime = time;
            for (int i = 0; i < 20; i++)
            {
                Route fastestRoute = new Route(startName, endName, starttime, dataModel);
                listRoute.Add(fastestRoute);
                starttime = fastestRoute.startTime + new TimeSpan(1, 0, 0);
                foreach (Node node in dataModel.nodes)
                {
                    node.minCostToStart = DateTime.MaxValue;
                    node.nearestToStart = null;
                    node.visited = false;
                }
            }
            return listRoute;
        }
    }
}
