using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace manderijntje
{ 
    class Routing
    { 

        public static List<Node> GetShortestPathDijkstra(string startName, string endName, DateTime time, DataModel dataModel)
        {
            Node start = dataModel.GetNodeName(startName, dataModel.GetNodesRouting());
            Node end = dataModel.GetNodeName(endName, dataModel.GetNodesRouting());
            DijkstraSearch(start, end, time);
            var shortestPath = new List<Node> {end};
            BuildShortestPath(shortestPath, end);
            shortestPath.Reverse();
            return shortestPath;
        }
         
        private static void BuildShortestPath(List<Node> list, Node node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            BuildShortestPath(list, node.NearestToStart);
        }

        private static void DijkstraSearch(Node start, Node end, DateTime time)
        {
            start.MinCostToStart = 0;
            var prioQueue = new List<Node> {start};
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var link in node.Connecties.OrderBy(x => x.Weight))
                {
                    int tempCost = node.MinCostToStart + link.Weight;
                    var childNode = link.End;
                    if (childNode.Visited)
                        continue;
                    if (tempCost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = tempCost;
                        childNode.NearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }

                node.Visited = true;
                if (node == end)
                    return;
            }
        }

        public static List<Route> GetRoute(string startName, string endName, DateTime time, DataModel dataModel)
        {
            int transfers = 0;
            List<Route> ListRoute = new List<Route>();
            DateTime starttime = time;
            for (int i = 0; i < 10; i++)
            {
                Route fastestRoute = new Route(startName, endName, transfers, time, dataModel);
                ListRoute.Add(fastestRoute);
                starttime = fastestRoute.startTime.AddMinutes(1);
                foreach (Node node in dataModel.nodesrouting)
                {
                    node.MinCostToStart = int.MaxValue;
                    node.NearestToStart = null;
                    node.Visited = false;
                }
            }
            return ListRoute;
        }
    }

    class Route
    {
        public List<Node> shortestPath;
        public DateTime startTime;
        public DateTime endTime;
        public int transfers;

        public Route(string startName, string endName, int totaltransfers, DateTime time, DataModel dataModel)
        {
            shortestPath = Routing.GetShortestPathDijkstra(startName, endName, time, dataModel);
            startTime = time;
            endTime = time.Add(TimeSpan.FromMinutes(shortestPath.Last().MinCostToStart));
            transfers = totaltransfers;
        }
    }
}

/* mincosttostart - nodes
 * visited - nodes
 * nearest to start - nodes
 * weight - links
 */