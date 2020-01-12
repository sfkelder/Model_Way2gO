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

    /*Deze class bevat alle methodes die de connectie zijn tussen de visuele classes (alles in deze file) en de andere classes*/
    public class connecties
    {
        public lists_bewerkingen l = new lists_bewerkingen();
        private bewerkingen b = new bewerkingen();
        public VisueelModel toegang;
        private const string filepath = "C:/Way2Go/visueelmodel_binary.txt";        

        public connecties(DataModel data)
        {
            if (File.Exists(filepath) && !Program.reimport)
            {
                files.inlezen(this, filepath);
            }
            else
            {
                toegang = (new parsing(data)).getModel(false);

                if (Directory.Exists(filepath))
                {
                    files.schrijven(toegang, FileMode.Open, filepath);
                }
                else
                {
                    files.schrijven(toegang, FileMode.Create, filepath);
                }
            }
        }

        public void SetSizeMap(int width, int height)
        {
            Point[] points = new Point[1000];

            for (int i = 0; i < toegang.nodes.Count; i++)
            {
                try
                {
                    points[i] = toegang.nodes[i].punt;
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
                    toegang.nodes[i].punt = points[i];
                }
                catch (Exception)
                {
                    break;
                }
            }

        }

        //wordt vanuit andere classes aangeroepen en stuurt alles in dit form aan
        public int visualcontrol(int schermhogte, int factor, int zoomgrote, Point startmouse, Point endmouse, List<string> s, bool stationnamen, List<VisueelNode> n, List<VisueelLink> links)
        {
            

            if (stationnamen)
            {
                List<Point> punten = new List<Point>();

                foreach (string st in s)
                {
                    Point T = l.zoekpunt(st, toegang);
                    punten.Add(T);
                }

                Point kleinstepunt = b.getpoints(punten).kleinste;
                Point grootstepunt = b.getpoints(punten).groteste;

                l.valuenode(toegang, factor, schermhogte, b, startmouse, endmouse, stationnamen, kleinstepunt, grootstepunt, map);

                l.valuenode(toegang, factor, schermhogte, b, startmouse, endmouse, stationnamen, kleinstepunt, grootstepunt, numberchange, n, links);

                return numberchange;
            }
            else
            {
                l.valuenode(toegang, factor, schermhogte, b, startmouse, endmouse, stationnamen, new Point(0, 0), new Point(0, 0), number, n, links);
            }


        }

    }


    /*deze class houd alle lists*/
    [Serializable]
    public class VisueelModel
    {
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisueelLink> links = new List<VisueelLink>();
    }


    /*Deze classe beheert de lijsten*/
    [Serializable]
    public class lists_bewerkingen
    {
        
        int totverschuivingX, totverschuivingY;

        public void aanpassenz(int factor, int width, int height)
        {

            totverschuivingX += ((width / 2) * factor) - ((width / 2) * (factor + 1));
            totverschuivingY += ((height / 2) * factor) - ((height / 2) * (factor + 1));
        }
        public void aanpassen(int factor, int width, int height)
        {

            totverschuivingX += ((width/2) * (factor - 1)) - ((width/2) * (factor - 2));
            totverschuivingY += ((height/2) * (factor - 1)) - ((height/2) * (factor-2));
        }

        //set de bool waarde van nodes naar true of false afhankelijk van de ingevoerde data
        public void valuenode(VisueelModel toegang, int factor, int schermbrete, bewerkingen b, Point start, Point end, bool stations, Point startpoint, Point endpoint, int number, List<VisueelNode> n, List<VisueelLink> links)
        {

            Point verschuiving = b.movemap(start, end);
            totverschuivingX += verschuiving.X;
            totverschuivingY += verschuiving.Y;


            foreach (VisueelNode v in toegang.nodes)
            {
                if ((v.punt.X - totverschuivingX) > 0 && (v.punt.X - totverschuivingX) < (schermbrete) && (v.punt.Y - totverschuivingY) > 0 && (v.punt.Y - totverschuivingY) < (schermbrete))
                {
                    if (stations && v.punt.X > startpoint.X && v.punt.X > startpoint.Y && v.punt.X < endpoint.X && v.punt.Y < endpoint.Y)
                    {
                        switching(v, factor, n);
                    }
                    else
                    {
                        switching(v, factor, n);
                    }

                }
                else
                {
                    v.paint = false;
                }
            }
            
           foreach(VisueelLink v in toegang.links)
            {
               if (v.u.paint && v.v.paint)
                    links.Add(v);
            }
           
        }


        //methode voor het veranderen van de kleur
        public void kleurverandering(List<Point> l, VisueelModel list)
        {
            foreach (Point p in l)
            {
                list.nodes.Find(item => item.punt == p).kleur = Color.Orange;
            }
        }

        //methode die kijkt welke punten bij welke namen horen
        public Point zoekpunt(string naamstation, VisueelModel toegang)
        {
            return toegang.nodes.Find(item => item.name_id == naamstation).punt;
        }

        //hulp methode valuenode
        public void switching(VisueelNode v, int zoom, List<VisueelNode> n)
        {
            //List<VisueelNode> nodes = new List<VisueelNode>();
            switch (zoom)
            {
                case 0:
                   // v.paint = (v.prioriteit < 5) ? false : true;
                    v.paint = true;
                    if (v.paint) n.Add(v);

                    break;
                case 1:
                    // v.paint = (v.prioriteit < 4) ? false : true;
                    v.paint = true;
                    if (v.paint) n.Add(v);
                    break;
                case 2:
                    //v.paint = (v.prioriteit < 3) ? false : true;
                    v.paint = true;
                    if (v.paint) n.Add(v);
                    break;
                case 3:
                    // v.paint = (v.prioriteit < 2) ? false : true;
                    v.paint = true;
                    if (v.paint) n.Add(v);
                    break;
                default:
                    v.paint = true;
                    if (v.paint) n.Add(v);
                    break;
            }
        }

    }



    /*Deze classen bevat alle methode die direct of indirect user input nodig hebben en vervolgens worden gebruikt voor een bewerking op de kaart*/
    public class bewerkingen
    {

        //returnt verschijving over de x en y richting in de vorm van een punt
        public Point movemap(Point startmouse, Point endmouse)
        {
            return new Point(startmouse.X - endmouse.X, startmouse.Y - endmouse.Y);
        }

        //wordt gebrukt om twee punten te kunnen returnen in getzoom
        public struct zoomopsations
        {
            public Point groteste, kleinste;
        }

        //returnt coordinat meet kleinste x,y en eentje met grotste x,y
        public zoomopsations getpoints(List<Point> p)
        {
            zoomopsations stations = new zoomopsations();
            int speling = 15;

            stations.kleinste = new Point(p.Min(Point => Point.X) - speling, p.Min(Point => Point.Y) - speling);
            stations.groteste = new Point(p.Max(Point => Point.X) + speling, p.Max(Point => Point.Y) + speling);

            return stations;
        }

        public void LargeandSmalPoints(VisueelModel v)
        {
            Point minX, minY, maxX, MaxY;
           
 
            
           
                
        }
        
    }


    /*Deze classen bevat methodes die zorgen voor het inlzen en schrijven van de benodigde files*/
    public class files
    {
        //zorgt voor het inlezen van de file
        public static void inlezen(connecties c, string path)
        {
            try
            {
                using (Stream str = File.Open(path, FileMode.Open))
                {
                    var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    c.toegang = (VisueelModel)formater.Deserialize(str);
                }
            }
            catch
            {
                MessageBox.Show("File coudn't be opened", "Error", MessageBoxButtons.OK);
            }
        }

        //zorgt voor het schrijven van een file
        public static void schrijven(VisueelModel l, FileMode fm, string path)
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


    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Part 3: aanspassing verwerken in echte programma


    /*Deze classe is een constructor classe voor een list*/
    [Serializable]
    public class VisueelNode
    {
        public int prioriteit, index;
        public Point punt;
        public string name_id;
        public Color kleur = Color.Gray;
        public bool paint = true;
        public bool dummynode = false;

        public VisueelNode(Point punt, string name_id, int prioriteit)
        {
            // pointer naar de Node in het data model
            //Node dataNode;


            this.punt = punt;
            this.name_id = name_id;
            this.prioriteit = prioriteit;

        }

    }



    /*Deze classe is een constructor classe voor een list*/
    [Serializable]
    public class VisueelLink
    {
        public string name_id;
        public Color kleur = Color.Gray;
        public bool paint = true;
        public VisueelNode u, v;

        public VisueelLink(string name_id)
        {
            // pointer naar de Link in het data model
            //Link dataLink;

            this.name_id = name_id;

        }

    }

}
