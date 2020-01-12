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
        List<VisueelLink> links = new List<VisueelLink>();

        int totverschuivingX, totverschuivingY, zoom = 1, height, width;
        Point start, end, newEnd;
        private bool startingUp = true, mouseMoved = false;
        connecties Connecties;
        public MapView mapView;
        

        public MapView(connecties c)
        {
            Connecties = c;
            InitializeComponent();
           
         
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

            Connecties.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);  
            Invalidate();
        }
         
        public void onclick(object o, MouseEventArgs ea)
        {         

            if (ea.Button == MouseButtons.Right)
            {
                if(zoom > 1)
                {
                    zoom--;
                    nodes.Clear();
                    links.Clear();

                    Connecties.SetSizeMap(width*zoom, height*zoom);

                    Connecties.l.aanpassenz(zoom, width, height);
                    totverschuivingX +=  ((width / 2) * zoom) - ((width / 2) * (zoom + 1));
                    totverschuivingY +=  ((height / 2) * zoom) - ((height / 2) * (zoom + 1));



                    Connecties.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);

                 

                    Invalidate();
                }
               
            }
            else if (ea.Button == MouseButtons.Middle)
            {
                if(zoom < 9)
                {
                    zoom++;

                    nodes.Clear();
                    links.Clear();

                    Connecties.l.aanpassen(zoom, width, height);
                    totverschuivingX += ((width/2) *(zoom - 1)) - ((width/2)*(zoom-2));
                    totverschuivingY += ((height/2) * (zoom - 1)) - ((height/2) * (zoom-2));


                    Connecties.SetSizeMap(width*zoom, height*zoom);
                    Connecties.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);
                   
                    Invalidate();
                }
                                   
            }
            else
            {
                if (mouseMoved)
                {
                    nodes.Clear();
                    links.Clear();
                    Connecties.visualcontrol(width, zoom, start, end, null, false, mapView);
                    totverschuivingX += start.X - end.X;
                    totverschuivingY += start.Y - end.Y;
                    mouseMoved = false;
                    Invalidate();
                }
               

            }
            

        }

              
        

        public void painting(object o, PaintEventArgs pea)
        {
                 

            for (int m = 0; m < nodes.Count - 1; m++)
            { 
                SolidBrush brush = new SolidBrush(nodes[m].kleur);

                if (nodes[m].paint == true && nodes[m].dummynode == false)
                {
                    pea.Graphics.FillRectangle(Brushes.Black, nodes[m].punt.X - totverschuivingX, nodes[m].punt.Y - totverschuivingY, 5, 5);   
                }
            }

            for (int n = 0; n < links.Count - 1; n++) 
            {
                Pen blackPen = new Pen(links[n].kleur, 1);

                if (links[n].paint)
                {
                    pea.Graphics.DrawLine(blackPen, new Point(links[n].u.punt.X -totverschuivingX + 2, links[n].u.punt.Y - totverschuivingY + 2), new Point(links[n].v.punt.X - totverschuivingX + 2, links[n].v.punt.Y - totverschuivingY + 2));
                }
            }



        }
    }
}

