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
        List<VisueelNode> nodes = new List<VisueelNode>();

        int totverschuivingX, totverschuivingY, zoom = 0, zoomgrote = 50, height = 0, width = 0;
        Point start, end;
        connecties Connecties;


        public MapView(connecties c)
        {
            Connecties = c;
            InitializeComponent();
            Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, nodes);
            BackColor = Color.Blue;
            this.Paint += this.painting;
            this.MouseClick += this.onclick;
            this.MouseDown += (object o, MouseEventArgs mea) => { start = mea.Location; };
            this.MouseUp += (object o, MouseEventArgs mea) => { end = mea.Location; };
        }
         
        public void setMap(int x, int y)
        {
            height = y;
            width = x;
            nodes.Clear();
            Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, nodes);
            Invalidate();
        }
         
        public void onclick(object o, MouseEventArgs ea)
        {         

            if (ea.Button == MouseButtons.Right)
            {
                zoom--;

                nodes.Clear();
                Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false, nodes);
            }
            else if (ea.Button == MouseButtons.Middle)
            {
                zoom++;

                nodes.Clear();
                Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false,  nodes);
            }
            else
            {
                nodes.Clear();
                Connecties.visualcontrol(width, zoom, zoomgrote, start, end, null, false, nodes);
                totverschuivingX += start.X - end.X;
                totverschuivingY += start.Y - end.Y;

            }

            Invalidate();
        }

        public void SacleCoordinates()
        {
            Point[] points = new Point[200];
            
            for(int i = 0; i < nodes.Count - 1; i++)
            {
                try
                {
                    points[i] = nodes[i].punt;
                }
                catch (Exception)
                {
                    break;
                }
                
            }

            points = coordinates.ScalePointsToSize(points, width, height);

            for(int i = 0; i < points.Length - 1 ; i++)
            {
                try
                {
                    nodes[i].punt = points[i];
                }
                catch (Exception)
                {
                    break;
                }
            }

            
        }

        public void painting(object o, PaintEventArgs pea)
        {
             SacleCoordinates();

            //pea.Graphics.FillRectangle(Brushes.Black, 30 - totverschuivingX, 30 - totverschuivingY, 10, 10);

            for (int m = 0; m < nodes.Count - 1; m++)
            {
                if (nodes[m].paint == true)
                {
                    pea.Graphics.FillRectangle(Brushes.Black, nodes[m].punt.X - totverschuivingX, nodes[m].punt.Y - totverschuivingY, 10, 10);   
                }
            }

            //nodes.Clear();
        }
    }
}

