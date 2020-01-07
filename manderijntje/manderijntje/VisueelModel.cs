﻿using System;
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
        public static class connecties
        {
            private static lists_bewerkingen l = new lists_bewerkingen();
            private static bewerkingen b = new bewerkingen();
            static lists toegang = "";

            //word eenmalig aangeropen bij het opstarten van het programma en kijkt of de benodigde file al bestaat of nog aangemaakt moet worden
            public static void opstarten()
            {
                try
                {
                    files.inlezen(toegang);
                }

                catch (Exception)
                {
                    files.schrijven(toegang);
                }

            }


            //wordt vanuit andere classes aangeroepen en stuurt alles in dit form aan
            public static int visualcontrol(int schermhogte, int factor, int zoomgrote, Point startmouse, Point endmouse, List<string> s, bool stationnamen)
            {
                int number = b.zoom(factor, zoomgrote);

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

                    int numberchange = b.factor(kleinstepunt, grootstepunt, schermhogte, zoomgrote);

                    l.valuenode(toegang, factor, schermhogte, b, startmouse, endmouse, stationnamen, kleinstepunt, grootstepunt, numberchange);

                    return numberchange;
                }
                else
                {
                    l.valuenode(toegang, factor, schermhogte, b, startmouse, endmouse, stationnamen, new Point(0, 0), new Point(0, 0), number);
                }

                return factor;


            }

        }


        /*deze class houd alle lists*/
        [Serializable]
        public class lists
        {
            public List<VisueelNode> nodes = new List<VisueelNode>();
            public List<VisueelLink> links = new List<VisueelLink>();
        }


        /*Deze classe beheert de lijsten*/
        [Serializable]
        public class lists_bewerkingen
        {

            int totverschuivingX, totverschuivingY;


            //set de bool waarde van nodes naar true of false afhankelijk van de ingevoerde data
            public void valuenode(lists toegang, int factor, int schermbrete, bewerkingen b, Point start, Point end, bool stations, Point startpoint, Point endpoint, int number)
            {

                Point verschuiving = b.movemap(start, end);
                totverschuivingX += verschuiving.X;
                totverschuivingY += verschuiving.Y;

                foreach (VisueelNode v in toegang.nodes)
                {
                    if ((v.punt.X - totverschuivingX) > number && (v.punt.X - totverschuivingX) < (schermbrete - number) && (v.punt.Y - totverschuivingY) > number && (v.punt.Y - totverschuivingY) < (schermbrete - number))
                    {
                        if (stations && v.punt.X > startpoint.X && v.punt.X > startpoint.Y && v.punt.X < endpoint.X && v.punt.Y < endpoint.Y)
                        {
                            switching(v, factor);
                        }
                        else
                        {
                            switching(v, factor);
                        }

                    }
                    else
                    {
                        v.paint = false;
                    }
                }
            }


            //methode voor het veranderen van de kleur
            public void kleurverandering(List<Point> l, lists list)
            {
                foreach (Point p in l)
                {
                    list.nodes.Find(item => item.punt == p).kleur = Color.Orange;
                }
            }

            //methode die kijkt welke punten bij welke namen horen
            public Point zoekpunt(string naamstation, lists toegang)
            {
                return toegang.nodes.Find(item => item.name_id == naamstation).punt;
            }

            //hulp methode valuenode
            public void switching(VisueelNode v, int zoom)
            {
                switch (zoom)
                {
                    case 0:
                        v.paint = (v.prioriteit < 5) ? false : true;
                        break;
                    case 1:
                        v.paint = (v.prioriteit < 4) ? false : true;
                        break;
                    case 2:
                        v.paint = (v.prioriteit < 3) ? false : true;
                        break;
                    case 3:
                        v.paint = (v.prioriteit < 2) ? false : true;
                        break;
                    default:
                        break;
                }
            }

        }



        /*Deze classen bevat alle methode die direct of indirect user input nodig hebben en vervolgens worden gebruikt voor een bewerking op de kaart*/
        public class bewerkingen
        {
            //returnt hoe ver er is ingezoomt op de kaart
            public int zoom(int factor, int zoomgrote)
            {
                return factor * zoomgrote;
            }

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

                //stations.kleinste = new Point(Math.Min(station1.X - speling, station2.X - speling), Math.Min(station1.Y - speling, station2.Y - speling));
                //stations.groteste = new Point(Math.Max(station1.X + speling, station2.X + speling), Math.Max(station1.Y + speling, station2.Y + speling));

                return stations;
            }

            //returnt een factor afhankelijk van de geselecteerde stations
            public int factor(Point punt1, Point punt2, int schermhogte, int zoomgrote)
            {
                int x, y;

                x = Math.Abs(punt1.X) + Math.Abs(punt2.X);
                y = Math.Abs(punt1.Y) + Math.Abs(punt2.Y);

                if (x < y)
                {
                    return (schermhogte - y) / zoomgrote;
                }
                else
                {
                    return (schermhogte - x) / zoomgrote;
                }

            }

        }


        /*Deze classen bevat methodes die zorgen voor het inlzen en schrijven van de benodigde files*/
        public class files
        {
            private static string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/appdata.txt";

            //zorgt voor het inlezen van de file
            public static void inlezen(lists l)
            {
                using (Stream str = File.Open(filepath, FileMode.Open))
                {
                    var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    l.nodes = (List<VisueelNode>)formater.Deserialize(str);
                }
            }

            //zorgt voor het schrijven van een file
            public static void schrijven(lists l)
            {
                try
                {
                    using (Stream str = File.Open(filepath, FileMode.Open))
                    {
                        var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        formater.Serialize(str, l.nodes);
                    }
                } 

                catch (Exception)
                {
                    using (Stream str = File.Open(filepath, FileMode.Create))
                    {
                        var formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        formater.Serialize(str, l.nodes);
                    }
                }

            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Part 3: aanspassing verwerken in echte programma


        /*Deze classe is een constructor classe voor een list*/
        [Serializable]
        public class VisueelNode
        {
            public int prioriteit;
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

            public VisueelLink(string name_id)
            {
                // pointer naar de Link in het data model
                //Link dataLink;

                this.name_id = name_id;

            }

        }

}
