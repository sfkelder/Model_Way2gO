using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace manderijntje
{
    class parsing
    {
        private List<sNode> nodes = new List<sNode>();
        private List<sLink> links = new List<sLink>();
        private List<sLinkPair> linkpairs = new List<sLinkPair>();
        private List<sBendLink> bendlinks = new List<sBendLink>();

        private const int width = 5000, height = 5000;

        public parsing(DataModel model)
        {
            if (model.unique_nodes.Count != 0 && model.unique_links.Count != 0)
            {
                setNodes(model.unique_nodes);   // mogelijk een bug met de coordinaten van het datamodel. mogelijk lat en long omgewisseld. kan problemen veroorzaken
                setLinks(model.unique_links);

                enforcePlanarity();
                setNeighbours();

                getLinkPairs();
                getBendLinks();
            }
        }

        public VisueelModel getModel (bool solve)
        {
            if (solve && getDegree() <= 8)
            {
                nodes = (new solver(nodes, links, linkpairs, bendlinks, width, height)).getSolution();
            }
            return createModel();
        }


        // planarity:

        private int getDegree()
        {
            int result = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].neighbours.Count > result)
                {
                    result = nodes[i].neighbours.Count;
                }
            }
            return result;
        }

        private void enforcePlanarity()
        {
            for (int i = 0; i < links.Count; i++)
            {
                for (int n = i; n < links.Count; n++)
                {
                    if (areLinksNonIncident(links[i], links[n]) && doLinksIntersect(links[i], links[n]))
                    {
                        insertNode(links[i], links[n]);
                    }
                }
            }
        }

        private void insertNode(sLink e1, sLink e2)
        {
            Point location = getIntersection(e1.u.x, e1.u.y, e1.v.x, e1.v.y, e2.u.x, e2.u.y, e2.v.x, e2.v.y);
            sNode node = new sNode(nodes.Count, location);
            node.neighbours = new List<sNode> { e1.u, e1.v, e2.u, e2.v };
            node.draw = false;

            links.Remove(e1);
            links.Remove(e2);

            links.Add(new sLink(node, e1.u));
            links.Add(new sLink(node, e1.v));
            links.Add(new sLink(node, e2.u));
            links.Add(new sLink(node, e2.v));

            nodes.Add(node);
        }

        private bool doLinksIntersect(sLink e1, sLink e2)
        {
            Point intersect = getIntersection(e1.u.x, e1.u.y, e1.v.x, e1.v.y, e2.u.x, e2.u.y, e2.v.x, e2.v.y);
            if (isPointInBounds(e1, e2, intersect))
            {
                return true;
            }
            return false;
        }

        private Point getIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {   // where (x1, y1), (x2, y2) definse a line and (x3, y3), (x4, y4) defines a line:

            double px_t = (((x1 * y2) - (y1 * x2)) * (x3 - x4)) - ((x1 - x2) * ((x3 * y4) - (y3 * x4)));
            double px_n = ((x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4));
            double py_t = (((x1 * y2) - (y1 * x2)) * (y3 - y4)) - ((y1 - y2) * ((x3 * y4) - (y3 * x4)));
            double py_n = ((x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4));

            if (px_n == 0 || py_n == 0)
            {
                return new Point(-1, -1);
            }

            return new Point((int)(px_t / px_n), (int)(py_t / py_n));
        }

        private bool isPointInBounds(sLink e1, sLink e2, Point p)
        {
            int min_x1 = Math.Min(e1.u.x, e1.v.x), max_x1 = Math.Max(e1.u.x, e1.v.x), min_y1 = Math.Min(e1.u.y, e1.v.y), max_y1 = Math.Max(e1.u.y, e1.v.y);
            int min_x2 = Math.Min(e2.u.x, e2.v.x), max_x2 = Math.Max(e2.u.x, e2.v.x), min_y2 = Math.Min(e2.u.y, e2.v.y), max_y2 = Math.Max(e2.u.y, e2.v.y);

            if (min_x1 < p.X && p.X < max_x1 && min_y1 < p.Y && p.Y < max_y1 && min_x2 < p.X && p.X < max_x2 && min_y2 < p.Y && p.Y < max_y2)
            {
                return true;
            }
            return false;
        }


        // populate extra arrays:

        private void getLinkPairs()
        {
            for (int i = 0; i < links.Count; i++)
            {
                for (int n = i; n < links.Count; n++)
                {
                    if (areLinksNonIncident(links[i], links[n]))
                    {
                        sLinkPair newPair = new sLinkPair(links[i], links[n]);
                        linkpairs.Add(newPair);
                    }
                }
            }
        }

        private void getBendLinks()
        {
            for (int i = 0; i < links.Count; i++)
            {
                for (int n = 0; n < links.Count; n++)
                {
                    if (areLinksAdjacent(links[i], links[n]))
                    {
                        bendlinks.Add(getBendLink(links[i], links[n]));
                    }
                }
            }
        }

        private sBendLink getBendLink(sLink e1, sLink e2)
        {
            sBendLink result;
            if (e1.v == e2.v)
            {
                result = new sBendLink(e1.u, e1.v, e2.u);

            }
            else if (e1.v == e2.u)
            {
                result = new sBendLink(e1.u, e1.v, e2.v);

            }
            else if (e1.u == e2.v)
            {
                result = new sBendLink(e1.v, e1.u, e2.u);

            }
            else   // e1.u == e2.u
            {
                result = new sBendLink(e1.v, e1.u, e2.v);

            }
            return result;
        }

        private bool areLinksNonIncident(sLink e1, sLink e2)
        {   // edges e1 and e2 don't share a common vertex
            if (e1 == e2)
            {
                return false;
            }
            if (e1.u == e2.u || e1.v == e2.u)
            {   // one of the vertexes of e1 in equal to e2.u
                return false;
            }
            if (e1.u == e2.v || e1.v == e2.v)
            {   // one of the vertexes of e1 in equal to e2.v
                return false;
            }
            return true;
        }

        private bool areLinksAdjacent(sLink e1, sLink e2)
        {
            if (e1 != e2)
            {
                if (e1.u == e2.u || e1.v == e2.u)
                {   // one of the vertexes of e1 in equal to e2.u
                    return true;
                }
                if (e1.u == e2.v || e1.v == e2.v)
                {   // one of the vertexes of e1 in equal to e2.v
                    return true;
                }
            }
            return false;
        }


        // init the nodes and links lists:

        private void setNodes(List<Node> dNodes)
        {
            for (int i = 0; i < dNodes.Count; i++)
            {
                dNodes[i].number = i;
            }

            List<Point> Coordinates = new List<Point>();
            for (int i = 0; i < dNodes.Count; i++)
            {
                // first argument is Lat, and the second argument is Long:
                Coordinates.Add(coordinates.GetLogicalCoordinate(dNodes[i].x, dNodes[i].y, 100000, 100000));
            }
            Point[] ScaledCoordinates = coordinates.ScalePointsToSize(Coordinates.ToArray(), width, height);
            for (int i = 0; i < dNodes.Count; i++)
            {
                sNode newNode = new sNode(dNodes[i].number, ScaledCoordinates[i]);
                nodes.Add(newNode);
            }
        }

        private void setNeighbours()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].neighbours = getNeighbours(nodes[i]);
                nodes[i].deg = nodes[i].neighbours.Count;
            }
        }

        private List<sNode> getNeighbours(sNode node)
        {
            List<sNode> result = new List<sNode>();

            for (int i = 0; i < links.Count; i++)
            {
                if (links[i].u == node)
                {
                    result.Add(links[i].v);
                }
                if (links[i].v == node)
                {
                    result.Add(links[i].u);
                }
            }

            return result;
        }

        private void setLinks(List<Link> dLinks)
        {
            for (int i = 0; i < dLinks.Count; i++)
            {
                sLink newLink = new sLink(getNode(dLinks[i].Start.number), getNode(dLinks[i].End.number));
                links.Add(newLink);
            }
        }

        private sNode getNode(int index)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].index == index)
                {
                    return nodes[i];
                }
            }
            return nodes[0];
        }


        // create a visual model object:

        private VisueelModel createModel()
        {
            VisueelModel model = new VisueelModel();

            List<VisueelNode> dNodes = new List<VisueelNode>();
            List<VisueelLink> dLinks = new List<VisueelLink>();

            for (int i = 0; i < nodes.Count; i++)
            {
                VisueelNode newNode = new VisueelNode(new Point(), "", 0);
                newNode.index = nodes[i].index;
                newNode.punt = new Point(nodes[i].x, nodes[i].y);
                newNode.dummynode = nodes[i].draw;

                dNodes.Add(newNode);
            }

            for (int i = 0; i < links.Count; i++)
            {
                VisueelLink newLink = new VisueelLink("");
                newLink.u = getNode(links[i].u.index, dNodes);
                newLink.v = getNode(links[i].v.index, dNodes);

                dLinks.Add(newLink);
            }

            model.nodes = dNodes;
            model.links = dLinks;

            return model;
        }

        private VisueelNode getNode(int i, List<VisueelNode> dNodes)
        {
            for (int n = 0; n < dNodes.Count; n++)
            {
                if (dNodes[n].index == i)
                {
                    return dNodes[n];
                }
            }
            return dNodes[0];
        }
    }

    class solver
    {
        private List<sNode> nodes;
        private List<sLink> links;
        private List<sLinkPair> linkpairs;
        private List<sBendLink> bendlinks;

        public solver (List<sNode> n, List<sLink> l, List<sLinkPair> p, List<sBendLink> b, int width, int height)
        {
            nodes = n; links = l; linkpairs = p; bendlinks = b;
        }

        public List<sNode> getSolution ()
        {
            // update the coordinates of the nodes first
            return nodes;
        }

        private int calc_sec(sNode u, sNode v)
        {
            double angle = calc_angle(u, v);

            if (22.5 <= angle && angle < 67.5)
            {
                return 1;
            }
            else if (67.5 <= angle && angle < 112.5)
            {
                return 2;
            }
            else if (112.5 <= angle && angle < 157.5)
            {
                return 3;
            }
            else if (157.5 <= angle && angle < 202.5)
            {
                return 4;
            }
            else if (202.5 <= angle && angle < 247.5)
            {
                return 5;
            }
            else if (247.5 <= angle && angle < 292.5)
            {
                return 6;
            }
            else if (292.5 <= angle && angle < 337.5)
            {
                return 7;
            }
            else  // 337.5 < angle && angle < 22.5
            {
                return 0;
            }
        }

        public static double calc_angle(sNode u, sNode v)
        {
            double t = v.x - u.x;
            double n = Math.Sqrt(Math.Pow((v.x - u.x), 2) + Math.Pow(v.y - u.y, 2));
            double alpha = Math.Acos(t / n);
            if (v.y - u.y < 0)
            {
                alpha = (2 * Math.PI) - alpha;
            }
            return (alpha * (180 / Math.PI));
        }
    }

    public class sNode
    {
        public int x, y, z1, z2, deg, index;
        public List<sNode> neighbours = new List<sNode>();
        public sNode[] neighbours_sorted;
        public string node_id;
        public bool draw = true;

        public sNode(int i, Point p)
        {
            index = i;
            x = p.X;
            y = p.Y;
        }

        public double getAngle(sNode u, sNode v)
        {
            return solver.calc_angle(u, v);
        }
    }

    public class sLink
    {
        public sNode u, v;
        public int sec_u_v, sec_v_u;

        public sLink(sNode U, sNode V)
        {
            u = U;
            v = V;
        }
    }

    public class sLinkPair
    {
        public sLink e1, e2;

        public sLinkPair(sLink E1, sLink E2)
        {
            e1 = E1;
            e2 = E2;
        }
    }

    public class sBendLink
    {
        public sNode u, v, w;

        public sBendLink(sNode U, sNode V, sNode W)
        {
            u = U;
            v = V;
            w = W;
        }
    }

    static class coordinates
    {
        public static Point GetLogicalCoordinate(double Lat, double Long, int width, int height)
        {
            return new Point(calc_x(Long, width), calc_y(Lat, width, height));
        }

        private static int calc_x(double Long, int width)
        {
            double result = (Long + 180) * (width / 360.0);

            return (int)result;
        }

        private static int calc_y(double Lat, int width, int height)
        {
            double latRad = (Lat * (Math.PI / 180));
            double mercN = Math.Log(Math.Tan((Math.PI / 4) + (latRad / 2)));
            int result = (int)((height / 2) - ((width * mercN) / (2 * Math.PI)));

            return result;
        }

        public static Point[] ScalePointsToSize(Point[] points, int width, int height)
        {
            double marge = 0.01 * Math.Min(width, height);
            int[] xs = new int[points.Length], ys = new int[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                xs[i] = points[i].X;
                ys[i] = points[i].Y;
            }

            double min_x = xs.Min(), min_y = ys.Min(), max_x = xs.Max(), max_y = ys.Max();
            for (int i = 0; i < points.Length; i++)
            {
                xs[i] -= (int)(min_x - marge);
                ys[i] -= (int)(min_y - marge);
            }

            double length_x = (max_x - min_x) + (2 * marge), length_y = (max_y - min_y) + (2 * marge);
            double factor = Math.Min((width / length_x), (height / length_y));
            for (int i = 0; i < points.Length; i++)
            {
                xs[i] = (int)(xs[i] * factor);
                ys[i] = (int)(ys[i] * factor);

                points[i].X = xs[i];
                points[i].Y = ys[i];
            }

            return points;
        }
    }
}
