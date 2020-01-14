using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using Gurobi;
 
namespace manderijntje
{
    class parsing
    {
        private List<sNode> nodes = new List<sNode>();
        private List<sLink> links = new List<sLink>();
        private List<sLinkPair> linkpairs = new List<sLinkPair>();
        private List<sBendLink> bendlinks = new List<sBendLink>();

        private const int width = 5000, height = 5000;

        public parsing(DataModel model, bool colapse)
        {
            if (model.unique_nodes.Count != 0 && model.unique_links.Count != 0)
            {
                setNodes(model.unique_nodes);
                setLinks(model.unique_links);
                setNeighbours();

                if (colapse)
                {
                    //colapseGraph();
                }

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
                nodes = (new solver(nodes, links, linkpairs, bendlinks)).getSolution(width, height);
            }
            return createModel();
        }

       /* public void test ()
        {
            using (StreamWriter w = new StreamWriter("/Users/Michael Bijker/Desktop/test_nodes2.txt"))
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    w.WriteLine(i + "," + nodes[i].x + "," + nodes[i].y + ",0,0");
                }
            }
            using (StreamWriter w = new StreamWriter("/Users/Michael Bijker/Desktop/test_links2.txt"))
            {
                for (int i = 0; i < links.Count; i++)
                {
                    w.WriteLine(i + "," + links[i].u.index + "," + links[i].v.index);
                }
            }

        }*/


        // PLANARITY:

        // returns the degree of the graph that is modeled by the list of nodes and the list of links
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

        // makes sure that there are no intersecting edges
        // if there are a node is inserted at the intersection
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

        // adds a node at the intersection of links e1 and e2
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

        // checks if the given links intersects
        private bool doLinksIntersect(sLink e1, sLink e2)
        {
            Point intersect = getIntersection(e1.u.x, e1.u.y, e1.v.x, e1.v.y, e2.u.x, e2.u.y, e2.v.x, e2.v.y);
            if (isPointInBounds(e1, e2, intersect))
            {
                return true;
            }
            return false;
        }

        // calculates the points of intersection of two lines, defined by line ((x1, y1) (x2, y2)) and line ((x3, y3) (x4, y4))
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

        // checks if the given points lies on both link e1 and e2
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


        // POPULATE EXTRA LISTS:

        // create and addes an instance of the link pair object to the list link pairs
        // for each pair of non incident links in the list links
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

        // create and addes an instance of the bend link object to the list bend links
        // for each pair of adjacent links in the list links
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

        // creates a bend link object such that (u, v) is a link in the list links
        // and that (v, w) is a link in the list links
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

        // checks if links e1 and e2 are non incident
        // that is they don't share a common node
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

        // checls if links e1 and e2 are adjacent
        // that is they share one common node
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


        // INIT THE NODES AND LINKS LISTS:

        // creates for every node in dNodes a new sNode object and adds it to the list nodes
        // and calculates the logical coordinates from the given Lat and Long
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
                Coordinates.Add(coordinates.GetLogicalCoordinate(dNodes[i].y, dNodes[i].x, 100000, 100000));
            }
            Point[] ScaledCoordinates = coordinates.ScalePointsToSize(Coordinates.ToArray(), width, height);
            for (int i = 0; i < dNodes.Count; i++)
            {
                sNode newNode = new sNode(dNodes[i].number, ScaledCoordinates[i]);
                newNode.name = dNodes[i].stationnaam;
                //newNode.weight = dNodes[i].
                nodes.Add(newNode);
            }
        }

        // populates the neighbours list for every node in the list nodes
        // using the list links
        private void setNeighbours()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].neighbours = getNeighbours(nodes[i]);
                nodes[i].deg = nodes[i].neighbours.Count;
                nodes[i].weight = nodes[i].neighbours.Count;
            }
        }

        // returns a list of all neighbours for a given node
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

        // creates for every link in dLinks a new sLink object and adds it to the list links
        private void setLinks(List<Link> dLinks)
        {
            for (int i = 0; i < dLinks.Count; i++)
            {
                sLink newLink = new sLink(getNode(dLinks[i].Start.number), getNode(dLinks[i].End.number));
                links.Add(newLink);
            }
        }

        // returns the sNode object with its index variable equal to i
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

        private void colapseGraph ()
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].deg == 2)
                {
                    sLink newLink = new sLink(nodes[i].neighbours[0], nodes[i].neighbours[1]);
                    links.Remove(getLink(nodes[i], nodes[i].neighbours[0]));
                    links.Remove(getLink(nodes[i], nodes[i].neighbours[1]));
                    nodes.Remove(nodes[i]);

                    for (int j = 0; j < nodes.Count; j++)
                    {
                        nodes[j].index = j;
                    }

                    setNeighbours();

                    links.Add(newLink);
                    i = 0;
                }
            }
        }

        private sLink getLink (sNode u, sNode v)
        {
            for(int i = 0; i < links.Count; i++)
            {
                if ((links[i].u == u && links[i].v == v) || (links[i].u == v && links[i].v == u))
                {
                    return links[i];
                }
            }
            return links[0];
        }

        // CREATE VISUAL MODEL OBJECT:

        // initializes a new instance of visual model
        private VisueelModel createModel()
        {
            VisueelModel model = new VisueelModel();

            List<VisueelNode> dNodes = new List<VisueelNode>();
            List<VisualLink> dLinks = new List<VisualLink>();

            for (int i = 0; i < nodes.Count; i++)
            {
                VisueelNode newNode = new VisueelNode(new Point(), nodes[i].name, 0);
                newNode.index = nodes[i].index;
                newNode.point = new Point(nodes[i].x, nodes[i].y);
                newNode.dummynode = !nodes[i].draw;
                newNode.prioriteit = (int)nodes[i].weight;

                if (newNode.dummynode)
                {
                    newNode.Color = Color.Orange;
                }

                dNodes.Add(newNode);
            }

            for (int i = 0; i < links.Count; i++)
            {
                VisualLink newLink = new VisualLink(links[i].u.node_id + "__" + links[i].v.node_id);
                newLink.u = getNode(links[i].u.index, dNodes);
                newLink.v = getNode(links[i].v.index, dNodes);

                dLinks.Add(newLink);
            }

            model.nodes = dNodes;
            model.links = dLinks;

            return model;
        }

        //returns the visual node object with its index variable equal to i
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

        private GRBModel model;
        private GRBEnv env;
        private int M;
        // the minimum length of an edge, the minimum distance between two edges, and the weight used in the objective function
        private double minL = 10.0, minD = 10.0, weightBend = 4.0, weightRpos = 3.0, weightLength = 1.0; 
        // the width and height where the solution is calculated over
        private const int width = 100000, height = 100000;
        private bool usePlanarity = false;

        private GRBLinExpr bendCost = new GRBLinExpr(), rposCost = new GRBLinExpr(), lengthCost = new GRBLinExpr();

        // create all the necessary variables and constraints and add these the model
        // set the objective function that we want to minimize 
        public solver (List<sNode> n, List<sLink> l, List<sLinkPair> p, List<sBendLink> b)
        {
            nodes = n; links = l; linkpairs = p; bendlinks = b;

            M = nodes.Count;

            env = new GRBEnv();
            model = new GRBModel(env);

            create_data();
            create_contraints();

            model.SetObjective(weightBend * bendCost + weightRpos * rposCost + weightLength * lengthCost, GRB.MINIMIZE);
            model.Parameters.MIPGap = 0.075;
        }

        // optimize the model and if necessary remove constraints from the model to make the model feasible
        // update the coordinate values in de nodes list and dispose of the model and environment objects
        public List<sNode> getSolution (int width, int height)
        {
            model.Optimize();
            relax_infeasible_model();

            updateData(width, height);

            model.Dispose();
            env.Dispose();

            return nodes;
        }

        // if the model for the given variables is infeasible, we compute an IIS object, remove one constraint from the model and check if the model is feasible. 
        // we do this untill the model is feasible
        private void relax_infeasible_model()
        {
            if (model.Status == GRB.Status.INFEASIBLE)
            {
                Console.WriteLine("The model is infeasible; computing IIS");
                LinkedList<string> removed = new LinkedList<string>();

                while (true)
                {
                    model.ComputeIIS();
                    foreach (GRBConstr c in model.GetConstrs())
                    {
                        if (c.IISConstr == 1)
                        {
                            Console.WriteLine("Constraint: " + c.ConstrName + " cannot be satisfied and is removed");
                            removed.AddFirst(c.ConstrName);
                            model.Remove(c);
                            break;
                        }
                    }

                    model.Optimize();

                    if (model.Status == GRB.Status.UNBOUNDED)
                    {
                        Console.WriteLine("Model is unbounded and cannot be solved");
                        return;
                    }
                    else if (model.Status == GRB.Status.OPTIMAL)
                    {
                        break;
                    }
                    else if (model.Status != GRB.Status.INF_OR_UNBD && model.Status != GRB.Status.INFEASIBLE)
                    {
                        return;
                    }
                }

                foreach (string s in removed)
                {
                    Console.WriteLine(s + " was removed from the model");
                }
            }
        }

        // first all the variables of the sNode objects in the list nodes are given the relevant value
        // then the variables for the coordinates for each station (and the corresponding constriants) are added to the model
        private void create_data()
        {
            // adding the data to the data source required by the algo:
            for (int i = 0; i < nodes.Count; i++)
            {
                // instantiate string node id
                nodes[i].node_id = "vertex_" + i.ToString();

                // instantiate deg parameter
                nodes[i].deg = nodes[i].neighbours.Count;

                // instantiate z1 and z2 coordinates
                nodes[i].z1 = (nodes[i].x + nodes[i].y);
                nodes[i].z2 = (nodes[i].x - nodes[i].y);

                // SORT THE NEIGHBOURS ARRAY OF EVERY NODE HERE

                nodes[i].neighbours_sorted = nodes[i].neighbours.ToArray();
                Array.Sort(nodes[i].neighbours_sorted, (x, y) => x.getAngle(nodes[i], x).CompareTo(y.getAngle(nodes[i], y)));
            }
            for (int i = 0; i < links.Count; i++)
            {
                // clac sector int values for each edge
                links[i].sec_u_v = calc_sec(links[i].u, links[i].v);
                links[i].sec_v_u = calc_sec(links[i].v, links[i].u);
            }

            // adding the variables for all the coordiantes to the model:
            for (int i = 0; i < nodes.Count; i++)
            {
                GRBVar x = model.AddVar(0.0, width, 0.0, GRB.INTEGER, nodes[i].node_id + "_x");
                GRBVar y = model.AddVar(0.0, height, 0.0, GRB.INTEGER, nodes[i].node_id + "_y");
                GRBVar z1 = model.AddVar(0.0, (width + height), 0.0, GRB.INTEGER, nodes[i].node_id + "_z1");
                GRBVar z2 = model.AddVar(0.0, (width + height), 0.0, GRB.INTEGER, nodes[i].node_id + "_z2");

                model.AddConstr(z1 == x + y, "");
                model.AddConstr(z2 == x - y, "");
            }
            model.Update();
        }

        // if the model is feasible, and there is thus a solution, all of the coordinate variables are pulled from the model
        // are scaled to the desired size and the variables in the nodes list are updated with these values
        private void updateData(int w, int h)
        {
            if (model.Status != GRB.Status.INFEASIBLE)
            {
                List<Point> results = new List<Point>();

                for (int i = 0; i < nodes.Count; i++)
                {
                    //Console.WriteLine("i: " + i + " x: " + model.GetVarByName("vertex_" + i + "_x").X + " y: " + model.GetVarByName("vertex_" + i + "_y").X + " z1: " + model.GetVarByName("vertex_" + i + "_z1").X + " z2: " + model.GetVarByName("vertex_" + i + "_z2").X);
                    results.Add(new Point((int)model.GetVarByName("vertex_" + i + "_x").X, (int)model.GetVarByName("vertex_" + i + "_y").X));
                }

                Point[] scaled_results = coordinates.ScalePointsToSize(results.ToArray(), w, h);

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].x = scaled_results[i].X;
                    nodes[i].y = scaled_results[i].Y;
                }
            }
        }

        // for each of the four lists, the corresponding create variable method is called for every entry in the list
        private void create_contraints()
        {
            for (int i = 0; i < links.Count; i++)
            {
                create_constraints_links(i);
            }
            model.Update();

            for (int i = 0; i < nodes.Count; i++)
            {
                create_constraints_nodes(i);
            }
            if (usePlanarity)
            {
                for (int i = 0; i < linkpairs.Count; i++)
                {
                    create_constraints_linkpairs(i);
                }
            }
            for (int i = 0; i < bendlinks.Count; i++)
            {
                create_constraints_bendlinks(i);
            }
            model.Update();
        }

        // for the node at place i in the list nodes, constraints are created so that the circular order of the neighbours 
        // of a given node is preserved
        private void create_constraints_nodes(int i)
        {
            // --- // 4.3:
            if (nodes[i].deg >= 2)
            {
                GRBLinExpr beta_sum = new GRBLinExpr();
                for (int j = 0; j < nodes[i].deg; j++)
                {
                    GRBVar beta = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "beta");

                    string dir_node = "dir_" + nodes[i].node_id + "_" + nodes[i].neighbours_sorted[j].node_id;
                    string dir_nextnode = "dir_" + nodes[i].node_id + "_" + nodes[i].neighbours_sorted[(j + 1) % nodes[i].deg].node_id;

                    model.AddConstr(model.GetVarByName(dir_node) <= model.GetVarByName(dir_nextnode) - 1 + 8 * beta, "");
                    beta_sum.AddTerm(1.0, beta);
                }
                model.AddConstr(beta_sum == 1, "");
            }
        }

        // for the link at place i in the list links, constraints are created so that direction of every link assumes one of eight possible value
        // also variables and constraints are added to the variables that define the objective function for the length and rpos part
        private void create_constraints_links(int i)
        {
            // --- // 4.2:
            GRBVar alpha_prec = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "alpha_prec");
            GRBVar alpha_orig = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "alpha_orig");
            GRBVar alpha_succ = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "alpha_succ");

            model.AddConstr(alpha_prec + alpha_orig + alpha_succ == 1.0, "");               // constraints need names

            GRBVar dir_u_v = model.AddVar(0.0, 7.0, 0.0, GRB.INTEGER, "dir_" + links[i].u.node_id + "_" + links[i].v.node_id);
            GRBVar dir_v_u = model.AddVar(0.0, 7.0, 0.0, GRB.INTEGER, "dir_" + links[i].v.node_id + "_" + links[i].u.node_id);

            int sec_u_v_prec = (links[i].sec_u_v + 7) % 8, sec_u_v_orig = links[i].sec_u_v, sec_u_v_succ = (links[i].sec_u_v + 1) % 8;
            int sec_v_u_prec = (links[i].sec_v_u + 7) % 8, sec_v_u_orig = links[i].sec_v_u, sec_v_u_succ = (links[i].sec_v_u + 1) % 8;

            model.AddConstr(dir_u_v == alpha_prec * sec_u_v_prec + alpha_orig * sec_u_v_orig + alpha_succ * sec_u_v_succ, "");    // constraints need names
            model.AddConstr(dir_v_u == alpha_prec * sec_v_u_prec + alpha_orig * sec_v_u_orig + alpha_succ * sec_v_u_succ, "");    // constraints need names

            // add coordinate constraints for sec_prec and alpha_prec and Nodes u and v
            create_constraints_coordinates(links[i].u, links[i].v, sec_u_v_prec, alpha_prec);

            // add coordinate constraints for sec_orig and alpha_orig and Nodes u and v
            create_constraints_coordinates(links[i].u, links[i].v, sec_u_v_orig, alpha_orig);

            // add coordinate constraints for sec_succ and alpha_succ and Nodes u and v
            create_constraints_coordinates(links[i].u, links[i].v, sec_u_v_succ, alpha_succ);


            // --- // 4.6:
            GRBVar rpos_u_v = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "rpos_" + links[i].u.node_id + "_" + links[i].v.node_id);
            GRBVar rpos_v_u = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "rpos_" + links[i].v.node_id + "_" + links[i].u.node_id);

            model.AddConstr(-M * rpos_u_v <= dir_u_v - links[i].sec_u_v, "");
            model.AddConstr(M * rpos_u_v >= dir_u_v - links[i].sec_u_v, "");

            rposCost += rpos_u_v;
            rposCost += rpos_v_u;

            // --- // 4.7:
            GRBVar labda = model.AddVar(0.0, GRB.INFINITY, 0.0, GRB.INTEGER, "labda_" + links[i].u.node_id + "_" + links[i].v.node_id);

            model.AddConstr(model.GetVarByName(links[i].u.node_id + "_x") - model.GetVarByName(links[i].v.node_id + "_x") <= labda, "");
            model.AddConstr(-model.GetVarByName(links[i].u.node_id + "_x") + model.GetVarByName(links[i].v.node_id + "_x") <= labda, "");
            model.AddConstr(model.GetVarByName(links[i].u.node_id + "_y") - model.GetVarByName(links[i].v.node_id + "_y") <= labda, "");
            model.AddConstr(-model.GetVarByName(links[i].u.node_id + "_y") + model.GetVarByName(links[i].v.node_id + "_y") <= labda, "");

            lengthCost += labda;
        }

        // for a given node u and v, sector u v, and an alpha variable constraints are added that ensure that the relative position of the nodes
        // (the coordinates) conform to the octilinear layout
        private void create_constraints_coordinates(sNode u, sNode v, int sec_u_v, GRBVar alpha)
        {
            switch (sec_u_v)
            {
                case 0:
                    //  y(u) - y(v) <= 0
                    // -y(u) + y(v) <= 0
                    // -x(u) + x(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_y") - model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_y") + model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_x") + model.GetVarByName(v.node_id + "_x") >= -M * (1 - alpha) + minL, "");     // constraints need names

                    break;
                case 1:
                    //  z2(u) - z2(v) <= 0
                    // -z2(u) + z2(v) <= 0
                    // -z1(u) + z1(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z2") - model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z2") + model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z1") + model.GetVarByName(v.node_id + "_z1") >= -M * (1 - alpha) + 2 * minL, "");   // constraints need names
                    break;
                case 2:
                    //  x(u) - x(v) <= 0
                    // -x(u) + x(v) <= 0
                    // -y(u) + y(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_x") - model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_x") + model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_y") + model.GetVarByName(v.node_id + "_y") >= -M * (1 - alpha) + minL, "");     // constraints need names
                    break;
                case 3:
                    //  z1(u) - z1(v) <= 0
                    // -z1(u) + z1(v) <= 0
                    //  z2(u) - z2(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z1") - model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z1") + model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(model.GetVarByName(u.node_id + "_z2") - model.GetVarByName(v.node_id + "_z2") >= -M * (1 - alpha) + 2 * minL, "");   // constraints need names
                    break;
                case 4:
                    //  y(u) - y(v) <= 0
                    // -y(u) + y(v) <= 0
                    //  x(u) - x(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_y") - model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_y") + model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(model.GetVarByName(u.node_id + "_x") - model.GetVarByName(v.node_id + "_x") >= -M * (1 - alpha) + minL, "");     // constraints need names
                    break;
                case 5:
                    //  z2(u) - z2(v) <= 0
                    // -z2(u) + z2(v) <= 0
                    //  z1(u) - z1(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z2") - model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z2") + model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(model.GetVarByName(u.node_id + "_z1") - model.GetVarByName(v.node_id + "_z1") >= -M * (1 - alpha) + 2 * minL, "");   // constraints need names
                    break;
                case 6:
                    //  x(u) - x(v) <= 0
                    // -x(u) + x(v) <= 0
                    //  y(u) - y(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_x") - model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_x") + model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(model.GetVarByName(u.node_id + "_y") - model.GetVarByName(v.node_id + "_y") >= -M * (1 - alpha) + minL, "");    // constraints need names
                    break;
                case 7:
                    //  z1(u) - z1(v) <= 0
                    // -z1(u) + z1(v) <= 0
                    // -z2(u) + z2(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z1") - model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");            // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z1") + model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");           // constraints need names
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z2") + model.GetVarByName(v.node_id + "_z2") >= -M * (1 - alpha) + 2 * minL, "");    // constraints need names
                    break;

                default:
                    Console.WriteLine("error during sec calculation");
                    break;
            }
        }

        // for the link pair at place i in the list linkpairs, constraints are added so that the two link pairs cant overlap
        // these constraint enforce planarity, and are optional because computing all these extra constriants demand a lot of resources
        private void create_constraints_linkpairs(int i)
        {
            // --- // 4.4:
            GRBVar N = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "N");
            GRBVar S = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "S");
            GRBVar E = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "E");
            GRBVar W = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "W");
            GRBVar NE = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "NE");
            GRBVar NW = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "NW");
            GRBVar SE = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "SE");
            GRBVar SW = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "SW");

            model.AddConstr(N + S + E + W + NE + NW + SE + SW >= 1, "");

            // N  // y(e1) - y(e2) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_y") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_y") <= M * (1 - N) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_y") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_y") <= M * (1 - N) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_y") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_y") <= M * (1 - N) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_y") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_y") <= M * (1 - N) - minD, "");

            // NE // z1(e1) - z1(e2) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_z1") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_z1") <= M * (1 - NE) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_z1") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_z1") <= M * (1 - NE) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_z1") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_z1") <= M * (1 - NE) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_z1") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_z1") <= M * (1 - NE) - minD, "");

            // E  // x(e2) - x(e1) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_x") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_x") <= M * (1 - E) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_x") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_x") <= M * (1 - E) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_x") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_x") <= M * (1 - E) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_x") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_x") <= M * (1 - E) - minD, "");

            // SE // z2(e1) - z2(e2) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_z2") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_z2") <= M * (1 - SE) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_z2") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_z2") <= M * (1 - SE) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_z2") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_z2") <= M * (1 - SE) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_z2") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_z2") <= M * (1 - SE) - minD, "");

            // S  // y(e2) - y(e1) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_y") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_y") <= M * (1 - S) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_y") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_y") <= M * (1 - S) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_y") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_y") <= M * (1 - S) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_y") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_y") <= M * (1 - S) - minD, "");

            // SW // z1(e2) - z1(e1) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_z1") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_z1") <= M * (1 - SW) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_z1") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_z1") <= M * (1 - SW) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_z1") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_z1") <= M * (1 - SW) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_z1") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_z1") <= M * (1 - SW) - minD, "");

            // W  // x(e1) - x(e2) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_x") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_x") <= M * (1 - W) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.u.node_id + "_x") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_x") <= M * (1 - W) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_x") - model.GetVarByName(linkpairs[i].e2.u.node_id + "_x") <= M * (1 - W) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e1.v.node_id + "_x") - model.GetVarByName(linkpairs[i].e2.v.node_id + "_x") <= M * (1 - W) - minD, "");

            // NW // z2(e2) - z2(e1) <= 0
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_z2") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_z2") <= M * (1 - NW) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.u.node_id + "_z2") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_z2") <= M * (1 - NW) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_z2") - model.GetVarByName(linkpairs[i].e1.u.node_id + "_z2") <= M * (1 - NW) - minD, "");
            model.AddConstr(model.GetVarByName(linkpairs[i].e2.v.node_id + "_z2") - model.GetVarByName(linkpairs[i].e1.v.node_id + "_z2") <= M * (1 - NW) - minD, "");
        }

        // variables and constraints are added to the variables that define the objective function for the bend cost part
        private void create_constraints_bendlinks(int i)
        {
            // --- // 4.5:
            string dir_u_v_id = "dir_" + bendlinks[i].u.node_id + "_" + bendlinks[i].v.node_id;
            string dir_v_u_id = "dir_" + bendlinks[i].v.node_id + "_" + bendlinks[i].w.node_id;

            GRBVar dir_u_v = model.GetVarByName(dir_u_v_id);
            GRBVar dir_v_w = model.GetVarByName(dir_v_u_id);
            GRBVar bend_u_v_w = model.AddVar(0.0, 3.0, 0.0, GRB.INTEGER, "bend_" + bendlinks[i].u.node_id + "_" + bendlinks[i].v.node_id + "_" + bendlinks[i].w.node_id);
            GRBVar delta1 = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "");
            GRBVar delta2 = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "");

            model.AddConstr(bend_u_v_w >= (dir_u_v - dir_v_w) - (8 * delta1) + (8 * delta2), "");
            model.AddConstr(-bend_u_v_w <= (dir_u_v - dir_v_w) - (8 * delta1) + (8 * delta2), "");

            bendCost += bend_u_v_w;
        }

        // given two nodes, one of eight directions is calculated
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

        // given two nodes, the angle between line segemnt defined by (u.x, u.y) (v.x, v.y) and the x-axes is calculated
        // in a way useful to the calc sec method
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
        public string node_id, name;
        public bool draw = true;
        public double weight = 0.0;

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
            if (points.Length > 0)
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
            }
            return points;
        }
    }
}
