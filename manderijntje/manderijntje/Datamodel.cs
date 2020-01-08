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

       // string sFile = "../../../../groningen test2.xml";
        // string sFile = "../../../../enkhuizen test 4.xml";
        //string sFile = "../../../../amsterdam test tram subway train2.xml";
        //string sFile = "../../../../train germany test.xml";
         string sFile = "../../../../groenhart train.xml";
        //string sFile = "../../../../amsterdam tram bus.xml";
        //string sFile = "../../../../subway london.xml";
        //string sFile = "../../../../berlin subway.xml";
        //string sFile = "../../../../train frankrijk.xml";
        //string sFile = "../../../../train uk.xml";
        // string sFile = "../../../../frankrijk test3.xml";
        //string sFile = "../../../../train europa.xml";
        //string sFile = "../../../../train westeuropa.xml";
        //string sFile = "../../../../subway europa.xml";
        string filename = @"C:\Users\Aletta S\Documents\Aletta\Utrecht\introductie project\train test.txt";

        public DataControl(string file_path)
        {

            
            LoadWP(sFile);//punten met stationnamen
            LoadTracks(sFile);//punten met routes
            LoadWay(sFile);
            dataModel = new DataModel();
            puntensamenvoegen();//samen naar een tweedimensionale array
            
            

        }


        DataModel dataModel;

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
        string[,] puntenklaar; // klaar
        string[,] puntenklaar2; // lat en long verandererd

        int lengthjag2;
        string[] punten1ID;
        double lamin = 100000000, lomin = 100000000; //min
        double lamax = 0, lomax = 0;//max
        double lamid = 0, lomid = 0;//midden ervan
        List<string> routes;
        string[] id; List<string> routids;
        string[][] jag; string[][] jag2;
        public void LoadWP(string sFile)
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
                        //pakt coordinaat met punt en doet dan naar een met , zodat het parsebaar is
                        string[] latitude = wpt.Latitude.Split('.');
                        string latitude2 = latitude[0] + "," + latitude[1];
                        string[] longitude = wpt.Longitude.Split('.');
                        string longitude2 = longitude[0] + "," + longitude[1];
                        //berekend min en max hoeken kaart
                        if (lamin > double.Parse(latitude2))
                        {
                            lamin = double.Parse(latitude2);
                        }
                        if (lomin > double.Parse(longitude2))
                        {
                            lomin = double.Parse(longitude2);
                        }
                        if (lamax < double.Parse(latitude2))
                        {
                            lamax = double.Parse(latitude2);
                        }
                        if (lomax < double.Parse(longitude2))
                        {
                            lomax = double.Parse(longitude2);
                        }
                        a++;
                    }
                }

            }
            //berekend verschil
            lamid = lamax - lamin;
            lomid = lomax - lomin;
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
                        string[] latitude = wpt.Latitude.Split('.');
                        string latitude2 = latitude[0] + "," + latitude[1];
                        string[] longitude = wpt.Longitude.Split('.');
                        string longitude2 = longitude[0] + "," + longitude[1];
                        //weer . eraf , erbij

                        punten1[n, 0] = wpt.ID.PadLeft(10, '9');
                        punten1[n, 1] = longitude2; //longitude
                        punten1[n, 2] = latitude2; //latitude

                        ID[n] = double.Parse(wpt.ID);
                        punten1[n, 3] = wptSeg.v;
                        n++;
                    }


                }

            }


            Sort(punten1, 0, "ASC");
            punten1ID = GetColumn(punten1, 0);

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
        public void LoadTracks(string sFile)
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

                    if (trkSeg2.k == "from")
                    {
                        from = trkSeg2.v;

                    }
                    if (trkSeg2.k == "to")
                    {
                        to = trkSeg2.v;

                    }

                    if (trkSeg2.k == "public_transport")
                    {
                        pt2 = trkSeg2.v;

                    }
                    if (trkSeg2.k == "wikidata")
                    {
                        w = trkSeg2.v;

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
                    bool dubbel = false;

                    for (int e = 0; e < Q.Count - 1; e++)
                    {
                        if (P[e] == refr && Q[e] == net && R[e] == op && U[e] == w)
                        {
                            if (S[e] == to && T[e] == from)
                            {

                                dubbel = true;



                            }
                        }

                    }

                    if (dubbel == false)
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
                    bool dubbel = false;

                    for (int e = 0; e < M.Count - 1; e++)
                    {
                        if (N[e] == refr && M[e] == net && O[e] == op && J[e] == w)
                        {
                            if (K[e] == to && L[e] == from)
                            {
                                dubbel = true;



                            }
                        }

                    }

                    if (dubbel == false)
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
                                    routid[o] = trk.ID;

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
            Sort(punten3, 0, "ASC");
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


        public void puntensamenvoegen()
        {


            string[,] puntenc = new string[lengthjag2, 2];
            int q = 0; int z = 0;
            foreach (string[] array in jag2)
            {


                List<int> puntenv = new List<int>();
                int[,] opvz = new int[array.Length, 3];
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
                    string first1 = ""; string last1 = "";
                    int a = 0;
                    int t = int.Parse(arrayid[e]);

                    first = jag[t][0];

                    last = jag[t][jag[t].Length - 1];
                    if (jag[t].Length > 2)
                    {
                        first1 = jag[t][1];
                        last1 = jag[t][jag[t].Length - 2];
                    }

                    opvz[e, 1] = t + 1;
                    a = t;



                    for (int d = 0; d < arrayid.Length; d++)
                    {
                        int i = int.Parse(arrayid[d]);
                        if (a != i)
                        {

                            if (first == jag[i][0])
                            {
                                opvz[e, 2] = i + 1;

                            }



                            if (last == jag[i][0])
                            {
                                opvz[e, 0] = i + 1;

                            }


                        }
                    }
                    if (opvz[e, 0] == 0 || opvz[e, 2] == 0)
                    {
                        for (int d = 0; d < arrayid.Length; d++)
                        {
                            int i = int.Parse(arrayid[d]);
                            if (i >= 0 && a != i)
                            {
                                if (opvz[e, 2] == 0)
                                {

                                    if (first == jag[i][jag[i].Length - 1])
                                    {
                                        opvz[e, 2] = i + 1;
                                    }
                                }
                                if (opvz[e, 0] == 0)
                                {
                                    if (last == jag[i][0])
                                    {
                                        opvz[e, 0] = i + 1;

                                    }

                                }

                            }
                        }
                    }



                    if (opvz[e, 0] == 0 || opvz[e, 2] == 0)
                    {
                        for (int d = 0; d < arrayid.Length; d++)
                        {
                            int i = int.Parse(arrayid[d]);
                            if (i >= 0 && a != i)
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
                    if ((opvz[e, 0] == 0 || opvz[e, 2] == 0) && first1 != "")
                    {
                        for (int d = 0; d < arrayid.Length; d++)
                        {
                            int i = int.Parse(arrayid[d]);
                            if (i >= 0 && a != i)
                            {


                                if (opvz[e, 2] == 0)
                                {
                                    if (jag[i].Contains(first1))
                                    {
                                        opvz[e, 2] = i + 1;

                                    }
                                }
                                if (opvz[e, 0] == 0)
                                {
                                    if (jag[i].Contains(last1))
                                    {
                                        opvz[e, 0] = i + 1;

                                    }
                                }


                            }


                        }

                    }

                }
                /*
                string dd = "";
                for(int d = 0; d<opvz.Length/3; d++)
                {
                    dd = dd + d +"-"+ opvz[d, 0] + "_" + opvz[d, 1] + "_" + opvz[d, 2] + "        " + "\n";
                }

                */

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
                if (puntenl.Length != l + 1 && puntenl.Length != l && puntenl.Length != l + 2)
                {

                }
                z++;

            }



            string[,] puntenf = new string[((punten4.Length / 2) + (punten3.Length / 6)) * 5, 6];
            string[,] punten44 = new string[punten4.Length / 2, 3];
            int r = 0;
            double ll = punten1.Length / 4;


            int f = 0;

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

            string[,] punteny = clear_array_nulls(punten44);
            string[] punten4ID = GetColumn(punteny, 0);
            string[,] puntend = clear_array_nulls(puntenc);
            string[] punten3ID = GetColumn(punten3, 0);


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
            string[,] puntens = clear_array_nulls(puntenf);
            string[,] punten5 = new string[puntens.Length / 6, 8];
            string[] duproutes = new string[puntens.Length / 6];
            for (int c = 0; c < puntens.Length / 6; c++)
            {

                int t = Array.BinarySearch(punten1ID, puntens[c, 0]);

                if (t >= 0)
                {

                    punten5[r, 0] = puntens[c, 1];
                    punten5[r, 1] = punten1[t, 3];
                    duproutes[r] = puntens[c, 1];

                    punten5[r, 2] = punten1[t, 1];
                    punten5[r, 3] = punten1[t, 2];
                    punten5[r, 4] = punten1[t, 0];

                    punten5[r, 5] = puntenf[c, 2];
                    punten5[r, 6] = puntenf[c, 3];
                    punten5[r, 7] = puntenf[c, 4];
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
            string[,] punten6 = clear_array_nulls(punten5);
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
            string[,] punten8 = clear_array_nulls(punten7);

            routes = duproutes.Distinct().ToList();
            routes.RemoveAll(item => item == null);


            for (int j = 0; j < punten2.Length / 3; j++)
            {


                int t = Array.BinarySearch(punten1ID, punten2[j, 0]);

                if (t >= 0)
                {

                    punten2[j, 2] = punten1[t, 3];

                }
                if (t < 0)
                {

                }
            }

            int g = 0; string na = "";
            puntenklaar = new string[punten8.Length / 8, 9];
            for (int i = 0; i < punten8.Length / 8; i++)
            {

                na = na + "1" + punten8[i, 0] + punten8[i, 1] + "\n";
                dupFound = false;
                puntenklaar[g, 0] = punten8[i, 0];
                puntenklaar[g, 1] = punten8[i, 1];
                puntenklaar[g, 2] = punten8[i, 2];
                puntenklaar[g, 3] = punten8[i, 3];
                puntenklaar[g, 4] = punten8[i, 4];
                puntenklaar[g, 5] = punten8[i, 5];
                puntenklaar[g, 6] = punten8[i, 6];
                puntenklaar[g, 7] = punten8[i, 7];
                puntenklaar[g, 8] = "true";
                for (int a = 0; a < punten2.Length / 9; a++)
                {
                    if (punten8[i, 0] == punten2[a, 1] && punten8[i, 1] == punten2[a, 2])
                    {
                        dupFound = true;
                        break;
                    }

                }

                if (!dupFound)
                {


                    puntenklaar[g, 8] = "false";

                }


                double x1 = double.Parse(puntenklaar[g, 2]);
                double y1 = double.Parse(puntenklaar[g, 3]);
                bool stop = bool.Parse(puntenklaar[g, 8]);
                dataModel.AddNode(new Node(puntenklaar[g, 4], x1, y1, puntenklaar[g, 0], puntenklaar[g, 1], puntenklaar[g, 5], puntenklaar[g, 6], puntenklaar[g, 7], stop,0));
                g++;
            }



            for (int i = 0; i < puntenklaar.Length / 9; i++)
            {
                if (i + 1 < puntenklaar.Length / 9)
                {
                    if (puntenklaar[i, 0] == puntenklaar[i + 1, 0])
                    {
                        Node station1node = dataModel.GetNode(puntenklaar[i, 4], dataModel.GetNodes());

                        Node station2node = dataModel.GetNode(puntenklaar[i + 1, 4], dataModel.GetNodes());

                        Link link = new Link(station1node, station2node);
                        dataModel.AddLink(link);
                        station1node.addBuur(station2node);
                        station1node.addLink(new Link(station1node, station2node));
                        station2node.addBuur(station1node);
                        station2node.addLink(new Link(station2node, station1node));

                    }


                }

            }

            puntenklaar2 = new string[puntenklaar.Length / 9, 9];

            dataModel.get_unique_nodes();
            dataModel.get_unique_links();
        }

        static double distance(double x1, double y1, double x2, double y2)
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
        List<Node> nodes;
        List<Link> links;

        public List<Node> unique_nodes;
        public List<Link> unique_links;

        public DataModel()
        {
            nodes = new List<Node>();
            links = new List<Link>();

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

        
        public List<Node> GetNodes()
        {
            return nodes;
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

        public void AddLink(Link l)
        {
            links.Add(l);
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

        public double MinCostToStart = Double.MaxValue;

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
    }

    [Serializable]
    public class Link
    {
        // twee pointers die wijzen naar de twee nodes die deze link verbind
        public Node Start, End;

        public double Weight;



        public Link(Node startpunt, Node eindpunt)
        {
            Start = startpunt;
            End = eindpunt;

        }
    }

}






