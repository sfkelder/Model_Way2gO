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
        public List<VisualLink> links = new List<VisualLink>();

        int totverschuivingX, totverschuivingY, zoom = 1, height, width;
        Point start, end, newEnd;
        private bool startingUp = true, mouseMoved = false;
        Connecion_to_files _connecionToFiles;
        public MapView mapView;
        

        public MapView(Connecion_to_files c)
        {
            _connecionToFiles = c;
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
                _connecionToFiles.SetSizeMap(width, height);
                startingUp = false; 
            }

            _connecionToFiles.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);  
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

                    _connecionToFiles.SetSizeMap(width*zoom, height*zoom);

                    _connecionToFiles.l.changez(zoom, width, height);
                    totverschuivingX +=  ((width / 2) * zoom) - ((width / 2) * (zoom + 1));
                    totverschuivingY +=  ((height / 2) * zoom) - ((height / 2) * (zoom + 1));



                    _connecionToFiles.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);

                 

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

                    _connecionToFiles.l.change(zoom, width, height);
                    totverschuivingX += ((width/2) *(zoom - 1)) - ((width/2)*(zoom-2));
                    totverschuivingY += ((height/2) * (zoom - 1)) - ((height/2) * (zoom-2));


                    _connecionToFiles.SetSizeMap(width*zoom, height*zoom);
                    _connecionToFiles.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);
                   
                    Invalidate();
                }
                                   
            }
            else
            {
                if (mouseMoved)
                {
                    nodes.Clear();
                    links.Clear();
                    _connecionToFiles.visualcontrol(width, zoom, start, end, null, false, mapView);
                    totverschuivingX += start.X - end.X;
                    totverschuivingY += start.Y - end.Y;
                    mouseMoved = false;
                    Invalidate();
                }
               

            }
            

        }

              
        

        public void painting(object o, PaintEventArgs pea)
        {
           Font font = new Font("Times New Roman", 12.0f);

            for (int m = 0; m < nodes.Count; m++)
            { 
                SolidBrush brush = new SolidBrush(nodes[m].Color);

                if (nodes[m].paint == true && nodes[m].dummynode == false) 
                {
                    pea.Graphics.FillRectangle(brush, nodes[m].point.X - totverschuivingX, nodes[m].point.Y - totverschuivingY, 7, 7);
                    //pea.Graphics.DrawString(nodes[m].name_id, font, brush, (float)nodes[m].point.X - (float)totverschuivingX, (float)nodes[m].point.Y - (float)totverschuivingY);
                }
                if (nodes[m].paint == true && nodes[m].dummynode == true)
                {
                    pea.Graphics.FillRectangle(Brushes.Red, nodes[m].point.X - totverschuivingX, nodes[m].point.Y - totverschuivingY, 7, 7);
                    //pea.Graphics.DrawString(nodes[m].name_id, font, brush, (float)nodes[m].point.X - (float)totverschuivingX, (float)nodes[m].point.Y - (float)totverschuivingY);
                }
            }

            for (int n = 0; n < links.Count; n++) 
            {

                

                if (links[n].paint && links[n].kleur == Color.Orange)
                {
                    Pen blackPen = new Pen(links[n].kleur, 3);
                    pea.Graphics.DrawLine(blackPen, new Point(links[n].u.point.X -totverschuivingX + 1, links[n].u.point.Y - totverschuivingY + 1), new Point(links[n].v.point.X - totverschuivingX + 1, links[n].v.point.Y - totverschuivingY + 1));
                }
                else
                {
                    Pen blackPen = new Pen(links[n].kleur, 1);
                    pea.Graphics.DrawLine(blackPen, new Point(links[n].u.point.X - totverschuivingX + 3, links[n].u.point.Y - totverschuivingY + 3), new Point(links[n].v.point.X - totverschuivingX + 3, links[n].v.point.Y - totverschuivingY + 3));                 
                }
            }

        }
    }
}

