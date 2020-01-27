using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Manderijntje
{

    /// <summary>
    /// this class contains all the methods for the connection between the visual classes (in this file) and other classes
    /// </summary>
    public class Connecion_to_files
    {
        public Lists_Change l = new Lists_Change();
        private Changes b = new Changes();
        public VisualModel access;
        private const string filepath = "C:/Way2Go/visueelmodel_binary.txt";

        /// <summary>
        /// constructor class, when project is opened it will check if there is a data file available to work with, otherwise it creates a new one
        /// </summary>
        /// <param name="data">Links the DataModel file</param>
        public Connecion_to_files(DataModel data)
        {
            if (File.Exists(filepath) && !Program.reimport)
            {
                try
                {
                    Files.Disk_read(this, filepath);
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

        /// <summary>
        /// if there is no data file, this mothod will create a new file
        /// </summary>
        /// <param name="data">Links the DataModel file</param>
        private void MakeDataForDisk(DataModel data)
        {
            access = (new Parsing(data)).GetModel(false);

            if (Directory.Exists(filepath))
            {
                Files.Writing_disk(access, FileMode.Open, filepath);
            }
            else
            {
                Files.Writing_disk(access, FileMode.Create, filepath);
            }
        }

        /// <summary>
        /// this method rescales all nodes acording to the zoom (in other methode width*zoom and height*zoom)
        /// </summary>
        /// <param name="width">The width of the map from the file MapView</param>
        /// <param name="height">The height of the map from the file Mapview</param>
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

            points = Coordinates.ScalePointsToSize(points, width, height);

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

        /// <summary>
        /// This method sets the amount of connections a node has to other nodes (1 to 8).
        /// </summary>
        public void CountConnections()
        {
            foreach(VisueelNode v in access.nodes)
            {
                foreach(VisualLink n in access.links)
                {
                    if (v.name_id == n.v.name_id || v.name_id == n.u.name_id)
                        v.numberOfLinks++;
                }
            }
        }

        /// <summary>
        /// This method sets the collor of the nodes depending on what country they are in.
        /// </summary>
        public void Colorchange()
        {
            foreach (VisueelNode v in access.nodes)
            {
                switch (v.country)
                {
                    case "Italy":
                        v.Color = Color.Yellow;
                        break;
                    case "Nederland":
                        v.Color = Color.Blue;
                        break;
                    case "België":
                        v.Color = Color.Pink;
                        break;
                    case "Spanje":
                        v.Color = Color.Red;
                        break;
                    case "France":
                        v.Color = Color.DarkBlue;
                        break;
                    case "Duitsland":
                        v.Color = Color.Green;
                        break;
                    case "Zwitserland":
                        v.Color = Color.Purple;
                        break;
                    default:
                        v.Color = Color.Black;
                        break;
                }
            }
        }

        /// <summary>
        /// is responsible for corectly colloring all nodes
        /// </summary>
        /// <param name="screenheight">Screenheight of the map from the Mapvieuw file</param>
        /// <param name="factor">The value of the zoom from the Mapview</param>
        /// <param name="startmouse">strat coordinate of the mouse from MapView</param>
        /// <param name="endmouse">end coordinate of the mouse from MapView</param>
        /// <param name="s">List of Nodes on the route the user entered, comming from Form1</param>
        /// <param name="stationnames">Only if a new set of names is filled in by the user, this bool turns to true</param>
        /// <param name="map">Gives the connection to the MapView</param>
        public void Visualcontrol(int screenheight, int factor, Point startmouse, Point endmouse, List<Node> s, bool stationnames, MapView map)
        {
            if (stationnames)
            {
                Colorchange();

                foreach (VisueelNode v in access.nodes)
                {
                    if (s[0].stationName == v.name_id)
                    {
                        v.priorityLinks = true;
                        map.station1 = s[0].stationName;
                    }
                       

                    if (s[s.Count - 1].stationName == v.name_id)
                    {
                        v.priorityLinks = true;
                        map.station2 = s[s.Count - 1].stationName;
                    }                      
                }

                foreach (VisualLink v in access.links)
                 {
                     v.kleur = Color.Gray;
                 }

                foreach (VisueelNode v in access.nodes)
                {
                    foreach (Node n in s)
                    {
                        if(v.name_id == n.stationName)
                            v.Color = Color.Orange;

                    }

                }

                foreach (VLogicalLink v in access.connections)
                {
                    string firstName, lastName;

                    for (int n = 0; n < s.Count; n++)
                    {
                        firstName = s[n].stationName;

                        for (int m = 0; m < s.Count; m++)
                        {
                            lastName = s[m].stationName;

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

            l.Valuenode(access, factor, screenheight, b, startmouse, endmouse, stationnames, map);

        }
    }


    /// <summary>
    /// this is the constructor class and contains all lists
    /// </summary>
    [Serializable]
    public class VisualModel
    {
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisualLink> links = new List<VisualLink>();
        public List<VLogicalLink> connections = new List<VLogicalLink>();
    }


    /// <summary>
    /// this class controls the lists
    /// </summary>
    [Serializable]
    public class Lists_Change
    {
        int toshiftX = -150, toshifty = 50;

        /// <summary>
        /// sets default x movement and y movement when zooming out
        /// </summary>
        /// <param name="factor">the value of zoom from the MapView</param>
        /// <param name="width">The width of the map from Mapview</param>
        /// <param name="height">The height of teh map from Mapview</param>
        public void Changez(int factor, int width, int height)
        {
            toshiftX += ((width / 2) * factor) - ((width / 2) * (factor + 1));
            toshifty += ((height / 2) * factor) - ((height / 2) * (factor + 1));
        }

        /// <summary>
        /// Sets default x movement and y movement when zooming in.
        /// </summary>
        /// <param name="factor">The value of zoom from the MapView</param>
        /// <param name="width">The width of the map from Mapview</param>
        /// <param name="height">The height of teh map from Mapview</param>
        public void Change(int factor, int width, int height)
        {
            toshiftX += ((width/2) * (factor - 1)) - ((width/2) * (factor - 2));
            toshifty += ((height/2) * (factor - 1)) - ((height/2) * (factor-2));
        }


        /// <summary>
        /// set bool value to true or false denping the given variables, true is for paint and falae is not paint
        /// </summary>
        /// <param name="access">Gives acces to the class VisueelModel in VisueelModel</param>
        /// <param name="factor">The value of zoom from the MapView</param>
        /// <param name="screenHeight">Screenheight of the map from the Mapvieuw file</param>
        /// <param name="b">Gives acces to the the Class Changes in VisueelModel</param>
        /// <param name="start">strat coordinate of the mouse from MapView</param>
        /// <param name="end">end coordinate of the mouse from MapView</param>
        /// <param name="stations">Only if a new set of names is filled in by the user, this bool turns to true</param>
        /// <param name="map">Gives the connection to the MapView</param>
        public void Valuenode(VisualModel access, int factor, int screenHeight, Changes b, Point start, Point end, bool stations, MapView map)
        {
            Point shift = b.Movemap(start, end);
            toshiftX += shift.X;
            toshifty += shift.Y;

            foreach (VisueelNode v in access.nodes)
            {
                if ((v.point.X - toshiftX) > 0 && (v.point.X - toshiftX) < (screenHeight) && (v.point.Y - toshifty) > 0 && (v.point.Y - toshifty) < (screenHeight))
                {
                    Switching(v, factor, map);             
                }
                else
                {
                    v.paint = false;
                }
            }
            
           foreach(VisualLink v in access.links)
            {
                if(v.v.paint || v.u.paint)
                    map.links.Add(v);
            }

            foreach (VLogicalLink v in access.connections)
            {
                if(v.v.paint || v.u.paint)
                    map.logicallinks.Add(v);
            }           
        }


        /// <summary>
        /// helping method valuenode
        /// </summary>
        /// <param name="v">Gives acces to the class VisueelNode</param>
        /// <param name="zoom">is the value of the zoom from MapView</param>
        /// <param name="map">Givves acces to MapView file</param>
        public void Switching(VisueelNode v, int zoom, MapView map)
        {
            switch (zoom)
            {
                case 1:                 
                    v.priorityLinks = (v.numberOfLinks < 8) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v); 
                    break; 
                case 2:
                    v.priorityLinks = (v.numberOfLinks < 7) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 3:
                    v.priorityLinks = (v.numberOfLinks < 6) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 4:
                    v.priorityLinks = (v.numberOfLinks < 5) ? false : true; 
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 5:
                    v.priorityLinks = (v.numberOfLinks < 4) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 6:
                    v.priorityLinks = (v.numberOfLinks < 3) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 7:
                    v.priorityLinks = (v.numberOfLinks < 2) ? false : true; 
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                case 8:
                    v.priorityLinks = (v.numberOfLinks < 1) ? false : true;
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
                default:
                    v.paint = true;
                    if (v.paint) map.nodes.Add(v);
                    break;
            }

            if (v.name_id == map.station1)
                v.priorityLinks = true;

            if (v.name_id == map.station2)
                v.priorityLinks = true;
        }
    }


    /// <summary>
    /// this class contain all methods with direct or indirect user input and change the map according to the input
    /// </summary>
    public class Changes
    {
        /// <summary>
        /// returns shift over x and y direction as a point
        /// </summary>
        /// <param name="startmouse">strat coordinate of the mouse from MapView</param>
        /// <param name="endmouse">end coordinate of the mouse from MapView</param>
        /// <returns></returns>
        public Point Movemap(Point startmouse, Point endmouse)
        {
            return new Point(startmouse.X - endmouse.X, startmouse.Y - endmouse.Y);
        }         
    }

    /// <summary>
    /// these classes contain methods for reading and writing to the disk
    /// </summary>
    public class Files
    {
        /// <summary>
        /// method for reading from the disk
        /// </summary>
        /// <param name="c">Gives acces to the class Connectiom_to_files</param>
        /// <param name="path">Is the path where the files is saved</param>
        public static void Disk_read(Connecion_to_files c, string path)
        {
            try
            {
                using (Stream str = File.Open(path, FileMode.Open))
                {
                    var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    c.access = (VisualModel)formater.Deserialize(str);
                }
            } 
            catch
            {
                MessageBox.Show("File coudn't be opened", "Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// writing to the disk
        /// </summary>
        /// <param name="l">Gives acces to the class VisueelModel</param>
        /// <param name="fm">writing to existing files or creating a new file when not available</param>
        /// <param name="path">Is the path where the files is saved</param>
        public static void Writing_disk(VisualModel l, FileMode fm, string path)
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


    /// <summary>
    /// this class is a constructor for the list
    /// </summary>
    [Serializable]
    public class VisueelNode
    {
        public int prioriteit, index;
        public Point point;
        public string name_id, country;
        public Color Color = Color.Gray;
        public bool paint = true;
        public bool dummynode = false;
        public bool priorityLinks = false;
        public int numberOfLinks = 0;
        public VisueelNode(Point point, string name_id, int prioriteit)
        { 
            this.point = point;
            this.name_id = name_id;
            this.prioriteit = prioriteit;
        }       
    }


    /// <summary>
    /// this class is a constructor for the list
    /// </summary>
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


    /// <summary>
    /// this class is a constructor for the list
    /// </summary>
    [Serializable]
    public class VLogicalLink
    {
        public VisueelNode u, v;
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisualLink> links = new List<VisualLink>();
        public VLogicalLink(VisueelNode U, VisueelNode V)
        {
            u = U;
            v = V;
        }

        /// <summary>
        /// if ther exists a dummy node in a route, this method finds the dummy node and the connecting stations
        /// </summary>
        /// <param name="l">List with links from the class VisualLink</param>
        public void GetLinks(List<VisualLink> l)
        {
            List<VisueelNode> booltest = nodes;
            booltest.Add(u);
            booltest.Add(v);
            for (int i = 0; i < booltest.Count; i++)
            {
                for (int n = i; n < booltest.Count; n++)
                {
                    if (GetLink(booltest[i], booltest[n], l) != null)
                    {
                        links.Add(GetLink(booltest[i], booltest[n], l));
                    }
                }
            }
        }

        /// <summary>
        /// Helping method for GetLinks
        /// </summary>
        /// <param name="u">Acces to the first VisueelNode from VisualLink</param>
        /// <param name="v">Acces to the second VisueelNode from VisualLink</param>
        /// <param name="l">List with VisualLink</param>
        /// <returns></returns>
        private VisualLink GetLink(VisueelNode u, VisueelNode v, List<VisualLink> l)
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
