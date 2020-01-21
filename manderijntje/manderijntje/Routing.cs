using System.Collections.Generic;
using System;
using System.Linq;

namespace manderijntje
{
    //class Routing
    //{ 
    //    //starts the dijkstra search algorithm and returns the fastest route through the datamodel
    //    public static List<Node> GetShortestPathDijkstra(string startName, string endName, DateTime time, DataModel dataModel)
    //    {
    //        Node start = dataModel.GetNodeName(startName, dataModel.GetNodesRouting());
    //        Node end = dataModel.GetNodeName(endName, dataModel.GetNodesRouting());
    //        DijkstraSearch(start, end, time);
    //        var shortestPath = new List<Node> {end};
    //        BuildShortestPath(shortestPath, end);
    //        shortestPath.Reverse();
    //        return shortestPath;
    //    }
    //    //After DijkstraSearch BuildShortestPath does make the shortest path to the end so GetShortestPathDijkstra can return a list of the shortest path.
    //    private static void BuildShortestPath(List<Node> list, Node node)
    //    {
    //        if (node.NearestToStart == null)
    //            return;
    //        list.Add(node.NearestToStart);
    //        BuildShortestPath(list, node.NearestToStart);
    //    }
    //    /*DijkstraSearch searches for the fastest route through the network by arranging the nodes and links from fastest to slowest.
    //     If DijkstaSearch finds the end, it will stop searching and the fastest route is found*/

    //    private static void DijkstraSearch(Node start, Node end, DateTime time)
    //    {
    //        start.MinCostToStart = 0;
    //        var prioQueue = new List<Node> {start};
    //        while (prioQueue.Any())
    //        {
    //            prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
    //            var node = prioQueue.First();
    //            prioQueue.Remove(node);
    //            foreach (var link in node.Connections.OrderBy(x => x.Weight))
    //            {
    //                int tempCost = node.MinCostToStart + link.Weight;
    //                var childNode = link.End;
    //                if (childNode.Visited)
    //                    continue;
    //                if (tempCost < childNode.MinCostToStart)
    //                {
    //                    childNode.MinCostToStart = tempCost;
    //                    childNode.NearestToStart = node;
    //                    if (!prioQueue.Contains(childNode))
    //                        prioQueue.Add(childNode);
    //                }
    //            }

    //            node.Visited = true;
    //            if (node == end)
    //                return;
    //        }
    //    }
    //GetRoute is the method to call for getting the shortest path and all the information needed to show
//    public static List<Route> GetRoute(string startName, string endName, DateTime time, DataModel dataModel)
//    {
//        int transfers = 0;
//        List<Route> ListRoute = new List<Route>();
//        DateTime starttime = time;
//        for (int i = 0; i < 10; i++)
//        {
//            Route fastestRoute = new Route(startName, endName, transfers, time, dataModel);
//            ListRoute.Add(fastestRoute);
//            starttime = fastestRoute.startTime.AddMinutes(1);
//            TravelInformation test = TimeModel.GetTravelCost(time, startName, endName);
//            foreach (Node node in dataModel.nodesrouting)
//            {
//                node.MinCostToStart = int.MaxValue;
//                node.NearestToStart = null;
//                node.Visited = false;
//            }
//        }
//        return ListRoute;
//    }
//}

//Route is an object to show all the information
class Route
{
    public List<Node> shortestPath; //list of the shortest path
    public DateTime startTime; //time the train will depart from the first station
    public DateTime endTime; //time the train should arrive at the destination
    public int transfers; //amount of transfers to another train

    public Route(string startName, string endName, int totaltransfers, DateTime time, DataModel dataModel)
    {
        shortestPath = Routing.GetShortestPathDijkstra(startName, endName, time, dataModel);
        startTime = time;
        endTime = shortestPath.Last().MinCostToStart;
        transfers = totaltransfers;
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
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            BuildShortestPath(list, node.NearestToStart);
        }
        /*DijkstraSearch searches for the fastest route through the network by arranging the nodes and links from fastest to slowest.
         If DijkstaSearch finds the end, it will stop searching and the fastest route is found*/

        private static void DijkstraSearch(Node start, Node end, DateTime time, DataModel dataModel)
        {
            start.MinCostToStart = time;
            var prioQueue = new List<Node> { start };
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var link in node.Connections.OrderBy(x => Link.getdeparttime(DataModel.GetLink(x.Start.number, x.End.number, dataModel.links), node.MinCostToStart) + x.Weight))
                {
                    DateTime test = Link.getdeparttime(DataModel.GetLink(link.Start.number, link.End.number, dataModel.links), node.MinCostToStart);
                    DateTime tempCost = Link.getdeparttime(DataModel.GetLink(link.Start.number, link.End.number, dataModel.links), node.MinCostToStart) + link.Weight;
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
                Route fastestRoute = new Route(startName, endName, transfers, starttime, dataModel);
                ListRoute.Add(fastestRoute);
                starttime = fastestRoute.startTime + new TimeSpan(1, 0, 0);
                foreach (Node node in dataModel.nodes)
                {
                    node.MinCostToStart = DateTime.MaxValue;
                    node.NearestToStart = null;
                    node.Visited = false;
                }
            }
            return ListRoute;
        }
    }
}
