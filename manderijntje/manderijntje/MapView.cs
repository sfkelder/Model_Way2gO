using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manderijntje
{
    public partial class MapView : UserControl
    {
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisueelLink> links = new List<VisueelLink>();

        int totverschuivingX, totverschuivingY, zoom = 1, zoomgrote = 50, height, width;
        Point start, end, newEnd;
        private bool startingUp = true, mouseMoved = false;
        connecties Connecties;
       
        

        public MapView(connecties c)
        {
            Connecties = c;
            InitializeComponent();
           
           // Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, nodes);
         
            this.Paint += this.painting;
            
            this.MouseDown += (object o, MouseEventArgs mea) => { if (mea.Button == MouseButtons.Left) start = mea.Location; };
            this.MouseMove += (object o, MouseEventArgs mea) => { if (mea.Button == MouseButtons.Left) end = mea.Location; if (end != newEnd) { mouseMoved = true; } newEnd = end; };
            this.MouseClick += this.onclick;

            
        }
         
        public void setMap(int x, int y)
        {
            height = y;
            width = x;
            nodes.Clear();
            links.Clear();
     
            if (startingUp)
            {
                Connecties.SetSizeMap(width, height);
                startingUp = false; 
            }

            Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, this);  
            Invalidate();
        }
         
        public void onclick(object o, MouseEventArgs ea)
        {         

            if (ea.Button == MouseButtons.Right)
            {
                if(zoom > 0)
                {
                    zoom--;
                    nodes.Clear();
                    links.Clear();
                    Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, this);
                    Invalidate();
                }
               
            }
            else if (ea.Button == MouseButtons.Middle)
            {
                if(zoom < 8)
                {
                    zoom++;
                    nodes.Clear();
                    links.Clear();
                    Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, this);
                    Invalidate();
                }
                                   
            }
            else
            {
                if (mouseMoved)
                {
                    nodes.Clear();
                    links.Clear();
                    Connecties.visualcontrol(width, zoom, zoomgrote, start, end, null, false, this);
                    totverschuivingX += start.X - end.X;
                    totverschuivingY += start.Y - end.Y;
                    mouseMoved = false;
                    Invalidate();
                }
               

            }

            
        }

        public void SacleCoordinates()
        {
            Point[] points = new Point[nodes.Count];
            Console.WriteLine(points.Length);
            for(int i = 0; i < nodes.Count; i++)
            {
                points[i] = nodes[i].punt;
                
            }

            points = coordinates.ScalePointsToSize(points, width, height);

            for(int i = 0; i < points.Length; i++)
            {
                nodes[i].punt = points[i];
            }  

             
        }


        public void painting(object o, PaintEventArgs pea)
        {
            SacleCoordinates();
            Pen blackPen = new Pen(Color.Black, 2);
            

            for (int m = 0; m < nodes.Count; m++)
            {
                if (nodes[m].paint == true && nodes[m].dummynode == false)
                {
                    pea.Graphics.FillRectangle(Brushes.Black, nodes[m].punt.X - totverschuivingX, nodes[m].punt.Y - totverschuivingY, 5, 5);   
                }
            }
            
            for (int n = 0; n < links.Count; n++)
            {
                if (links[n].paint)
                {
                    pea.Graphics.DrawLine(blackPen, new Point(links[n].u.punt.X -totverschuivingX, links[n].u.punt.Y - totverschuivingY), new Point(links[n].v.punt.X - totverschuivingX, links[n].v.punt.Y - totverschuivingY));
                }
            }
            


        }
    }
}

