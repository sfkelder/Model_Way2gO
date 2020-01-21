using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace manderijntje
{
    public partial class MapView : UserControl
    {
       public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisualLink> links = new List<VisualLink>();
        public List<vLogicalLink> logicallinks = new List<vLogicalLink>();

        public  int totverschuivingX = 50, totverschuivingY = 50, zoom = 1, height, width;
        Point start, end, newEnd;
        private bool startingUp = true, mouseMoved = false;
        Connecion_to_files _connecionToFiles;
        public ZoomInandOut zoomInOut;
        public MapView mapView;
        Bitmap bitmap1;
        PictureBox picbox1;


        //constructort method
        public MapView(Connecion_to_files c)
        {
            _connecionToFiles = c;
            InitializeComponent();
            picbox1 = new PictureBox();
            Controls.Add(picbox1);
            // this.Paint += this.painting;
            
            this.MouseWheel += new MouseEventHandler(mouseWheel); ;
            picbox1.MouseDown += (object o, MouseEventArgs mea) => { if (mea.Button == MouseButtons.Left) start = mea.Location; };
            picbox1.MouseMove += (object o, MouseEventArgs mea) => { if (mea.Button == MouseButtons.Left) end = mea.Location; if (end != newEnd) { mouseMoved = true;  } onclick(); start = end; };
            picbox1.MouseUp += (object o, MouseEventArgs mea) => { end = mea.Location; mouseMoved = false; newEnd = end; };
        }
        
        //when mousewheel is moved this will result in a zoom
        public void mouseWheel(object o, MouseEventArgs mea)
        {
            
            if(mea.Delta < 0)
            {
                if(zoom > 1)
                    zoomOut();
            }
            else
            {   
                if(zoom < 9)
                    zoomIn();
            }


        }

        //this method is used to set de neccesary values when the size of the map is changed within the program
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

            try
            {
                bitmap1 = new Bitmap(x, y);
                picbox1.Size = new Size(x, y);
                _connecionToFiles.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);
                painting();
            }
            catch(Exception)
            {

            }
             
        }

        //this mathod takes care for moving around over the map
        public void onclick()
        {
            if (mouseMoved)
            {
                nodes.Clear();
                links.Clear();

                if ((totverschuivingX + start.X - end.X) < (250*(zoom-1)) && -(totverschuivingX + start.X - end.X) > (250 * (zoom - 1)))
                {
                    totverschuivingX += start.X - end.X;
                }
              //  totverschuivingX += start.X - end.X;
                totverschuivingY += start.Y - end.Y;

                _connecionToFiles.visualcontrol(width, zoom, start, end, null, false, mapView);

                mouseMoved = false;



                painting();
            }
        }


        //method for zooming in
        public void zoomIn()
        {
            zoom++;

            _connecionToFiles.l.change(zoom, width, height);
            totverschuivingX += ((width / 2) * (zoom - 1)) - ((width / 2) * (zoom - 2));
            totverschuivingY += ((height / 2) * (zoom - 1)) - ((height / 2) * (zoom - 2));

            ZoomingBoth();
        }

        //method for zooming out
        public void zoomOut()
        {
            zoom--;

            _connecionToFiles.l.changez(zoom, width, height);
            totverschuivingX += ((width / 2) * zoom) - ((width / 2) * (zoom + 1));
            totverschuivingY += ((height / 2) * zoom) - ((height / 2) * (zoom + 1));

            ZoomingBoth();
        }

        //hulp method for zooming in and out
        public void ZoomingBoth()
        {
            nodes.Clear();
            links.Clear();

            _connecionToFiles.SetSizeMap(width * zoom, height * zoom);

            _connecionToFiles.visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);
            zoomInOut.track.Value = zoom;
            painting();
        }


        //Painting method
        public void painting()
        {
            
            Graphics g = Graphics.FromImage(bitmap1);

            g.FillRectangle(Brushes.White, 0, 0, picbox1.Width, picbox1.Height);

            Font font = new Font("Times New Roman", 12.0f);
           
            for (int n = 0; n < links.Count; n++)
            {
                if (links[n].paint && links[n].kleur == Color.Orange)
                {
                    Pen blackPen = new Pen(Color.FromArgb(255, 122, 0), 3);
                    g.DrawLine(blackPen, new Point(links[n].u.point.X - totverschuivingX + 2, links[n].u.point.Y - totverschuivingY + 2), new Point(links[n].v.point.X - totverschuivingX + 2, links[n].v.point.Y - totverschuivingY + 2));
                }
                else
                {
                    Pen blackPen = new Pen(links[n].kleur, 1);
                    g.DrawLine(blackPen, new Point(links[n].u.point.X - totverschuivingX + 3, links[n].u.point.Y - totverschuivingY + 3), new Point(links[n].v.point.X - totverschuivingX + 3, links[n].v.point.Y - totverschuivingY + 3));

                }
            }

           /* for (int i = 0; i < logicallinks.Count; i++)
            {
                for (int n = 0; n < logicallinks[i].links.Count; n++)
                {
                    Pen blackPen = new Pen(Color.Orange, 1);
                    g.DrawLine(blackPen, new Point(logicallinks[i].links[n].u.point.X - totverschuivingX + 3, logicallinks[i].links[n].u.point.Y - totverschuivingY + 3), new Point(logicallinks[i].links[n].v.point.X - totverschuivingX + 3, logicallinks[i].links[n].v.point.Y - totverschuivingY + 3));
                }
            }*/
             
            for (int m = 0; m < nodes.Count; m++)
            {
                SolidBrush brush = new SolidBrush(nodes[m].Color);

                if (nodes[m].paint == true && nodes[m].dummynode == false)
                {
                    g.FillRectangle(brush, nodes[m].point.X - totverschuivingX, nodes[m].point.Y - totverschuivingY, 7, 7);
                  //  g.DrawString(nodes[m].name_id, font, brush, (float)nodes[m].point.X - (float)totverschuivingX, (float)nodes[m].point.Y - (float)totverschuivingY);

                    if (nodes[m].name_id == "Ronald Reagon Washington" || nodes[m].name_id == "Naylor Road")
                    {
                        g.DrawString(nodes[m].name_id, font, brush, (float)nodes[m].point.X - (float)totverschuivingX + 3, (float)nodes[m].point.Y - (float)totverschuivingY + 3);
                    }
                }

                /*if (nodes[m].paint == true && nodes[m].dummynode == true)
                {
                    g.FillRectangle(Brushes.Red, nodes[m].point.X - totverschuivingX, nodes[m].point.Y - totverschuivingY, 7, 7);
                    g.DrawString(nodes[m].name_id, font, brush, (float)nodes[m].point.X - (float)totverschuivingX, (float)nodes[m].point.Y - (float)totverschuivingY);
                }*/

                picbox1.Image = bitmap1;
                //bitmap1.Save("button.bmp");
            }
        }
    }
}

