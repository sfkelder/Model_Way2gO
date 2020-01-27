using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Gurobi;
 
namespace Manderijntje
{
    // creates all the necessary variables and object for the Solver class
    // and creates an instance of Visual Model with all the relevant values
    class Parsing
    {
        private List<Snode> nodes = new List<Snode>();                      // list of all the nodes that make up the graph, including so called dummy nodes that will not be shown but are required by the solve algo
        private List<Slink> links = new List<Slink>();                      // list of all the links that make up the graph
        private List<Slinkpair> linkpairs = new List<Slinkpair>();          // list of all link pairs where the two links making up the pair do not share a common node
        private List<Sbendlink> bendlinks = new List<Sbendlink>();          // list of all bend links. a bend link consists of three node that discribe two links that share one common node

        private List<Slink> logicallinks = new List<Slink>();               // list of all logical links. a logical links discribes the link from one node to another but that links has been devided by a dummy node
        private List<Slogical> logicalconnections = new List<Slogical>();   // list of all logical connections. a logical connection discribes a full path from one real node to another real node separated by one or more dummy nodes

        private const int width = 5000, height = 5000;                      // the width and height of the plain of which the graph is projected

        // given an instance of Data Model and if there are nodes and links in said Data Model
        // first the lists of nodes and links are populated and all nodes and links are given the relevant properties
        // then dummy stations are added to the graph where necessary and the neighbours properties reevaluated
        // last the list of bendlinks and linkpairs are populated
        public Parsing(DataModel model)
        {
            if (model.nodes.Count != 0 && model.links.Count != 0)
            {
                SetNodes(model.nodes);
                SetLinks(model.links);
                SetNeighbours();

                EnforcePlanarity();
                CreateLogicalConnections();
                SetNeighbours();

                GetLinkPairs();
                GetBendLinks();
            }
        }

        // if solve is true, indicating that a octalinear garph is desired, and the degree of input graph (given via the contructor) is no greater than 8
        // a new instance of the Solver class is created, and the list of node (actuale only the coordinates of the nodes are different) is updated
        // an instance of Visual Model is created and returned
        public VisualModel GetModel (bool solve)
        {
            if (solve && GetDegree() <= 8)
            {
                nodes = (new Solver(nodes, links, linkpairs, bendlinks)).GetSolution(width, height);
            }

            return CreateModel();
        }


        // PLANARITY:

        // returns the degree of the graph that is modeled by the list of nodes and the list of links
        private int GetDegree()
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
        private void EnforcePlanarity()
        {
            for (int i = 0; i < links.Count; i++)
            {
                for (int n = i; n < links.Count; n++)
                {
                    if (AreLinksNonIncident(links[i], links[n]) && DoLinksIntersect(links[i], links[n]))
                    {
                        InsertNode(links[i], links[n]);
                    }
                }
            }
        }

        // adds a node at the intersection of links e1 and e2
        private void InsertNode(Slink e1, Slink e2)
        {
            Point location = GetIntersection(e1.u.x, e1.u.y, e1.v.x, e1.v.y, e2.u.x, e2.u.y, e2.v.x, e2.v.y);
            Snode node = new Snode(nodes.Count, location);
            node.neighbours = new List<Snode> { e1.u, e1.v, e2.u, e2.v };
            node.draw = false;

            links.Remove(e1);
            links.Remove(e2);

            e1.isLogical = true; e1.addedNode = node; logicallinks.Add(e1);
            e2.isLogical = true; e2.addedNode = node; logicallinks.Add(e2);

            links.Add(new Slink(node, e1.u));
            links.Add(new Slink(node, e1.v));
            links.Add(new Slink(node, e2.u));
            links.Add(new Slink(node, e2.v));

            nodes.Add(node);
        }

        // creates all logical connection object from all logical links in the list
        private void CreateLogicalConnections()
        {
            for (int i = 0; i < logicallinks.Count; i++)
            {
                if (logicallinks[i].u.draw && logicallinks[i].v.draw)
                {
                    logicalconnections.Add(new Slogical(logicallinks[i].u, logicallinks[i].v, logicallinks[i].addedNode));

                } else
                {
                    for (int n = 0; n < logicalconnections.Count; n++)
                    {
                        if (logicalconnections[n].isSubRoute(logicallinks[i].u, logicallinks[i].v))
                        {
                            logicalconnections[n].nodes.Add(logicallinks[i].addedNode);
                            break;
                        }
                    }
                }
            }
        }

        // checks if the given links intersects
        private bool DoLinksIntersect(Slink e1, Slink e2)
        {
            Point intersect = GetIntersection(e1.u.x, e1.u.y, e1.v.x, e1.v.y, e2.u.x, e2.u.y, e2.v.x, e2.v.y);
            if (IsPointInBounds(e1, e2, intersect))
            {
                return true;
            }
            return false;
        }

        // calculates the points of intersection of two lines, defined by line ((x1, y1) (x2, y2)) and line ((x3, y3) (x4, y4))
        private Point GetIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
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
        private bool IsPointInBounds(Slink e1, Slink e2, Point p)
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
        private void GetLinkPairs()
        {
            for (int i = 0; i < links.Count; i++)
            {
                for (int n = i; n < links.Count; n++)
                {
                    if (AreLinksNonIncident(links[i], links[n]))
                    {
                        Slinkpair newPair = new Slinkpair(links[i], links[n]);
                        linkpairs.Add(newPair);
                    }
                }
            }
        }

        // create and addes an instance of the bend link object to the list bend links
        // for each pair of adjacent links in the list links
        private void GetBendLinks()
        {
            for (int i = 0; i < links.Count; i++)
            {
                for (int n = 0; n < links.Count; n++)
                {
                    if (AreLinksAdjacent(links[i], links[n]))
                    {
                        bendlinks.Add(GetBendLink(links[i], links[n]));
                    }
                }
            }
        }

        // creates a bend link object such that (u, v) is a link in the list links
        // and that (v, w) is a link in the list links
        private Sbendlink GetBendLink(Slink e1, Slink e2)
        {
            Sbendlink result;
            if (e1.v == e2.v)
            {
                result = new Sbendlink(e1.u, e1.v, e2.u);

            }
            else if (e1.v == e2.u)
            {
                result = new Sbendlink(e1.u, e1.v, e2.v);

            }
            else if (e1.u == e2.v)
            {
                result = new Sbendlink(e1.v, e1.u, e2.u);

            }
            else   // e1.u == e2.u
            {
                result = new Sbendlink(e1.v, e1.u, e2.v);

            }
            return result;
        }

        // checks if links e1 and e2 are non incident
        // that is they don't share a common node
        private bool AreLinksNonIncident(Slink e1, Slink e2)
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
        private bool AreLinksAdjacent(Slink e1, Slink e2)
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
        private void SetNodes(List<Node> dNodes)
        {
            for (int i = 0; i < dNodes.Count; i++)
            {
                dNodes[i].number = i;
            }

            List<Point> Coordinates = new List<Point>();
            for (int i = 0; i < dNodes.Count; i++)
            {
                // first argument is Lat, and the second argument is Long:
                Coordinates.Add(Manderijntje.Coordinates.GetLogicalCoordinate(dNodes[i].x, dNodes[i].y, 1000000, 1000000));
            }
            Point[] ScaledCoordinates = Manderijntje.Coordinates.ScalePointsToSize(Coordinates.ToArray(), width, height);
            for (int i = 0; i < dNodes.Count; i++)
            {
                Snode newNode = new Snode(dNodes[i].number, ScaledCoordinates[i]);
                newNode.name = dNodes[i].stationName;
                newNode.country = dNodes[i].country;
                nodes.Add(newNode);
            }
        }

        // populates the neighbours list for every node in the list nodes
        // using the list links
        private void SetNeighbours()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].neighbours = GetNeighbours(nodes[i]);
                nodes[i].deg = nodes[i].neighbours.Count;
                nodes[i].weight = nodes[i].neighbours.Count;
            }
        }

        // returns a list of all neighbours for a given node
        private List<Snode> GetNeighbours(Snode node)
        {
            List<Snode> result = new List<Snode>();

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
        private void SetLinks(List<Link> dLinks)
        {
            for (int i = 0; i < dLinks.Count; i++)
            {
                Slink newLink = new Slink(GetNode(dLinks[i].start.number), GetNode(dLinks[i].end.number));
                links.Add(newLink);
            }
        }

        // returns the sNode object with its index variable equal to i
        private Snode GetNode(int index)
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


        // CREATE VISUAL MODEL OBJECT:

        // initializes a new instance of visual model
        private VisualModel CreateModel()
        {
            VisualModel model = new VisualModel();

            List<VisueelNode> dNodes = new List<VisueelNode>();
            List<VisualLink> dLinks = new List<VisualLink>();
            List<VLogicalLink> dConnections = new List<VLogicalLink>();

            for (int i = 0; i < nodes.Count; i++)
            {
                VisueelNode newNode = new VisueelNode(new Point(), nodes[i].name, 0);
                newNode.index = nodes[i].index;
                newNode.point = new Point(nodes[i].x, nodes[i].y);
                newNode.dummynode = !nodes[i].draw;
                newNode.prioriteit = (int)nodes[i].weight;
                newNode.country = nodes[i].country;

                if (newNode.dummynode)
                {
                    newNode.Color = Color.Orange;
                }

                dNodes.Add(newNode);
            }

            for (int i = 0; i < links.Count; i++)
            {
                VisualLink newLink = new VisualLink(links[i].u.node_id + "__" + links[i].v.node_id);
                newLink.u = GetNode(links[i].u.index, dNodes);
                newLink.v = GetNode(links[i].v.index, dNodes);

                dLinks.Add(newLink);
            }

            for (int i = 0; i < logicalconnections.Count; i++)
            {
                VLogicalLink newLogical = new VLogicalLink(GetNode(logicalconnections[i].u.index, dNodes), GetNode(logicalconnections[i].v.index, dNodes));
                for (int n = 0; n < logicalconnections[i].nodes.Count; n++)
                {
                    newLogical.nodes.Add(GetNode(logicalconnections[i].nodes[n].index, dNodes));
                }
                newLogical.GetLinks(dLinks);
                dConnections.Add(newLogical);
            }

            model.nodes = dNodes;
            model.links = dLinks;
            model.connections = dConnections;

            return model;
        }

        //returns the visual node object with its index variable equal to i
        private VisueelNode GetNode(int i, List<VisueelNode> dNodes)
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


    // takes (essentially) a graph as input and create a new graph
    // where all the nodes are in an octolinear layout
    class Solver
    {
        private List<Snode> nodes;          // the exact same list of nodes as in the Parsing class
        private List<Slink> links;          // the exact same list of links as in the Parsing class
        private List<Slinkpair> linkpairs;  // the exact same list of link pairs as in the Parsing class
        private List<Sbendlink> bendlinks;  // the exact same list of bend links as in the Parsing class

        private GRBModel model;             // the model that discribes the mathematical model (of the graph) we wish to solve 
        private GRBEnv env;                 // the environment in which the solver solves the model
        private int M;                      // a large constant used in many model calculations
        private double minL = 10.0, minD = 10.0, weightBend = 1.0, weightRpos = 5.0, weightLength = 1.0;            // the minimum length of an edge, the minimum distance between two edges, and the weight used in the objective function 
        private const int width = 100000, height = 100000;                                                          // the size of the plain on which the solver tries to find a solution. it is so large to unsure it find an optimal solution regardless of the size of the input graph
        private bool usePlanarity = false;                                                                          // indicates whether or not planarity ensuring constraints should be added to the model 

        private GRBLinExpr bendCost = new GRBLinExpr(), rposCost = new GRBLinExpr(), lengthCost = new GRBLinExpr(); // these expressions hold the sum of all the relavent variables for the objective function

        // create all the necessary variables and constraints and add these the model
        // set the objective function that we want to minimize 
        public Solver (List<Snode> n, List<Slink> l, List<Slinkpair> p, List<Sbendlink> b)
        {
            nodes = n; links = l; linkpairs = p; bendlinks = b;

            M = nodes.Count;

            env = new GRBEnv();
            model = new GRBModel(env);

            CreateData();
            CreateContraints();

            model.SetObjective(weightBend * bendCost + weightRpos * rposCost + weightLength * lengthCost, GRB.MINIMIZE);
            model.Parameters.MIPGap = 0.075;
        }

        // optimize the model and if necessary remove constraints from the model to make the model feasible
        // update the coordinate values in de nodes list and dispose of the model and environment objects
        public List<Snode> GetSolution (int width, int height)
        {
            model.Optimize();
            RelaxInfeasibleModel();
            //RelaxInfeasibleConstraints();

            UpdateData(width, height);

            model.Dispose();
            env.Dispose();

            return nodes;
        }


        // INFEABILE MODEL MANAGEMENT

        // if the model for the given variables is infeasible, we compute an IIS object, remove one constraint from the model and check if the model is feasible. 
        // we do this until the model is feasible
        private void RelaxInfeasibleModel()
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

        // if the model is infeasible, all constraints in the model are given a on/off switch. we then solve the model again but add a extra objective
        // minimize the amount of constraints that have to be turned off
        private void RelaxInfeasibleConstraints ()
        {
            Console.WriteLine("The model is infeasible; relaxing the constraints");
            int originNumVars = model.NumVars;
            model.FeasRelax(0, false, false, true);
            model.Optimize();
    
            if (model.Status == GRB.Status.INF_OR_UNBD || model.Status == GRB.Status.INFEASIBLE || model.Status == GRB.Status.UNBOUNDED)
            {
                Console.WriteLine("The relaxed model cannot be solved because it is infeasible or unbounded");
            }

            Console.WriteLine("Slack values: ");
            GRBVar[] vars = model.GetVars();
            for (int i = originNumVars; i < model.NumVars; i++)
            {
                GRBVar sv = vars[i];
                if (sv.X > 1e-6)
                {
                    Console.WriteLine(sv.VarName + " = " + sv.X);
                }
            }
        }


        // GET AND SET MODEL VARIABLES

        // first all the variables of the sNode objects in the list nodes are given the relevant value
        // then the variables for the coordinates for each station (and the corresponding constriants) are added to the model
        private void CreateData()
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
                links[i].sec_u_v = CalcSec(links[i].u, links[i].v);
                links[i].sec_v_u = CalcSec(links[i].v, links[i].u);
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
        private void UpdateData(int w, int h)
        {
            if (model.Status != GRB.Status.INFEASIBLE)
            {
                List<Point> results = new List<Point>();

                for (int i = 0; i < nodes.Count; i++)
                {
                    results.Add(new Point((int)model.GetVarByName("vertex_" + i + "_x").X, (int)model.GetVarByName("vertex_" + i + "_y").X));
                }

                Point[] scaled_results = Coordinates.ScalePointsToSize(results.ToArray(), w, h);

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].x = scaled_results[i].X;
                    nodes[i].y = scaled_results[i].Y;
                }
            }
        }


        // CREATION OF ALL CONSTRAINTS AND RELEVANT VARIABLES

        // for each of the four lists, the corresponding create variable method is called for every entry in the list
        private void CreateContraints()
        {
            for (int i = 0; i < links.Count; i++)
            {
                CreateConstraintsLinks(i);
            }
            model.Update();

            for (int i = 0; i < nodes.Count; i++)
            {
                CreateConstraintsNodes(i);
            }
            if (usePlanarity)
            {
                for (int i = 0; i < linkpairs.Count; i++)
                {
                    CreateConstraintsLinkpairs(i);
                }
            }
            for (int i = 0; i < bendlinks.Count; i++)
            {
                CreateConstraintsBendlinks(i);
            }
            model.Update();
        }

        // for the node at place i in the list nodes, constraints are created so that the circular order of the neighbours 
        // of a given node is preserved
        private void CreateConstraintsNodes(int i)
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
        private void CreateConstraintsLinks(int i)
        {
            // --- // 4.2:
            GRBVar alpha_prec = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "alpha_prec");
            GRBVar alpha_orig = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "alpha_orig");
            GRBVar alpha_succ = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "alpha_succ");

            model.AddConstr(alpha_prec + alpha_orig + alpha_succ == 1.0, "");              

            GRBVar dir_u_v = model.AddVar(0.0, 7.0, 0.0, GRB.INTEGER, "dir_" + links[i].u.node_id + "_" + links[i].v.node_id);
            GRBVar dir_v_u = model.AddVar(0.0, 7.0, 0.0, GRB.INTEGER, "dir_" + links[i].v.node_id + "_" + links[i].u.node_id);

            int sec_u_v_prec = (links[i].sec_u_v + 7) % 8, sec_u_v_orig = links[i].sec_u_v, sec_u_v_succ = (links[i].sec_u_v + 1) % 8;
            int sec_v_u_prec = (links[i].sec_v_u + 7) % 8, sec_v_u_orig = links[i].sec_v_u, sec_v_u_succ = (links[i].sec_v_u + 1) % 8;

            model.AddConstr(dir_u_v == alpha_prec * sec_u_v_prec + alpha_orig * sec_u_v_orig + alpha_succ * sec_u_v_succ, "");   
            model.AddConstr(dir_v_u == alpha_prec * sec_v_u_prec + alpha_orig * sec_v_u_orig + alpha_succ * sec_v_u_succ, "");  

            // add coordinate constraints for sec_prec and alpha_prec and Nodes u and v
            CreateConstraintsCoordinates(links[i].u, links[i].v, sec_u_v_prec, alpha_prec);

            // add coordinate constraints for sec_orig and alpha_orig and Nodes u and v
            CreateConstraintsCoordinates(links[i].u, links[i].v, sec_u_v_orig, alpha_orig);

            // add coordinate constraints for sec_succ and alpha_succ and Nodes u and v
            CreateConstraintsCoordinates(links[i].u, links[i].v, sec_u_v_succ, alpha_succ);


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
        private void CreateConstraintsCoordinates(Snode u, Snode v, int sec_u_v, GRBVar alpha)
        {
            switch (sec_u_v)
            {
                case 0:
                    //  y(u) - y(v) <= 0
                    // -y(u) + y(v) <= 0
                    // -x(u) + x(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_y") - model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");            
                    model.AddConstr(-model.GetVarByName(u.node_id + "_y") + model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");            
                    model.AddConstr(-model.GetVarByName(u.node_id + "_x") + model.GetVarByName(v.node_id + "_x") >= -M * (1 - alpha) + minL, "");    

                    break;
                case 1:
                    //  z2(u) - z2(v) <= 0
                    // -z2(u) + z2(v) <= 0
                    // -z1(u) + z1(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z2") - model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");           
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z2") + model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");            
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z1") + model.GetVarByName(v.node_id + "_z1") >= -M * (1 - alpha) + 2 * minL, "");   
                    break;
                case 2:
                    //  x(u) - x(v) <= 0
                    // -x(u) + x(v) <= 0
                    // -y(u) + y(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_x") - model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");          
                    model.AddConstr(-model.GetVarByName(u.node_id + "_x") + model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");         
                    model.AddConstr(-model.GetVarByName(u.node_id + "_y") + model.GetVarByName(v.node_id + "_y") >= -M * (1 - alpha) + minL, "");    
                    break;
                case 3:
                    //  z1(u) - z1(v) <= 0
                    // -z1(u) + z1(v) <= 0
                    //  z2(u) - z2(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z1") - model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");         
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z1") + model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");         
                    model.AddConstr(model.GetVarByName(u.node_id + "_z2") - model.GetVarByName(v.node_id + "_z2") >= -M * (1 - alpha) + 2 * minL, "");  
                    break;
                case 4:
                    //  y(u) - y(v) <= 0
                    // -y(u) + y(v) <= 0
                    //  x(u) - x(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_y") - model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");        
                    model.AddConstr(-model.GetVarByName(u.node_id + "_y") + model.GetVarByName(v.node_id + "_y") <= M * (1 - alpha), "");         
                    model.AddConstr(model.GetVarByName(u.node_id + "_x") - model.GetVarByName(v.node_id + "_x") >= -M * (1 - alpha) + minL, "");     
                    break;
                case 5:
                    //  z2(u) - z2(v) <= 0
                    // -z2(u) + z2(v) <= 0
                    //  z1(u) - z1(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z2") - model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");        
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z2") + model.GetVarByName(v.node_id + "_z2") <= M * (1 - alpha), "");          
                    model.AddConstr(model.GetVarByName(u.node_id + "_z1") - model.GetVarByName(v.node_id + "_z1") >= -M * (1 - alpha) + 2 * minL, "");  
                    break;
                case 6:
                    //  x(u) - x(v) <= 0
                    // -x(u) + x(v) <= 0
                    //  y(u) - y(v) >= minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_x") - model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");           
                    model.AddConstr(-model.GetVarByName(u.node_id + "_x") + model.GetVarByName(v.node_id + "_x") <= M * (1 - alpha), "");         
                    model.AddConstr(model.GetVarByName(u.node_id + "_y") - model.GetVarByName(v.node_id + "_y") >= -M * (1 - alpha) + minL, "");    
                    break;
                case 7:
                    //  z1(u) - z1(v) <= 0
                    // -z1(u) + z1(v) <= 0
                    // -z2(u) + z2(v) >= 2*minL

                    model.AddConstr(model.GetVarByName(u.node_id + "_z1") - model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");           
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z1") + model.GetVarByName(v.node_id + "_z1") <= M * (1 - alpha), "");         
                    model.AddConstr(-model.GetVarByName(u.node_id + "_z2") + model.GetVarByName(v.node_id + "_z2") >= -M * (1 - alpha) + 2 * minL, "");  
                    break;

                default:
                    Console.WriteLine("error during sec calculation");
                    break;
            }
        }

        // for the link pair at place i in the list linkpairs, constraints are added so that the two link pairs cant overlap
        // these constraint enforce planarity, and are optional because computing all these extra constriants demand a lot of resources
        private void CreateConstraintsLinkpairs(int i)
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
        private void CreateConstraintsBendlinks(int i)
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


        // SECTOR CALCULATIONS

        // given two nodes, one of eight directions is calculated
        private int CalcSec(Snode u, Snode v)
        {
            double angle = CalcAngle(u, v);

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
        public static double CalcAngle(Snode u, Snode v)
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
    

    // discribes any node in a graph
    public class Snode
    {
        public int x, y, z1, z2, deg, index;                // z1 and z2 coordinates are extra coordinates used only by the solver.
        public List<Snode> neighbours = new List<Snode>();
        public Snode[] neighbours_sorted;                   // array where all a nodes neighbours are sorted in such a way that is required by the algo
        public string node_id, name, country;
        public bool draw = true;                            // indicated whether or not this this should be drawn by the MapView (e.g. if a node is a dummy node, draw = false)
        public double weight = 0.0;                         // indicates how important a node is, required by Visual Model

        public Snode(int i, Point p)
        {
            index = i;
            x = p.X;
            y = p.Y;
        }
        
        // return the angle between the two given nodes
        public double getAngle(Snode u, Snode v)
        {
            return Solver.CalcAngle(u, v);
        }
    }


    // discribes a link between two nodes u and v in a graph
    public class Slink
    {
        public Snode u, v;
        public int sec_u_v, sec_v_u;                        // sector between this node u and v and the sector between v and u

        public Snode addedNode;                             // reference to a dummy node that was added on this link. making this link a logical link
        public bool isLogical = false;                      // indicates whether or not a link is a logical link

        public Slink(Snode U, Snode V)
        {
            u = U;
            v = V;
        }
    }


    // discribes a logical connection between 'real' nodes u and v
    // the 'real' nodes are connected via the nodes and links in the two arrays
    public class Slogical
    {
        public Snode u, v;
        public List<Snode> nodes = new List<Snode>();
        public List<Slink> links = new List<Slink>();

        public Slogical (Snode U, Snode V, Snode x)
        {
            u = U;
            v = V;
            nodes.Add(x);
        }

        // adds a link made of nodes u and v to the logical connection
        public bool isSubRoute(Snode U, Snode V)
        {
            List<Snode> booltest = nodes;
            booltest.Add(u);
            booltest.Add(v);
            if (booltest.Contains(U) && booltest.Contains(V))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }


    // discribes a link pair consisting of two Slinks. where the two links making up the pair do not share a common node
    public class Slinkpair
    {
        public Slink e1, e2;

        public Slinkpair(Slink E1, Slink E2)
        {
            e1 = E1;
            e2 = E2;
        }
    }


    // discribes a bend link consisting of three Snodes, that discribe two links that share one common node
    public class Sbendlink
    {
        public Snode u, v, w;

        public Sbendlink(Snode U, Snode V, Snode W)
        {
            u = U;
            v = V;
            w = W;
        }
    }


    // transforms (lat,long) coordinates to (x,y) coordinates
    // scales point to a specific size
    static class Coordinates
    {
        // given a (lat,long) coordinate and a width and height
        // the relevant (x,y) coordiante is returned
        public static Point GetLogicalCoordinate(double Lat, double Long, int width, int height)
        {
            return new Point(CalcX(Long, width), CalcY(Lat, width, height));
        }

        // calculates the relevant x coordinate based on the width and a long coordinate
        private static int CalcX(double Long, int width)
        {
            double result = (Long + 180.0) * (width / 360.0);

            return (int)result;
        }

        // calculates the relevant y coordinate based on the width, height and a lat coordinate
        private static int CalcY(double Lat, int width, int height)
        {
            double latRad = (Lat * (Math.PI / 180.0));
            double mercN = Math.Log(Math.Tan((Math.PI / 4.0) + (latRad / 2.0)));
            int result = (int)((height / 2.0) - ((width * mercN) / (2.0 * Math.PI)));

            return result;
        }

        // scales the given array of Points to the desired output plain of size (width, height)
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
