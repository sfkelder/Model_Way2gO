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
        public DataControl()
        {
            if (File.Exists(filepath) && !Program.reimport)
            {

                ReadDataFromDisk();
            }
            else
            {
                Loadnodes(sFile);//punten met stationnamen
                Loadroutes(sFile);//punten met routes
                LoadWay(sFile);
                dataModel = new DataModel();
                waysinorder();
                combinepoints();//samen naar een tweedimensionale array

                foreach (Node node in dataModel.unique_nodes)
                {
                    if (node.Buren.Count > 8)
                    {
                        CheckDegree(node);
                    }

                }
                //dataModel.get_unique_links();
                int deg = 0;
                for (int i = 0; i < dataModel.unique_nodes.Count; i++)
                {
                    if (dataModel.unique_nodes[i].Buren.Count > deg)
                    {
                        deg = dataModel.unique_nodes[i].Buren.Count;
                    }
                }

                Console.WriteLine("real: " + deg);

                if (Directory.Exists(filepath))
                {
                    WriteDataToDisk(FileMode.Open);
                }
                else
                {
                    WriteDataToDisk(FileMode.Create);
                }

            }
        }

        DataModel dataModel;
        private const string filepath = "C:/Way2Go/datamodel_binary.txt";

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

        private void CheckDegree(Node n)
        {
            Node[] neighours = n.Buren.ToArray();
            Array.Sort(neighours, (x, y) => n.DistanceToNode(x).CompareTo(n.DistanceToNode(y)));
            Array.Reverse(neighours);

            int toCheck = (n.Buren.Count - 8);
            Console.WriteLine("check: " + toCheck);
            for (int i = 0; i < toCheck; i++)
            {
                //n.Buren.Remove(neighours[0]);
                //neighours[0].Buren.Remove(n);
                dataModel.unique_links.Remove(getLink(n, neighours[i]));
                Console.WriteLine("removed: ");
            }





        }

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



        public DataModel GetDataModel()
        {
            return dataModel;
        }
        //deze twee methodes zijn nodig om de data uit de file te halen
        private XDocument GetGpxDoc(string sFile)
        {
            XDocument gpxDoc = XDocument.Load(sFile);
            return gpxDoc;
        }
        /*
         je krijgt 4 datagroepen van de xml file,
         punten1 met de nodeID en coordinaten en stationnaam,
         punten2 met de nodeID en routenaam waar de trein ook echt stopt,
         punten3 met de wayID en routeID(dus alle ways waar de trein langskomt),
         en punten4 met de wayID en de NodeID die bij die way horen.
        de wayID van de routeID van punten3 staan soms in de verkeerde volgorde, 
        daarom moet je naar de eerste en de laatste(soms ook andersom) nodeID van elke wayID kijken 
        of die de dezelfde is als de volgende wayID om ze op volgorde te zetten.
        daarna moet je de data van alle datagroepen samenvoegen.
        */
        string[,] punten1;//met de stationnamen
        string[,] punten2; // met de routenamen
        string[,] punten3;
        string[,] punten4;
        string[,] pointsdone; // klaar

        int lengthjag2;
        string[] punten1ID;

        List<string> routes;
        string[] id; List<string> routids;
        string[][] jag; string[][] jag2;
        public void Loadnodes(string sFile)
        {
            XDocument gpxDoc = GetGpxDoc(sFile);
            //krijgt punten en namen uit file
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

            //label1.Text = lamin.ToString();
            int n = 0;//teller
            punten1 = new string[a, 4];//eerste puntenstring
            double[] ID = new double[a];
            foreach (var wpt in waypoints)
            {
                foreach (var wptSeg in wpt.Segs)
                {
                    if (wptSeg.k == "name")
                    {
                        //string[] latitude = wpt.Latitude.Split('.');
                        //string latitude2 = latitude[0] + "," + latitude[1];
                        //string[] longitude = wpt.Longitude.Split('.');
                        //string longitude2 = longitude[0] + "," + longitude[1];
                        //weer . eraf , erbij
                        punten1[n, 0] = wpt.ID.PadLeft(10, '9');
                        punten1[n, 1] = getCoordinateFormatting(wpt.Longitude); //longitude with the correct formatting for the local settings
                        punten1[n, 2] = getCoordinateFormatting(wpt.Latitude); //latitude with the correct formatting for the local settings
                        ID[n] = double.Parse(wpt.ID);
                        punten1[n, 3] = wptSeg.v;
                        n++;
                    }
                }
            }
            Sort(punten1, 0, "ASC");
            punten1ID = GetColumn(punten1, 0);
        }

        // this function makes sure that whatever the formatting settings are for the decimal separator, the parsing class always gets the correct double value from the string
        private string getCoordinateFormatting (string c)
        {
            string dot_format = c;

            string[] comma_format_array = c.Split('.');
            string comma_format = comma_format_array[0] + "," + comma_format_array[1];

            double dot_value = double.Parse(dot_format);
            double comma_value = double.Parse(comma_format);

            if (dot_value > 1000.0 || dot_value < -1000.0)
            {
                return comma_format;
            } else if (comma_value > 1000.0 || comma_value < -1000.0)
            {
                return dot_format;
            } else
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
        public void Loadroutes(string sFile)
        {
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
                                    type = trackpoint.Attribute("type").Value,
                                    refr = trackpoint.Attribute("ref").Value,
                                    role = trackpoint.Attribute("role").Value,
                                }
                              ),
                             Segs2 = (
                                from trackpoint in track.Descendants("tag")
                                select new
                                {
                                    k = trackpoint.Attribute("k").Value,
                                    v = trackpoint.Attribute("v").Value,

                                }
                              )
                         };
            // om voor punten2 juiste aantal te bepalen
            int l = 0;//teller
            int k = 0;//teller
            int h = 0;
            List<string> P = new List<string>();
            List<string> Q = new List<string>();
            List<string> R = new List<string>();
            List<string> S = new List<string>();
            List<string> T = new List<string>();
            List<string> U = new List<string>();
            foreach (var trk in tracks)
            {

                string refr = "ref";
                string net = "net";
                string op = "op";
                string from = "des";
                string to = "des";
                string pt2 = "pt2";
                string w = "w";
                string type = "type";
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
                if (pt2 != "stop_area" && pt2 != "platform" && type == "route")
                {
                    P.Add(refr);
                    Q.Add(net);
                    R.Add(op);
                    S.Add(from);
                    T.Add(to);
                    U.Add(w);

                    if (!nodoubleroutes(P, Q, R, U, S, T,
             refr, net, op, w, to, from))
                    {
                        foreach (var trkSeg in trk.Segs)
                        {
                            if (trkSeg.type == "node")
                            {
                                l++;
                            }
                            if (trkSeg.type == "way")
                            {
                                if (trkSeg.role == "")
                                {
                                    k++;
                                }
                            }
                        }
                    }
                }
                h++;
            }
            List<string> N = new List<string>();
            List<string> M = new List<string>();
            List<string> O = new List<string>();
            List<string> K = new List<string>();
            List<string> L = new List<string>();
            List<string> J = new List<string>();
            punten2 = new string[l, 3];
            punten3 = new string[k, 6];
            int n = 0;//teller
            int m = 0;//teller
            int o = 0;//teller
            string[] routid = new string[k];
            jag2 = new string[h][];
            int j = 0;
            foreach (var trk in tracks)
            {
                int g = 0; int i = 0;
                string refr = "ref";
                string net = "net";
                string op = "op";
                string to = "des";
                string from = "des";
                string pt = "pt";
                string w = "w";
                string type = "type";
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
                    N.Add(refr);
                    M.Add(net);
                    O.Add(op);
                    K.Add(from);
                    L.Add(to);
                    J.Add(w);

                    if (!nodoubleroutes(N, M, O, J, K, L,
             refr, net, op, w, to, from))
                    {
                        foreach (var trkSeg in trk.Segs)
                        {
                            if (trkSeg.type == "way")
                            {
                                if (trkSeg.role == "")
                                {
                                    i++;
                                }
                            }
                        }
                        if (i > 0)
                        {
                            jag2[j] = new string[i];
                        }
                        foreach (var trkSeg in trk.Segs)
                        {
                            if (trkSeg.type == "node")
                            {
                                punten2[n, 0] = trkSeg.refr.PadLeft(10, '9');
                                punten2[n, 1] = "route" + " " + m;
                                foreach (var trkSeg2 in trk.Segs2)
                                {
                                    if (trkSeg2.k == "name")
                                    {
                                        punten2[n, 1] = trkSeg2.v;
                                    }
                                }
                                n++;
                            }
                            if (trkSeg.type == "way")
                            {
                                if (trkSeg.role == "")
                                {
                                    punten3[o, 0] = trkSeg.refr.PadLeft(10, '9');
                                    punten3[o, 1] = trk.ID;
                                    punten3[o, 2] = "route" + " " + m;
                                    routid[j] = trk.ID;
                                    punten3[o, 3] = "-";
                                    punten3[o, 4] = "ref";
                                    punten3[o, 5] = "vehicle";
                                    foreach (var trkSeg2 in trk.Segs2)
                                    {
                                        if (trkSeg2.k == "name")
                                        {
                                            punten3[o, 2] = trkSeg2.v;
                                        }
                                        if (trkSeg2.k == "route")
                                        {
                                            punten3[o, 5] = trkSeg2.v;
                                        }
                                        if (trkSeg2.k == "service")
                                        {
                                            punten3[o, 3] = trkSeg2.v;
                                        }
                                        if (trkSeg2.k == "ref")
                                        {
                                            punten3[o, 4] = trkSeg2.v;
                                        }
                                    }
                                    o++;
                                    jag2[j][g] = trkSeg.refr.PadLeft(10, '9');
                                    g++;
                                    lengthjag2++;
                                }
                            }
                        }
                    }
                    j++;
                }
                m++;
            }
            routids = routid.Distinct().ToList();
            routids.RemoveAll(item => item == null);
            jag2 = jag2.Where(x => x != null).ToArray();

        }
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
            punten4 = new string[k, 2];
            id = new string[h];
            jag = new string[h][];
            int j = 0;
            foreach (var wpt in waypoints)
            {
                int i = 0; int g = 0;
                id[j] = wpt.ID.PadLeft(10, '9');
                foreach (var wptSeg in wpt.Segs)
                {
                    punten4[o, 0] = wpt.ID.PadLeft(10, '9');
                    punten4[o, 1] = wptSeg.refr.PadLeft(10, '9');
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
            Sort(punten4, 0, "ASC");
        }
        string[,] puntenc;
        int[,] opvz;
        public void waysinorder()
        {
            puntenc = new string[lengthjag2, 2];
            int q = 0; int z = 0;
            foreach (string[] array in jag2)
            {
                List<int> puntenv = new List<int>();
                opvz = new int[array.Length, 3];
                string[] arrayid = new string[array.Length];
                for (int e = 0; e < array.Length; e++)
                {
                    int t = Array.IndexOf(id, array[e]);
                    if (t >= 0)
                    {
                        arrayid[e] = t.ToString();
                    }
                }
                for (int e = 0; e < arrayid.Length; e++)
                {
                    string first = ""; string last = "";
                    int a = 0;
                    int t = int.Parse(arrayid[e]);
                    first = jag[t][0];
                    last = jag[t][jag[t].Length - 1];

                    opvz[e, 1] = t + 1;
                    a = t;
                    testfirstandlast(false, arrayid, e, a, first, last, true);
                    testfirstandlast(false, arrayid, e, a, first, last, false);
                    testfirstandlast(true, arrayid, e, a, first, last, false);

                }

                insertpuntenv(puntenv);
                string[] puntenl = new string[puntenv.Count()];
                for (int e = 0; e < puntenv.Count(); e++)
                {
                    if (puntenv[e] != 0)
                    {
                        puntenl[e] = id[puntenv[e] - 1];
                    }
                }
                int l = 0;
                for (int t = 0; t < puntenl.Length; t++)
                {
                    int k = Array.IndexOf(id, puntenl[t]);
                    if (k >= 0)
                    {
                        puntenc[q, 0] = id[k];
                        puntenc[q, 1] = routids[z];
                        q++;
                        l++;
                    }
                }

                z++;
            }
        }

        public void testfirstandlast(bool contains, string[] arrayid, int e, int a, string first, string last, bool turn)
        {
            if (opvz[e, 0] == 0 || opvz[e, 2] == 0)
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
                                    opvz[e, 2] = i + 1;
                                }
                            if (last == jag[i][jag[i].Length - 1])
                            {
                                opvz[e, 0] = i + 1;
                            }
                            if (!turn)
                            {
                                if (opvz[e, 2] == 0)
                                {

                                    if (first == jag[i][0])
                                    {
                                        opvz[e, 2] = i + 1;
                                    }
                                }
                                if (opvz[e, 0] == 0)
                                {
                                    if (last == jag[i][jag[i].Length - 1])
                                    {
                                        opvz[e, 0] = i + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (opvz[e, 2] == 0)
                            {
                                if (jag[i].Contains(first))
                                {
                                    opvz[e, 2] = i + 1;
                                }
                            }
                            if (opvz[e, 0] == 0)
                            {
                                if (jag[i].Contains(last))
                                {
                                    opvz[e, 0] = i + 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void insertpuntenv(List<int> puntenv)
        {
            if (opvz[0, 0] != 0)
            {
                puntenv.Add(opvz[0, 0]);
            }
            puntenv.Add(opvz[0, 1]);
            if (opvz[0, 2] != 0)
            {
                puntenv.Add(opvz[0, 2]);
            }
            if (opvz.Length / 3 > 1)
            {
                if (opvz[0, 2] == 0 && opvz[0, 0] == 0)
                {
                    if (opvz[1, 0] != 0)
                    {
                        puntenv.Add(opvz[1, 0]);
                    }
                    puntenv.Add(opvz[1, 1]);
                    if (opvz[1, 2] != 0)
                    {
                        puntenv.Add(opvz[1, 2]);
                    }
                }
            }
            for (int t = 0; puntenv.Count() != (opvz.Length / 3) && t < (opvz.Length / 3) + 10; t++)
            {
                int j = 0;
                for (int e = 1; e < opvz.Length / 3; e++)
                {
                    if (puntenv[0] == opvz[e, 1] && puntenv[1] == opvz[e, 2] && (opvz[e, 0] != 0 || j != e))
                    {
                        int index = puntenv.IndexOf(opvz[e, 1]);
                        puntenv.Insert(index, opvz[e, 0]);
                        j = e;
                    }
                    if (puntenv[0] == opvz[e, 1] && puntenv[1] == opvz[e, 0] && (opvz[e, 2] != 0 || j != e))
                    {
                        int index = puntenv.IndexOf(opvz[e, 1]);
                        puntenv.Insert(index, opvz[e, 2]);
                        j = e;
                    }
                    if (puntenv[puntenv.Count() - 2] == opvz[e, 0] && puntenv[puntenv.Count() - 1] == opvz[e, 1] && (opvz[e, 2] != 0 || j != e))
                    {
                        int index = puntenv.IndexOf(opvz[e, 1]);
                        puntenv.Insert(index + 1, opvz[e, 2]);
                        j = e;
                    }
                    if (puntenv[puntenv.Count() - 2] == opvz[e, 2] && puntenv[puntenv.Count() - 1] == opvz[e, 1] && (opvz[e, 0] != 0 || j != e))
                    {
                        int index = puntenv.IndexOf(opvz[e, 1]);
                        puntenv.Insert(index + 1, opvz[e, 0]);
                        j = e;
                    }
                }
            }

        }
        string[,] punteny;
        public void four_and_one_to_y()
        {
            string[,] punten44 = new string[punten4.Length / 2, 3];



            int x = 0;
            for (int j = 0; j < punten4.Length / 2; j++)
            {
                int t = Array.BinarySearch(punten1ID, punten4[j, 1]);
                if (t >= 0)
                {
                    punten44[x, 0] = punten4[j, 0];
                    punten44[x, 1] = punten4[j, 1];
                    punten44[x, 2] = punten1[t, 3];
                    x++;
                }
            }
            punteny = clear_array_nulls(punten44);
        }
        string[,] puntens;
        public void y_and_tree_to_f()
        {
            string[] punten4ID = GetColumn(punteny, 0);
            string[,] puntend = clear_array_nulls(puntenc);
            int f = 0;
            string[,] puntenf = new string[((punten4.Length / 2) + (punten3.Length / 6)) * 5, 6];

            for (int j = 0; j < puntend.Length / 2; j++)
            {
                int t = Array.BinarySearch(punten4ID, puntend[j, 0]);
                if (t >= 0)
                {
                    puntenf[f, 0] = punteny[t, 1];
                    puntenf[f, 5] = punteny[t, 2];
                    for (int d = 0; d < punten3.Length / 6; d++)
                    {
                        if (punten3[d, 0] == puntend[j, 0] && punten3[d, 1] == puntend[j, 1])
                        {
                            puntenf[f, 1] = punten3[d, 2];
                            puntenf[f, 2] = punten3[d, 3];
                            puntenf[f, 3] = punten3[d, 4];
                            puntenf[f, 4] = punten3[d, 5];
                            break;
                        }
                    }
                    f++;
                }
            }
            puntens = clear_array_nulls(puntenf);
        }
        string[,] punten6;
        public void f_and_one_to_five()
        {
            string[,] punten5 = new string[puntens.Length / 6, 8];
            int r = 0;
            for (int c = 0; c < puntens.Length / 6; c++)
            {
                int t = Array.BinarySearch(punten1ID, puntens[c, 0]);
                if (t >= 0)
                {
                    punten5[r, 0] = puntens[c, 1];
                    punten5[r, 1] = punten1[t, 3];

                    punten5[r, 2] = punten1[t, 1];
                    punten5[r, 3] = punten1[t, 2];
                    punten5[r, 4] = punten1[t, 0];
                    punten5[r, 5] = puntens[c, 2];
                    punten5[r, 6] = puntens[c, 3];
                    punten5[r, 7] = puntens[c, 4];
                    r++;
                }
                if (r - 1 >= 0)
                    if (punten5[r - 1, 4] != null)
                    {
                        for (int y = 0; y < punten5.Length / 8; y++)
                        {
                            if (punten5[r - 1, 1] == punten5[y, 1] && punten5[y, 1] != null)
                            {
                                if (distance(double.Parse(punten5[r - 1, 2]), double.Parse(punten5[r - 1, 3]), double.Parse(punten5[y, 2]), double.Parse(punten5[y, 3])) < 0.05)
                                {
                                    punten5[r - 1, 4] = punten5[y, 4];
                                    punten5[r - 1, 2] = punten5[y, 2];
                                    punten5[r - 1, 3] = punten5[y, 3];
                                    break;
                                }
                            }
                        }
                    }
            }
            punten6 = clear_array_nulls(punten5);
        }
        string[,] punten8;
        public void five_to_seven()
        {
            string[,] punten7 = new string[punten6.Length / 8, 8];
            bool dupFound;
            int h = 0;
            for (int i = 0; i < punten6.Length / 8; i++)
            {
                dupFound = false;
                for (int a = i + 1; a < i + 5 && a < punten6.Length / 8; a++)
                {
                    if ((i != a) && punten6[a, 0] == punten6[i, 0] && punten6[a, 1] == punten6[i, 1])
                    {
                        dupFound = true;
                        break;
                    }
                }
                if (!dupFound)
                {
                    punten7[h, 0] = punten6[i, 0];
                    punten7[h, 1] = punten6[i, 1];
                    punten7[h, 2] = punten6[i, 2];
                    punten7[h, 3] = punten6[i, 3];
                    punten7[h, 4] = punten6[i, 4];
                    punten7[h, 5] = punten6[i, 5];
                    punten7[h, 6] = punten6[i, 6];
                    punten7[h, 7] = punten6[i, 7];
                    h++;
                }
            }
            punten8 = clear_array_nulls(punten7);
        }
        public void noderouting()
        {
            for (int j = 0; j < punten2.Length / 3; j++)
            {
                int t = Array.BinarySearch(punten1ID, punten2[j, 0]);
                if (t >= 0)
                {
                    punten2[j, 2] = punten1[t, 3];

                    bool dubbel = false;
                    foreach (Node node in dataModel.nodesrouting)
                    {
                        if (node.stationnaam == punten1[t, 3])
                            dubbel = true;
                    }

                    if (!dubbel)
                    {
                        dataModel.AddNoderouting(new Node(punten1[t, 0], double.Parse(punten1[t, 1]), double.Parse(punten1[t, 2]), "", punten1[t, 3], "0", "0", "0", true, 0));
                    }
                }
            }
        }
        public void linkrouting()
        {

            for (int j = 0; j < punten2.GetLength(0); j++)
            {
                if (j + 1 < punten2.GetLength(0))
                {

                    if (punten2[j, 1] == punten2[j + 1, 1])
                    {
                        Node station1node = dataModel.GetNodeName(punten2[j, 2], dataModel.GetNodesRouting());
                        Node station2node = dataModel.GetNodeName(punten2[j + 1, 2], dataModel.GetNodesRouting());
                        if (!(station1node == null || station2node == null))
                        {
                            Link link = new Link(station1node, station2node, punten2[j, 1]);
                            dataModel.AddLinkrouting(link);
                            station1node.addBuur(station2node);
                            station1node.addLink(new Link(station1node, station2node, punten2[j, 1]));
                            station2node.addBuur(station1node);
                            station2node.addLink(new Link(station2node, station1node, punten2[j, 1]));
                        }
                    }
                }

            }
        }
        public void seven_to_doneandnode()
        {
            int g = 0;
            pointsdone = new string[punten8.Length / 8, 9];
            bool dupFound;
            for (int i = 0; i < punten8.Length / 8; i++)
            {

                dupFound = false;
                pointsdone[g, 0] = punten8[i, 0];
                pointsdone[g, 1] = punten8[i, 1];
                pointsdone[g, 2] = punten8[i, 2];
                pointsdone[g, 3] = punten8[i, 3];
                pointsdone[g, 4] = punten8[i, 4];
                pointsdone[g, 5] = punten8[i, 5];
                pointsdone[g, 6] = punten8[i, 6];
                pointsdone[g, 7] = punten8[i, 7];
                pointsdone[g, 8] = "true";
                for (int a = 0; a < punten2.Length / 3; a++)
                {
                    if (punten8[i, 0] == punten2[a, 1] && punten8[i, 1] == punten2[a, 2])
                    {
                        dupFound = true;
                        break;
                    }
                }
                if (!dupFound)
                {
                    pointsdone[g, 8] = "false";
                }
                double x1 = double.Parse(pointsdone[g, 2]);
                double y1 = double.Parse(pointsdone[g, 3]);
                bool stop = bool.Parse(pointsdone[g, 8]);
                dataModel.AddNode(new Node(pointsdone[g, 4], x1, y1, pointsdone[g, 0], pointsdone[g, 1], pointsdone[g, 5], pointsdone[g, 6], pointsdone[g, 7], stop, 0));
                g++;

            }
        }
        public void links()
        {
            for (int i = 0; i < pointsdone.Length / 9; i++)
            {
                if (i + 1 < pointsdone.Length / 9)
                {
                    if (pointsdone[i, 0] == pointsdone[i + 1, 0])
                    {
                        Node station1node = dataModel.GetNode(pointsdone[i, 4], dataModel.GetNodes());
                        Node station2node = dataModel.GetNode(pointsdone[i + 1, 4], dataModel.GetNodes());
                        Link link = new Link(station1node, station2node, pointsdone[i, 0]);
                        dataModel.AddLink(link);
                        station1node.addBuur(station2node);
                        //station1node.addLink(new Link(station1node, station2node, puntenklaar[g,0]));
                        station2node.addBuur(station1node);
                        //station2node.addLink(new Link(station2node, station1node, puntenklaar[g,0]));
                    }
                }

            }
        }
        public void combinepoints()
        {
            four_and_one_to_y();
            y_and_tree_to_f();
            f_and_one_to_five();
            five_to_seven();
            noderouting();
            linkrouting();
            seven_to_doneandnode();
            links();

            dataModel.get_unique_nodes();
            dataModel.get_unique_links(); 
        }
        public static double distance(double x1, double y1, double x2, double y2)
        {
            // Calculating distance 
            return Math.Sqrt(Math.Pow(x2 - x1, 2) +
                          Math.Pow(y2 - y1, 2) * 1.0);
        }
        public string[,] clear_array_nulls(string[,] input)
        {
            int m = input.GetUpperBound(0);
            int n = input.GetUpperBound(1) + 1;
            string[] temp = new string[input.GetUpperBound(0)];
            for (int x = 0; x < m; x++)
                temp[x] = input[x, 0];
            temp = temp.Where(s => !object.Equals(s, null)).ToArray();
            string[,] output = new string[temp.Length, n];
            Array.Copy(input, output, temp.Length * n);
            return output;
        }
        public int[,] clear_array_nulls(int[,] input)
        {
            int m = input.GetUpperBound(0);
            int n = input.GetUpperBound(1) + 1;
            int[] temp = new int[input.GetUpperBound(0)];
            for (int x = 0; x < m; x++)
                temp[x] = input[x, 0];
            temp = temp.Where(s => !object.Equals(s, 0)).ToArray();
            int[,] output = new int[temp.Length, n];
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
        public List<Node> GetNodesrouting()
        {
            return nodesrouting;
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
        public Node GetNoderouting(string name, List<Node> lijst)
        {
            foreach (Node node in lijst)
            {
                if (name == node.stationnaam)
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
        public List<Node> Buren;
        // array met pointers naar alle links die verbonden zijn met deze node
        public List<Link> Connecties;
        // 'echte' coordinaten
        public double x, y;
        public int number;
        // unieke indentifier, naam in de vorm van een string
        public string name_id, routnaam, stationnaam, soortrout, routid, vervoersmiddels;
        public bool stops;
        public Node NearestToStart;
        public int MinCostToStart = int.MaxValue;
        public bool Visited = false;
        public Node(string name, double coordx, double coordy, string routenaam, string stationsnaam, string soortroute, string routeid, string vervoersmiddel, bool stop, int i)
        {
            number = i;
            name_id = name;
            x = coordx;
            y = coordy;
            routnaam = routenaam;
            stationnaam = stationsnaam;
            soortrout = soortroute;
            routid = routeid;
            vervoersmiddels = vervoersmiddel;
            stops = stop;
            Buren = new List<Node>();
            Connecties = new List<Link>();
        }
        public void addBuur(Node buur)
        {
            bool buurtest = true;
            foreach (Node naaste in Buren)
            {
                if (naaste == buur)
                    buurtest = false;
            }
            if (buurtest)
                Buren.Add(buur);
        }
        public void addLink(Link link)
        {
            Connecties.Add(link);
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

        public int Weight = 1;



        public Link(Node startpunt, Node eindpunt, string RouteNames)
        {
            Start = startpunt;
            End = eindpunt;
            RouteName = RouteNames;
        }
    }
}
