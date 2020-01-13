using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
namespace manderijntje
{
    public class DataControl
    {
        DataModel dataModel;
        private const string filepath = "C:/Way2Go/datamodel_binary.txt";


        //different files if you want to load different datasets

        //string sFile = "C:/Way2Go/groningen test2.xml";
        // string sFile = "C:/Way2Go/enkhuizen test 4.xml";
        //string sFile = "C:/Way2Go/amsterdam test tram subway train.xml";
        //string sFile = "C:/Way2Go/train germany.xml";
        string sFile = "C:/Way2Go/train netherlands.xml";
        //string sFile = "C:/Way2Go/groenhart train.xml";
        //string sFile = "C:/Way2Go/amsterdam tram bus.xml";
        //string sFile = "C:/Way2Go/subway london.xml";
        //string sFile = "C:/Way2Go/berlin subway.xml";
        //string sFile = "C:/Way2Go/train frankrijk.xml";
        //string sFile = "C:/Way2Go/train uk.xml";
        // string sFile = "C:/Way2Go/frankrijk test3.xml";
        //string sFile = "C:/Way2Go/train europa.xml";
        //string sFile = "C:/Way2Go/train westeuropa.xml";
        //string sFile = "C:/Way2Go/subway europa.xml";

        //Creating the DataControl and DataModel dataset.
        public DataControl()
        {
            if (File.Exists(filepath) && !Program.reimport)
            {
                try
                {
                    ReadDataFromDisk();
                }
                catch
                {
                    MakeDataForDisk();
                }
            }
            else
            {
                MakeDataForDisk();
            }
        }
        private void MakeDataForDisk()
        {
            Loadnodes(sFile);//points with coordinates and stationnames
            Loadroutes(sFile);//points with routes
            LoadWay(sFile);//points with ways
            dataModel = new DataModel();
            waysinorder();//sets ways in right order
            combinepoints();//combine data
            foreach (Node node in dataModel.unique_nodes)
            {
                if (node.neighbours.Count > 8)
                {
                    CheckDegree(node);
                }
            }

            if (Directory.Exists(filepath))
            {
                WriteDataToDisk(FileMode.Open);
            }
            else
            {
                WriteDataToDisk(FileMode.Create);
            }
        }


        //Reading the serialized dataModel from the disk
        private void ReadDataFromDisk()
        {
            try
            {
                using (Stream str = File.Open(filepath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    dataModel = (DataModel)bformatter.Deserialize(str);
                }
            }
            catch
            {
                MessageBox.Show("File coudn't be opened", "Error", MessageBoxButtons.OK);
            }
        }
        //Writing the serialized dataModel to the disk
        private void WriteDataToDisk(FileMode fm)
        {
            try
            {
                using (Stream str = File.Open(filepath, fm))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(str, dataModel);
                }
            }
            catch
            {
                MessageBox.Show("File coudn't be opened", "Error", MessageBoxButtons.OK);
            }
        }

        //checks the amount of neighbours to ensure there are no more then 8 neighbours so the parsing class can handle it.
        private void CheckDegree(Node n)
        {
            Node[] array = n.neighbours.ToArray();
            Array.Sort(array, (x, y) => n.DistanceToNode(x).CompareTo(n.DistanceToNode(y)));
            Array.Reverse(array);
            int toCheck = (n.neighbours.Count - 8);
            Console.WriteLine("check: " + toCheck);
            for (int i = 0; i < toCheck; i++)
            {
                n.neighbours.Remove(array[0]);
                array[0].neighbours.Remove(n);
                dataModel.unique_links.Remove(getLink(n, array[i]));
            }
        }
        //tries to find a specific link in unique_links.Count
        private Link getLink(Node u, Node v)
        {
            for (int i = 0; i < dataModel.unique_links.Count; i++)
            {
                if ((dataModel.unique_links[i].Start.stationnaam == u.stationnaam && dataModel.unique_links[i].End.stationnaam == v.stationnaam) || (dataModel.unique_links[i].Start.stationnaam == v.stationnaam && dataModel.unique_links[i].End.stationnaam == u.stationnaam))
                {
                    Console.WriteLine("found");
                    return dataModel.unique_links[i];
                }
            }
            return dataModel.unique_links[0];
        }
        //returns the dataModel
        public DataModel GetDataModel()
        {
            return dataModel;
        }

        //this method is needed to get data out of a xml file
        private XDocument GetGpxDoc(string sFile)
        {
            XDocument gpxDoc = XDocument.Load(sFile);
            return gpxDoc;
        }
        /*
         there are 4 twodimensional arrays of data,
         points1 with the nodeID and the coordinates and stationname,
         points2 with nodeID and routename where all the trains realy stop,
         points3 with wayID and routeID where the trains pass by,
         and points4 with wayID and the NodeID that belong to that way.
         the wayID of the Route is sometimes not in the right order so in waysinorder that is done in the right order,
         in combinepoints we combine all the data from all the groups.
         
        */
        string[,] points1;//met de NodeIDs and stationnamen and coordinates
        string[,] points2; // where they stop
        string[,] points3; // where they pass by
        string[,] points4; //which nodes belong to which ways
        string[] pointsstop; // stop yes or no
        string[] points1ID;//all nodeID with names
        string[] id; //all WayIDs
        List<string> routids = new List<string>();// all routIDs
        string[][] jag; //jagged array of all NodeIDs where index is the same as id(points4)
        string[][] jag2; // jagged array of all wayIDs where index is the same as routids(points3)
        int lengthjag2; // length of jag2
        
        public void Loadnodes(string sFile)
        {//load data of all nodes
            XDocument gpxDoc = GetGpxDoc(sFile);
            //gets coordinates and names from file
            var waypoints = from waypoint in gpxDoc.Descendants("node")
                            select new
                            {
                                Latitude = waypoint.Attribute("lat").Value,
                                Longitude = waypoint.Attribute("lon").Value,
                                ID = waypoint.Attribute("id") != null ?
                                waypoint.Attribute("id").Value : null,
                                Segs = (
                                from waypoints2 in waypoint.Descendants("tag")
                                select new
                                {
                                    k = waypoints2.Attribute("k").Value,
                                    v = waypoints2.Attribute("v").Value,
                                }
                              )
                            };
            int a = 0;
            foreach (var wpt in waypoints)
            {
                foreach (var wptSeg in wpt.Segs)
                {
                    if (wptSeg.k == "name")
                    {
                        a++;
                    }
                }
            }
            int n = 0;//counter
            points1 = new string[a, 4];
            foreach (var wpt in waypoints)
            {
                foreach (var wptSeg in wpt.Segs)
                {
                    if (wptSeg.k == "name")
                    {
                        points1[n, 0] = wpt.ID.PadLeft(10, '9');//NodeID
                        points1[n, 1] = getCoordinateFormatting(wpt.Longitude); //longitude with the correct formatting for the local settings
                        points1[n, 2] = getCoordinateFormatting(wpt.Latitude); //latitude with the correct formatting for the local settings
                        points1[n, 3] = wptSeg.v;//Name
                        n++;
                    }
                }
            }
            Sort(points1, 0, "ASC");
            points1ID = GetColumn(points1, 0);
        }
        // this function makes sure that whatever the formatting settings are for the decimal separator, the parsing class always gets the correct double value from the string
        private string getCoordinateFormatting(string c)
        {
            string dot_format = c;
            string[] comma_format_array = c.Split('.');
            string comma_format = comma_format_array[0] + "," + comma_format_array[1];
            double dot_value = double.Parse(dot_format);
            double comma_value = double.Parse(comma_format);
            if (dot_value > 1000.0 || dot_value < -1000.0)
            {
                return comma_format;
            }
            else if (comma_value > 1000.0 || comma_value < -1000.0)
            {
                return dot_format;
            }
            else
            {
                return c;
            }
        }
        public string[] GetColumn(string[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }
        private static void Sort<T>(T[,] array, int sortCol, string order)
        {
            int colCount = array.GetLength(1), rowCount = array.GetLength(0);
            if (sortCol >= colCount || sortCol < 0)
                throw new System.ArgumentOutOfRangeException("sortCol", "The column to sort on must be contained within the array bounds.");
            DataTable dt = new DataTable();
            // Name the columns with the second dimension index values, e.g., "0", "1", etc.
            for (int col = 0; col < colCount; col++)
            {
                DataColumn dc = new DataColumn(col.ToString(), typeof(T));
                dt.Columns.Add(dc);
            }
            // Load data into the data table:
            for (int rowindex = 0; rowindex < rowCount; rowindex++)
            {
                DataRow rowData = dt.NewRow();
                for (int col = 0; col < colCount; col++)
                    rowData[col] = array[rowindex, col];
                dt.Rows.Add(rowData);
            }
            // Sort by using the column index = name + an optional order:
            DataRow[] rows = dt.Select("", sortCol.ToString() + " " + order);
            for (int row = 0; row <= rows.GetUpperBound(0); row++)
            {
                DataRow dr = rows[row];
                for (int col = 0; col < colCount; col++)
                {
                    array[row, col] = (T)dr[col];
                }
            }
            dt.Dispose();
        }
        //this is a long method because it is hard to it make smaller.
        public void Loadroutes(string sFile)
        { //loads all routes
            
            XDocument gpxDoc = GetGpxDoc(sFile);
            var tracks = from track in gpxDoc.Descendants("relation")
                         select new
                         {
                             ID = track.Attribute("id") != null ?
                                track.Attribute("id").Value : null,
                             Segs = (
                                from trackpoint in track.Descendants("member")
                                select new
                                {
                                    type = trackpoint.Attribute("type").Value,//way or node
                                    refr = trackpoint.Attribute("ref").Value,//wayID or nodeID
                                    role = trackpoint.Attribute("role").Value,//stop, platform or ""
                                }
                              ),
                             Segs2 = (
                                from trackpoint in track.Descendants("tag")
                                select new
                                {
                                    k = trackpoint.Attribute("k").Value,
                                    v = trackpoint.Attribute("v").Value,
                                    //more information about each route
                                }
                              )
                         };
            // to determine the right number for points2
            int l = 0;//counter
            int k = 0;//counter
            int h = 0;//counter
            //I make lists of properties of each route. than i check of those properties already exist to make sure 
            // there are no double routes(each going in an other direchtion)
            //the routeID and  routename are diffrent in these double routes, so i cant check those.
            List<string> P = new List<string>();
            List<string> Q = new List<string>();
            List<string> R = new List<string>();
            List<string> S = new List<string>();
            List<string> T = new List<string>();
            List<string> U = new List<string>();
            foreach (var trk in tracks)
            {
                string refr = "ref";//a number of a letter mostly
                string net = "net";//networtk
                string op = "op";//operator
                string from = "des";//from
                string to = "des";//to
                string pt2 = "pt2";//public_transport
                string w = "w";//wikidata
                string type = "type";//type
                foreach (var trkSeg2 in trk.Segs2)
                {
                    if (trkSeg2.k == "ref")
                    {
                        refr = trkSeg2.v;
                    }
                    if (trkSeg2.k == "network")
                    {
                        net = trkSeg2.v;
                    }
                    if (trkSeg2.k == "operator")
                    {
                        op = trkSeg2.v;
                    }
                    if (trkSeg2.k == "to")
                    {
                        to = trkSeg2.v;
                    }
                    if (trkSeg2.k == "from")
                    {
                        from = trkSeg2.v;
                    }
                    if (trkSeg2.k == "wikidata")
                    {
                        w = trkSeg2.v;
                    }
                    if (trkSeg2.k == "public_transport")
                    {
                        pt2 = trkSeg2.v;
                    }
                    if (trkSeg2.k == "type")
                    {
                        type = trkSeg2.v;
                    }
                }
                if (pt2 != "stop_area" && pt2 != "platform" && type == "route")//only routes
                {
                    P.Add(refr);
                    Q.Add(net);
                    R.Add(op);
                    S.Add(from);
                    T.Add(to);
                    U.Add(w);
                    if (!nodoubleroutes(P, Q, R, U, S, T,
             refr, net, op, w, to, from))//checks if there ar no double routes
                    {
                        foreach (var trkSeg in trk.Segs)
                        {
                            if (trkSeg.type == "node")
                            {
                                l++;//counts nodes
                            }
                            if (trkSeg.type == "way")
                            {
                                if (trkSeg.role == "")
                                {
                                    k++;//counts ways
                                }
                            }
                        }
                    }
                }
                h++;//counts routes
            }

            //exacly the same as before
            List<string> N = new List<string>();
            List<string> M = new List<string>();
            List<string> O = new List<string>();
            List<string> K = new List<string>();
            List<string> L = new List<string>();
            List<string> J = new List<string>();
            points2 = new string[l, 3];//stops
            points3 = new string[k, 6];//rides past
            int n = 0;//counter
            int m = 0;//counter
            int o = 0;//counter
            jag2 = new string[h][];//wayids
            int j = 0;
            foreach (var trk in tracks)
            {
                int g = 0; int i = 0;
                string refr = "ref";//refnumber or letter
                string net = "net";//network
                string op = "op";//opertor
                string to = "des";//to
                string from = "des";//from
                string pt = "pt";//public_transport
                string w = "w";//wikidata
                string type = "type";//type (route)
                foreach (var trkSeg2 in trk.Segs2)
                {
                    if (trkSeg2.k == "ref")
                    {
                        refr = trkSeg2.v;
                    }
                    if (trkSeg2.k == "network")
                    {
                        net = trkSeg2.v;
                    }
                    if (trkSeg2.k == "operator")
                    {
                        op = trkSeg2.v;
                    }
                    if (trkSeg2.k == "to")
                    {
                        to = trkSeg2.v;
                    }
                    if (trkSeg2.k == "from")
                    {
                        from = trkSeg2.v;
                    }
                    if (trkSeg2.k == "wikidata")
                    {
                        w = trkSeg2.v;
                    }
                    if (trkSeg2.k == "public_transport")
                    {
                        pt = trkSeg2.v;
                    }
                    if (trkSeg2.k == "type")
                    {
                        type = trkSeg2.v;
                    }
                }
                if (pt != "stop_area" && pt != "platform" && type == "route")
                {
                    //adds properties to lists
                    N.Add(refr);
                    M.Add(net);
                    O.Add(op);
                    K.Add(from);
                    L.Add(to);
                    J.Add(w);
                    if (!nodoubleroutes(N, M, O, J, K, L,
                        refr, net, op, w, to, from))//checks if there ar no double routes
                        //by checking if these properties dont come for twice(sometimes not all properies are given,
                        //so checking all is best.
                    {
                        foreach (var trkSeg in trk.Segs)
                        {
                            if (trkSeg.type == "way")
                            {
                                if (trkSeg.role == "")
                                {
                                    i++;//counts
                                }
                            }
                        }
                        if (i > 0)
                        {
                            jag2[j] = new string[i];//makes a jagged array with right numbers
                        }
                        foreach (var trkSeg in trk.Segs)
                        {
                            if (trkSeg.type == "node")//stops
                            {
                                points2[n, 0] = trkSeg.refr.PadLeft(10, '9');//nodeID
                                points2[n, 1] = "route" + " " + m;//routename
                                foreach (var trkSeg2 in trk.Segs2)
                                {
                                    if (trkSeg2.k == "name")
                                    {
                                        points2[n, 1] = trkSeg2.v;//routename
                                    }
                                }
                                n++;
                            }
                            if (trkSeg.type == "way")//rides past
                            {
                                if (trkSeg.role == "")
                                {
                                    points3[o, 0] = trkSeg.refr.PadLeft(10, '9');//nodeID
                                    points3[o, 1] = trk.ID;//wayID
                                    points3[o, 2] = "route" + " " + m;//routename
                                    points3[o, 3] = "-";//sort route, for example: long_distance
                                    points3[o, 4] = "ref";//mostly numbers or letters
                                    points3[o, 5] = "vehicle";//type vehicle
                                    foreach (var trkSeg2 in trk.Segs2)
                                    {
                                        if (trkSeg2.k == "name")
                                        {
                                            points3[o, 2] = trkSeg2.v;
                                        }
                                        if (trkSeg2.k == "route")
                                        {
                                            points3[o, 5] = trkSeg2.v;
                                        }
                                        if (trkSeg2.k == "service")
                                        {
                                            points3[o, 3] = trkSeg2.v;
                                        }
                                        if (trkSeg2.k == "ref")
                                        {
                                            points3[o, 4] = trkSeg2.v;
                                        }
                                    }
                                    o++;
                                    jag2[j][g] = trkSeg.refr.PadLeft(10, '9');//all ways in a array for each route
                                    g++;
                                    lengthjag2++;//length of jag 2 to use later
                                }
                            }
                        }
                    }
                    routids.Add(trk.ID);//list of all routID
                    j++;
                }
                m++;
            }
            routids.RemoveAll(item => item == null);
            jag2 = jag2.Where(x => x != null).ToArray();
        }

        //Checks for double routes and ensure there are no doubles anymore
        public bool nodoubleroutes(List<string> P, List<string> Q, List<string> R, List<string> U, List<string> S, List<string> T,
            string refr, string net, string op, string w, string to, string from)
        {
            for (int e = 0; e < Q.Count - 1; e++)
            {
                if (P[e] == refr && Q[e] == net && R[e] == op && U[e] == w)
                {
                    if (S[e] == to && T[e] == from)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //loads ways out of a file
        public void LoadWay(string sFile)
        {
            XDocument gpxDoc = GetGpxDoc(sFile);
            var waypoints = from waypoint in gpxDoc.Descendants("way")
                            select new
                            {
                                ID = waypoint.Attribute("id") != null ?
                                waypoint.Attribute("id").Value : null,
                                Segs = (
                                from waypoints2 in waypoint.Descendants("nd")
                                select new
                                {
                                    refr = waypoints2.Attribute("ref").Value,
                                }
                              )
                            };
            int k = 0;
            int h = 0;
            foreach (var wpt in waypoints)
            {
                foreach (var wptSeg in wpt.Segs)
                {
                    k++;
                }
                h++;
            }
            int o = 0;
            points4 = new string[k, 2];
            id = new string[h];
            jag = new string[h][];
            int j = 0;
            foreach (var wpt in waypoints)
            {
                int i = 0; int g = 0;
                id[j] = wpt.ID.PadLeft(10, '9');
                foreach (var wptSeg in wpt.Segs)
                {
                    points4[o, 0] = wpt.ID.PadLeft(10, '9');//wayid
                    points4[o, 1] = wptSeg.refr.PadLeft(10, '9');//nodeid
                    o++;
                    i++;
                }
                if (i > 0)
                {
                    jag[j] = new string[i];
                }
                foreach (var wptSeg in wpt.Segs)
                {
                    jag[j][g] = wptSeg.refr.PadLeft(10, '9');
                    g++;
                }
                j++;
            }
            Sort(points4, 0, "ASC");
        }
        string[,] pointsc;
        int[,] inorder;
        public void waysinorder()
        {//sets ways in order based (mostly) on the first and last of each node in the way
            pointsc = new string[lengthjag2, 2];
            int q = 0; int z = 0;
            foreach (string[] array in jag2)
            {
                List<int> pointsv = new List<int>();
                inorder = new int[array.Length, 3];
                string[] arrayid = new string[array.Length];
                for (int e = 0; e < array.Length; e++)
                {
                    int t = Array.IndexOf(id, array[e]);
                    if (t >= 0)
                    {
                        arrayid[e] = t.ToString();
                        //makes a array of all the indexes of the wayids that are in this specific route.
                    }
                }
                for (int e = 0; e < arrayid.Length; e++)
                {
                    string first = ""; string last = "";
                    int a = 0;
                    int t = int.Parse(arrayid[e]);
                    first = jag[t][0];
                    last = jag[t][jag[t].Length - 1];
                    //finds the first and last nodeID
                    inorder[e, 1] = t + 1;
                    a = t;
                    //test the first and last node against first and last, last en first and rest of other nodeids
                    testfirstandlast(false, arrayid, e, a, first, last, true);
                    testfirstandlast(false, arrayid, e, a, first, last, false);
                    testfirstandlast(true, arrayid, e, a, first, last, false);
                }
                insertpointsv(pointsv);//insert inorder in the right order in pointsv
                string[] pointsl = new string[pointsv.Count()];
                for (int e = 0; e < pointsv.Count(); e++)
                {
                    if (pointsv[e] != 0)
                    {
                        pointsl[e] = id[pointsv[e] - 1];//gets the index from pointsv
                    }
                }
                int l = 0;
                for (int t = 0; t < pointsl.Length; t++)
                {
                    int k = Array.IndexOf(id, pointsl[t]);
                    if (k >= 0)
                    {
                        pointsc[q, 0] = id[k];//wayid
                        pointsc[q, 1] = routids[z];//routeid
                        q++;
                        l++;
                    }
                }
                z++;
            }
        }

        //returns the nodes in a route in order
        public void testfirstandlast(bool contains, string[] arrayid, int e, int a, string first, string last, bool turn)
        {
            if (inorder[e, 0] == 0 || inorder[e, 2] == 0)
            {
                for (int d = 0; d < arrayid.Length; d++)
                {
                    int i = int.Parse(arrayid[d]);
                    if (i >= 0 && a != i)
                    {
                        if (!contains)
                        {
                            if (turn)
                                if (first == jag[i][0])
                                {
                                    inorder[e, 2] = i + 1;
                                }
                            if (last == jag[i][jag[i].Length - 1])
                            {
                                inorder[e, 0] = i + 1;
                            }
                            if (!turn)
                            {
                                if (inorder[e, 2] == 0)
                                {
                                    if (first == jag[i][0])
                                    {
                                        inorder[e, 2] = i + 1;
                                    }
                                }
                                if (inorder[e, 0] == 0)
                                {
                                    if (last == jag[i][jag[i].Length - 1])
                                    {
                                        inorder[e, 0] = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (inorder[e, 2] == 0)
                            {
                                if (jag[i].Contains(first))
                                {
                                    inorder[e, 2] = i + 1;
                                }
                            }
                            if (inorder[e, 0] == 0)
                            {
                                if (jag[i].Contains(last))
                                {
                                    inorder[e, 0] = i + 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void insertpointsv(List<int> pointsv)
        { //insert inorder in the right order in pointsv
            //first the first numbers 
            if (inorder[0, 0] != 0)
            {
                pointsv.Add(inorder[0, 0]);
            }
            pointsv.Add(inorder[0, 1]);
            if (inorder[0, 2] != 0) 
            {
                pointsv.Add(inorder[0, 2]);
            }
            
            if (inorder.Length / 3 > 1)
            {
                if (inorder[0, 2] == 0 && inorder[0, 0] == 0)
                {
                    if (inorder[1, 0] != 0)
                    {
                        pointsv.Add(inorder[1, 0]);
                    }
                    pointsv.Add(inorder[1, 1]);
                    if (inorder[1, 2] != 0)
                    {
                        pointsv.Add(inorder[1, 2]);
                    }
                }
            }
            //then the rest of the numbers
            for (int t = 0; pointsv.Count() != (inorder.Length / 3) && t < (inorder.Length / 3) + 10; t++)
            {
                int j = 0;
                for (int e = 1; e < inorder.Length / 3; e++)
                {
                    if (pointsv[0] == inorder[e, 1] && pointsv[1] == inorder[e, 2] && (inorder[e, 0] != 0 || j != e))
                    {
                        int index = pointsv.IndexOf(inorder[e, 1]);
                        pointsv.Insert(index, inorder[e, 0]);
                        j = e;
                    }
                    if (pointsv[0] == inorder[e, 1] && pointsv[1] == inorder[e, 0] && (inorder[e, 2] != 0 || j != e))
                    {
                        int index = pointsv.IndexOf(inorder[e, 1]);
                        pointsv.Insert(index, inorder[e, 2]);
                        j = e;
                    }
                    if (pointsv[pointsv.Count() - 2] == inorder[e, 0] && pointsv[pointsv.Count() - 1] == inorder[e, 1] && (inorder[e, 2] != 0 || j != e))
                    {
                        int index = pointsv.IndexOf(inorder[e, 1]);
                        pointsv.Insert(index + 1, inorder[e, 2]);
                        j = e;
                    }
                    if (pointsv[pointsv.Count() - 2] == inorder[e, 2] && pointsv[pointsv.Count() - 1] == inorder[e, 1] && (inorder[e, 0] != 0 || j != e))
                    {
                        int index = pointsv.IndexOf(inorder[e, 1]);
                        pointsv.Insert(index + 1, inorder[e, 0]);
                        j = e;
                    }
                }
            }
        }

        //comines the points to make the datamodel
        public void combinepoints()
        {
            //checks if all nodes in four have a name(by checking one)
            four_and_one_to_y();
            //runs past c(from waysinorder) in the right order, gets nodeid and stationname from y 
            //and gets routedata from tree
            c_and_y_and_tree_to_f();
            //gets coordinates from one and makes sure stations with same name that are close to each other have same NodeIDs
            f_and_one_to_five();
            //checks for double entrys in list
            five_to_seven();

            //makes nodes for routing, based on one and two
            noderouting();
            //makes links for routing, based on two
            linkrouting();
            //makes nodes with correct information, including if it stops(two) at that station or not
            seven_to_stop_and_node();
            //makes links
            links();
            dataModel.get_unique_nodes();//checks for unique nodes
            dataModel.get_unique_links();// checks for unique links
        }
        string[,] pointsy;
        //checks if all nodes in four have a name(by checking one)

        public void four_and_one_to_y()
        {
            string[,] pointso = new string[points4.Length / 2, 3];
            int x = 0;
            for (int j = 0; j < points4.Length / 2; j++)
            {
                int t = Array.BinarySearch(points1ID, points4[j, 1]);
                if (t >= 0)
                {
                    pointso[x, 0] = points4[j, 0];//wayid
                    pointso[x, 1] = points4[j, 1];//nodeid
                    pointso[x, 2] = points1[t, 3];//stationname
                    x++;
                }
            }
            pointsy = clear_array_nulls(pointso);
        }
        string[,] pointsw;
        //runs past c(from waysinorder) in the right order, gets nodeid and stationname from y 
        //and gets routedata from tree
        public void c_and_y_and_tree_to_f()
        {
            string[] points4ID = GetColumn(pointsy, 0);
            string[,] pointsd = clear_array_nulls(pointsc);
            int f = 0;
            string[,] pointsf = new string[pointsd.Length / 2, 6];
            for (int j = 0; j < pointsd.Length / 2; j++)
            {
                int t = Array.BinarySearch(points4ID, pointsd[j, 0]);
                if (t >= 0)
                {
                    pointsf[f, 0] = pointsy[t, 1];//nodeid
                    pointsf[f, 5] = pointsy[t, 2];//stationname
                    for (int d = 0; d < points3.Length / 6; d++)
                    {
                        if (points3[d, 0] == pointsd[j, 0] && points3[d, 1] == pointsd[j, 1])
                        {
                            pointsf[f, 1] = points3[d, 2];//routename
                            pointsf[f, 2] = points3[d, 3];//kind of route
                            pointsf[f, 3] = points3[d, 4];//ref of routename
                            pointsf[f, 4] = points3[d, 5];//sort vehicle
                            break;
                        }
                    }
                    f++;
                }
            }
            pointsw = clear_array_nulls(pointsf);
        }
        string[,] points6;
        //gets coordinates from one and makes sure stations with same name that are close to each other have same NodeIDs

        public void f_and_one_to_five()
        {
            string[,] points5 = new string[pointsw.Length / 6, 8];
            int r = 0;
            for (int c = 0; c < pointsw.Length / 6; c++)
            {
                int t = Array.BinarySearch(points1ID, pointsw[c, 0]);
                if (t >= 0)
                {
                    points5[r, 0] = pointsw[c, 1];//routename
                    points5[r, 1] = points1[t, 3];//stationname
                    points5[r, 2] = points1[t, 1];//x
                    points5[r, 3] = points1[t, 2];//y
                    points5[r, 4] = points1[t, 0];//nodeid
                    points5[r, 5] = pointsw[c, 2];//sort route
                    points5[r, 6] = pointsw[c, 3];//ref routename
                    points5[r, 7] = pointsw[c, 4];//sort vehicle
                    r++;
                }
                if (r - 1 >= 0)
                    if (points5[r - 1, 4] != null)
                    {
                        for (int y = 0; y < points5.Length / 8; y++)
                        {
                            if (points5[r - 1, 1] == points5[y, 1] && points5[y, 1] != null)
                            {
                                if (distance(double.Parse(points5[r - 1, 2]), double.Parse(points5[r - 1, 3]), double.Parse(points5[y, 2]), double.Parse(points5[y, 3])) < 0.05)
                                {
                                    points5[r - 1, 4] = points5[y, 4];
                                    points5[r - 1, 2] = points5[y, 2];
                                    points5[r - 1, 3] = points5[y, 3];
                                    //makes sure all 
                                    break;
                                }
                            }
                        }
                    }
            }
            points6 = clear_array_nulls(points5);
        }
        string[,] points8;
        //checks for double entrys in list

        public void five_to_seven()
        {
            string[,] points7 = new string[points6.Length / 8, 8];
            bool dupFound;
            int h = 0;
            //removes double enries from list
            for (int i = 0; i < points6.Length / 8; i++)
            {
                dupFound = false;
                for (int a = i + 1; a < i + 5 && a < points6.Length / 8; a++)
                {
                    if ((i != a) && points6[a, 0] == points6[i, 0] && points6[a, 1] == points6[i, 1])
                    {
                        dupFound = true;
                        break;
                    }
                }
                if (!dupFound)
                {
                    points7[h, 0] = points6[i, 0];
                    points7[h, 1] = points6[i, 1];
                    points7[h, 2] = points6[i, 2];
                    points7[h, 3] = points6[i, 3];
                    points7[h, 4] = points6[i, 4];
                    points7[h, 5] = points6[i, 5];
                    points7[h, 6] = points6[i, 6];
                    points7[h, 7] = points6[i, 7];
                    h++;
                }
            }
            points8 = clear_array_nulls(points7);
        }
        //makes nodes for routing, based on one and two

        public void noderouting()
        {
            for (int j = 0; j < points2.Length / 3; j++)
            {
                int t = Array.BinarySearch(points1ID, points2[j, 0]);
                if (t >= 0)
                {
                    points2[j, 2] = points1[t, 3];
                    bool dubbel = false;
                    foreach (Node node in dataModel.nodesrouting)
                    {
                        if (node.stationnaam == points1[t, 3])
                            dubbel = true;
                    }
                    if (!dubbel)
                    {
                        dataModel.AddNoderouting(new Node(points1[t, 0], double.Parse(points1[t, 1]), double.Parse(points1[t, 2]), "", points1[t, 3], "0", "0", "0", true, 0));
                    }
                }
            }
        }           
        //makes links for routing, based on two

        public void linkrouting()
        {
            for (int j = 0; j < points2.GetLength(0); j++)
            {
                if (j + 1 < points2.GetLength(0))
                {
                    if (points2[j, 1] == points2[j + 1, 1])
                    {
                        Node station1node = dataModel.GetNodeName(points2[j, 2], dataModel.GetNodesRouting());
                        Node station2node = dataModel.GetNodeName(points2[j + 1, 2], dataModel.GetNodesRouting());
                        if (!(station1node == null || station2node == null))
                        {
                            Link link = new Link(station1node, station2node, points2[j, 1]);
                            dataModel.AddLinkrouting(link);
                            station1node.addneighbour(station2node);
                            station1node.addLink(new Link(station1node, station2node, points2[j, 1]));
                            station2node.addneighbour(station1node);
                            station2node.addLink(new Link(station2node, station1node, points2[j, 1]));
                        }
                    }
                }
            }
        }
        //makes nodes with correct information, including if it stops(two) at that station or not

        public void seven_to_stop_and_node()
        {
            int g = 0;
            pointsstop = new string[points8.Length / 8];
            bool dupFound;
            for (int i = 0; i < points8.Length / 8; i++)
            {
                dupFound = false;
                pointsstop[g] = "true";
                for (int a = 0; a < points2.Length / 3; a++)
                {
                    if (points8[i, 0] == points2[a, 1] && points8[i, 1] == points2[a, 2])
                    {
                        dupFound = true;
                        break;
                    }
                }
                if (!dupFound)
                {
                    pointsstop[g] = "false";
                }
                double x1 = double.Parse(points8[g, 2]);
                double y1 = double.Parse(points8[g, 3]);
                bool stop = bool.Parse(pointsstop[g]);
                dataModel.AddNode(new Node(points8[g, 4], x1, y1, points8[g, 0], points8[g, 1], points8[g, 5], points8[g, 6], points8[g, 7], stop, 0));
                g++;
            }
        }
        //makes links
        public void links()
        {
            for (int i = 0; i < points8.Length / 8; i++)
            {
                if (i + 1 < points8.Length / 8)
                {
                    if (points8[i, 0] == points8[i + 1, 0])
                    {
                        Node station1node = dataModel.GetNode(points8[i, 4], dataModel.GetNodes());
                        Node station2node = dataModel.GetNode(points8[i + 1, 4], dataModel.GetNodes());
                        Link link = new Link(station1node, station2node, points8[i, 0]);
                        dataModel.AddLink(link);
                        station1node.addneighbour(station2node);
                        station2node.addneighbour(station1node);
                    }
                }
            }
        }
        public static double distance(double x1, double y1, double x2, double y2) 
        {
            // Calculating distance 
            return Math.Sqrt(Math.Pow(x2 - x1, 2) +
                          Math.Pow(y2 - y1, 2) * 1.0);
        }
        public string[,] clear_array_nulls(string[,] input)
        {
            int m = input.GetUpperBound(0) + 1;
            int n = input.GetUpperBound(1) + 1;
            string[] temp = new string[input.GetUpperBound(0) + 1];
            for (int x = 0; x < m; x++)
                temp[x] = input[x, 0];
            temp = temp.Where(s => !object.Equals(s, null)).ToArray();
            string[,] output = new string[temp.Length, n];
            Array.Copy(input, output, temp.Length * n);
            return output;
        }
    }
    [Serializable]
    public class DataModel
    {
        public List<Node> nodes;
        public List<Node> nodesrouting;
        public List<Link> links;
        public List<Link> linksrouting;
        public List<Node> unique_nodes;
        public List<Link> unique_links;
        public DataModel()
        {
            nodes = new List<Node>();
            nodesrouting = new List<Node>();
            links = new List<Link>();
            linksrouting = new List<Link>();
            unique_nodes = new List<Node>();
            unique_links = new List<Link>();
        }
        // populate unique lists:
        public void get_unique_nodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (!node_in_unique_nodes(nodes[i]))
                {
                    unique_nodes.Add(nodes[i]);
                }
            }
        }
        public void get_unique_links()
        {
            for (int i = 0; i < links.Count; i++)
            {
                if (!link_in_unique_links(links[i]))
                {
                    unique_links.Add(links[i]);
                }
            }
        }
        private bool node_in_unique_nodes(Node n)
        {
            for (int i = 0; i < unique_nodes.Count; i++)
            {
                if (unique_nodes[i].name_id == n.name_id)
                {
                    return true;
                }
            }
            return false;
        }
        private bool link_in_unique_links(Link n)
        {
            for (int i = 0; i < unique_links.Count; i++)
            {
                if (unique_links[i].Start == n.Start && unique_links[i].End == n.End || unique_links[i].Start == n.End && unique_links[i].End == n.Start)
                {
                    return true;
                }
            }
            return false;
        }
        // end populate
        public void AddNode(Node n)
        {
            nodes.Add(n);
        }
        public void AddNoderouting(Node n)
        {
            nodesrouting.Add(n);
        }
        public List<Node> GetNodes()
        {
            return nodes;
        }
        public List<Node> GetNodesRouting()
        {
            return nodesrouting;
        }
        public Node GetNode(string name, List<Node> lijst)
        {
            foreach (Node node in lijst)
            {
                if (name == node.name_id)
                    return node;
            }
            return null;
        }
        public Node GetNodeName(string name, List<Node> lijst)
        {
            foreach (Node node in lijst)
            {
                if (name == node.stationnaam)
                    return node;
            }
            return null;
        }
        public void AddLink(Link l)
        {
            links.Add(l);
        }
        public void AddLinkrouting(Link l)
        {
            linksrouting.Add(l);
        }
        public List<Link> GetLinks()
        {
            return links;
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
        // unieke indentifier, naam in de vorm van een string
        public string name_id, routnaam, stationnaam, soortrout, routid, vehicle;
        public bool stops;
        public Node NearestToStart;
        public int MinCostToStart = int.MaxValue;
        public bool Visited = false;
        public Node(string name, double coordx, double coordy, string routenaam, string stationsnaam, string soortroute, string routeid, string vehicles, bool stop, int i)
        {
            number = i;
            name_id = name;
            x = coordx;
            y = coordy;
            routnaam = routenaam;
            stationnaam = stationsnaam;  
            soortrout = soortroute;
            routid = routeid;
            vehicle = vehicles; 
            stops = stop;
            neighbours = new List<Node>();
            Connections = new List<Link>();
        }
        public void addneighbour(Node neighbour)
        {
            bool neighbourtest = true;
            foreach (Node naaste in neighbours)
            {
                if (naaste == neighbour)
                    neighbourtest = false;
            }
            if (neighbourtest)
                neighbours.Add(neighbour);
        }
        public void addLink(Link link)
        {
            Connections.Add(link);
        }
        public double DistanceToNode(Node u)
        {
            return DataControl.distance(this.x, this.y, u.x, u.y);
        }
    }
    [Serializable]
    public class Link
    {
        // twee pointers die wijzen naar de twee nodes die deze link verbind
        public Node Start, End;
        public string RouteName;
        public int Weight = 1;//the weight will be variable based on time and place but not for now
        public Link(Node startpunt, Node eindpunt, string RouteNames) 
        {
            Start = startpunt;
            End = eindpunt;
            RouteName = RouteNames;
        }
    }
}
