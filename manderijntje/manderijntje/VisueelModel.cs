using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;

namespace manderijntje
{
    /*this class contains all the methods for the connection between the visual classes (in this file) and other classes*/
    public class Connecion_to_files
    {
        public lists_change l = new lists_change();
        private changes b = new changes();
        public VisueelModel access;
        private const string filepath = "C:/Way2Go/visueelmodel_binary.txt";        

        //constructor class, when project is opened it will check if there is a data file available to work with, otherwise it creates a new one
        public Connecion_to_files(DataModel data)
        {
            if (File.Exists(filepath) && !Program.reimport)
            {
                try
                {
                    files.disk_read(this, filepath);
                }
                catch
                {
                    MakeDataForDisk(data);
                }
            }
            else
            {
                MakeDataForDisk(data);
            }


        }

        //if there is no data file, this mothod will create a new file
        private void MakeDataForDisk(DataModel data)
        {
            access = (new parsing(data)).getModel(false);

            if (Directory.Exists(filepath))
            {
                files.writing_disk(access, FileMode.Open, filepath);
            }
            else
            {
                files.writing_disk(access, FileMode.Create, filepath);
            }
        }

        //this method rescales all nodes acording to the zoom (in other methode width*zoom and height*zoom)
        public void SetSizeMap(int width, int height)
        {
            Point[] points = new Point[1000]; 

            for (int i = 0; i < access.nodes.Count; i++)
            {
                try
                {
                    points[i] = access.nodes[i].point;
                }
                catch (Exception)
                {
                    break;
                }

            }

            points = coordinates.ScalePointsToSize(points, width, height);

            for (int i = 0; i < points.Length; i++)
            {
                try
                {
                    access.nodes[i].point = points[i];
                }
                catch (Exception)
                {
                    break;
                }
            }

        }

        //is responsible for corectly colloring all nodes
        public void visualcontrol(int screenheight, int factor, Point startmouse, Point endmouse, List<Node> s, bool stationnames, MapView map)
        {

            if (stationnames)
            {
                foreach (VisueelNode v in access.nodes)
                {
                        v.Color = Color.Gray;
                }


                foreach (VisualLink v in access.links)
                {
                    v.kleur = Color.Gray;
                }

                foreach (VisueelNode v in access.nodes)
                {
                    foreach (Node n in s)
                    {
                        if(v.name_id == n.stationnaam)
                            v.Color = Color.Orange;

                    }

                }


                foreach (vLogicalLink v in access.connections)
                {
                    string firstName, lastName;

                    for (int n = 0; n < s.Count; n++)
                    {
                        firstName = s[n].stationnaam;

                        for (int m = 0; m < s.Count; m++)
                        {
                            lastName = s[m].stationnaam;

                            if (firstName == v.v.name_id && lastName == v.u.name_id)
                            {
                                for (int i = 0; i < v.nodes.Count; i++)
                                {
                                    v.nodes[i].Color = Color.Orange;
                                }

                                for (int i = 0; i < v.links.Count; i++)
                                {
                                    v.links[i].kleur = Color.Orange;
                                }
                            }
                        }
                            
                        

                        
                    }

                }


                foreach (VisualLink m in access.links)
                {

                    if (m.v.Color == Color.Orange && m.u.Color == Color.Orange && !m.v.dummynode && !m.u.dummynode)
                    {
                        m.kleur = Color.Orange;

                    }

                }
            }

            l.valuenode(access, factor, screenheight, b, startmouse, endmouse, stationnames, new Point(0, 0), new Point(0, 0), map);

        }

    }


    /*this class contains all lists*/
    [Serializable]
    public class VisueelModel
    {
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisualLink> links = new List<VisualLink>();
        public List<vLogicalLink> connections = new List<vLogicalLink>();
    }


    /*this class controls the lists*/
    [Serializable]
    public class lists_change
    {

        int toshiftX = 50, toshifty = 50; 

        //sets default xmovement and y movement when zooming out
        public void changez(int factor, int width, int height)
        {
            toshiftX += ((width / 2) * factor) - ((width / 2) * (factor + 1));
            toshifty += ((height / 2) * factor) - ((height / 2) * (factor + 1));
        }

        //sets default xmovement and y movement when zooming in
        public void change(int factor, int width, int height)
        {
            toshiftX += ((width/2) * (factor - 1)) - ((width/2) * (factor - 2));
            toshifty += ((height/2) * (factor - 1)) - ((height/2) * (factor-2));
        }
       

        //set bool value to true or false denping the given variables, true is for paint and falae is not paint
        public void valuenode(VisueelModel access, int factor, int screenwidth, changes b, Point start, Point end, bool stations, Point startpoint, Point endpoint, MapView map)
        {

            Point shift = b.movemap(start, end);
           toshiftX += shift.X;
            toshifty+= shift.Y;

           



            foreach (VisueelNode v in access.nodes)
            {
                if ((v.point.X - toshiftX) > 0 && (v.point.X - toshiftX) < (screenwidth) && (v.point.Y - toshifty) > 0 && (v.point.Y - toshifty) < (screenwidth))
                {
                    if (stations && v.point.X > startpoint.X && v.point.X > startpoint.Y && v.point.X < endpoint.X && v.point.Y < endpoint.Y)
                    {
                        switching(v, factor, map);
                    }
                    else
                    {
                        switching(v, factor, map);
                    }

                }
                else
                {
                    v.paint = false;
                }
            }
            
           foreach(VisualLink v in access.links)
            {
               if (v.u.paint && v.v.paint)
                    map.links.Add(v);
            }

            foreach (vLogicalLink v in access.connections)
            {
                if (v.u.paint && v.v.paint)
                    map.logicallinks.Add(v);
            }
            
        }


        //helping method valuenode
        public void switching(VisueelNode v, int zoom, MapView map)
        {

            switch (zoom)
            {
                case 0:
                   //v.paint = (v.prioriteit < 5) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v); 

                    break;
                case 1:
                    // v.paint = (v.prioriteit < 4) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 2:
                    //v.paint = (v.prioriteit < 3) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 3:
                    // v.paint = (v.prioriteit < 2) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                default:
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
            }
        }

    }


    /*this classes contain all methods with direct or indirect user input and change the map according to the input */
    public class changes
    {
        //returns shift over x and y direction as a point
        public Point movemap(Point startmouse, Point endmouse)
        {
            return new Point(startmouse.X - endmouse.X, startmouse.Y - endmouse.Y);
        }

        // uses 2 points to return in getzoom
        public struct zoomtostations
        {
            public Point biggest, smallest;
        }

        //returns coordinate with smallest x,y and one with biggest x,y
        public zoomtostations getpoints(List<Point> p)
        {
            zoomtostations stations = new zoomtostations();
            int tolerance = 15;

            stations.smallest = new Point(p.Min(Point => Point.X) - tolerance, p.Min(Point => Point.Y) - tolerance);
            stations.biggest = new Point(p.Max(Point => Point.X) + tolerance, p.Max(Point => Point.Y) + tolerance);

            return stations;
        }

     
        
        
    }

    //these classes contain methods for reading and writing to the disk
    public class files
    {
        //method for reading from the disk
        public static void disk_read(Connecion_to_files c, string path)
        {
            try
            {
                using (Stream str = File.Open(path, FileMode.Open))
                {
                    var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    c.access = (VisueelModel)formater.Deserialize(str);
                }
            } 
            catch
            {
                MessageBox.Show("File coudn't be opened", "Error", MessageBoxButtons.OK);
            }
        }

        //writing to the disk
        public static void writing_disk(VisueelModel l, FileMode fm, string path)
        {
            try
            {
                using (Stream str = File.Open(path, fm))
                {
                    var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formater.Serialize(str, l);
                }
            }
            catch
            {
                MessageBox.Show("File coudn't be opened", "Error", MessageBoxButtons.OK);
            }

        }
    }


    /*this class is a constructor for the list*/
    [Serializable]
    public class VisueelNode
    {
        public int prioriteit, index;
        public Point point;
        public string name_id;
        public Color Color = Color.Gray;
        public bool paint = true;
        public bool dummynode = false;
        public bool dummynodeDrawLine = false; 

        public VisueelNode(Point point, string name_id, int prioriteit)
        { 
            this.point = point;
            this.name_id = name_id;
            this.prioriteit = prioriteit;
        }

    }


    /*this class is a constructor for the list*/
    [Serializable]
    public class VisualLink
    {
        public string name_id;
        public Color kleur = Color.Gray;
        public bool paint = true;
        public VisueelNode u, v;

        public VisualLink(string name_id)
        {
            this.name_id = name_id;
        }

    }


    /*this class is a constructor for the list*/
    [Serializable]
    public class vLogicalLink
    {
        public VisueelNode u, v;
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisualLink> links = new List<VisualLink>();

        public vLogicalLink(VisueelNode U, VisueelNode V)
        {
            u = U;
            v = V;
        }

        //if ther exists a dummy node in a route, this method finds the dummy node and the connecting stations
        public void getLinks(List<VisualLink> l)
        {
            List<VisueelNode> booltest = nodes;
            booltest.Add(u);
            booltest.Add(v);
            for (int i = 0; i < booltest.Count; i++)
            {
                for (int n = i; n < booltest.Count; n++)
                {
                    if (getLink(booltest[i], booltest[n], l) != null)
                    {
                        links.Add(getLink(booltest[i], booltest[n], l));
                    }
                }
            }
        }

      
        private VisualLink getLink(VisueelNode u, VisueelNode v, List<VisualLink> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if ((l[i].u == u && l[i].v == v) || (l[i].u == v && l[i].v == u))
                {
                    return l[i];
                }
            }
            return null;
        }
    }

}
