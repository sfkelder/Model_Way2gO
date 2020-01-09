using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace manderijntje
{ 
    class Routing
    {
        public int transfers;

        public List<Node> GetShortestPathDijkstra(string startName, string endName, DateTime time, DataModel dataModel)
        {
            Node start = dataModel.GetNodeName(startName, dataModel.GetNodesRouting());
            Node end = dataModel.GetNodeName(endName, dataModel.GetNodesRouting());
            DijkstraSearch(start, end, time);
            var shortestPath = new List<Node> {end};
            BuildShortestPath(shortestPath, end);
            shortestPath.Reverse();
            return shortestPath;
        }

        private void BuildShortestPath(List<Node> list, Node node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            BuildShortestPath(list, node.NearestToStart);
        }

        private void DijkstraSearch(Node start, Node end, DateTime time)
        {
            start.MinCostToStart = 0;
            var prioQueue = new List<Node> {start};
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var link in node. Connecties.OrderBy(x => x.Weight))
                {
                    var childNode = link.End;
                    if (childNode.Visited)
                        continue;
                    if (node.MinCostToStart + link.Weight < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + link.Weight;
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

        public Route GetRoute(string startName, string endName, DateTime time, DataModel dataModel)
        {
            Route fastestRoute = new Route(startName, endName, time, dataModel);
            return fastestRoute;
        }
    }

    class Route
    {
        public List<Node> shortestPath;
        public DateTime startTime;
        public DateTime endTime;
        public int transfers;

        public Route(string startName, string endName, DateTime time, DataModel dataModel)
        {
            Routing r = new Routing();
            shortestPath = r.GetShortestPathDijkstra(startName, endName, time, dataModel);
            startTime = time;
            //endTime = time.Add(TimeSpan.FromMinutes(shortestPath.Last().MinCostToStart));
            transfers = r.transfers;
        }
    }
}
