using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class MapView : UserControl
    {
        public List<VisueelNode> nodes = new List<VisueelNode>();
        public List<VisualLink> links = new List<VisualLink>();
        public List<VLogicalLink> logicallinks = new List<VLogicalLink>();

        public  int totMoveX = -150, totMoveY = 50, zoom = 1, height, width;
        Point start, end, newEnd;
        private bool startingUp = true, mouseMoved = false;
        Connecion_to_files _connecionToFiles;
        public ZoomInandOut zoomInOut;
        public MapView mapView;
        Bitmap bitmap1;
        PictureBox picbox1; 
        public string station1, station2;

        /// <summary>
        /// constructor method
        /// </summary>
        /// <param name="c"> sets connection to Visueel Model</param>
        public MapView(Connecion_to_files c)
        {
            _connecionToFiles = c;
            InitializeComponent();
            picbox1 = new PictureBox();
            Controls.Add(picbox1);
            
            this.MouseWheel += new MouseEventHandler(MoveMouseWheel); ;
            picbox1.MouseDown += (object o, MouseEventArgs mea) => { if (mea.Button == MouseButtons.Left) start = mea.Location; };
            picbox1.MouseMove += (object o, MouseEventArgs mea) => { if (mea.Button == MouseButtons.Left) end = mea.Location; if (end != newEnd) { mouseMoved = true;  } Onclick(); start = end; };
            picbox1.MouseUp += (object o, MouseEventArgs mea) => { end = mea.Location; mouseMoved = false; newEnd = end; };
        }

        /// <summary>
        /// when mousewheel is moved this will result in a zoom
        /// </summary>
        /// <param name="o">object</param>
        /// <param name="mea">MouseEventArgs</param>
        public void MoveMouseWheel(object o, MouseEventArgs mea)
        {          
            if(mea.Delta < 0)
            {
                if(zoom > 1)
                    ZoomOut();
            }
            else
            {   
                if(zoom < 9)
                    ZoomIn();
            }
        }

        /// <summary>
        /// this method is used to set de neccesary values when the size of the map is changed within the program
        /// </summary>
        /// <param name="x">sets new width of the map</param>
        /// <param name="y">sets new height of the map</param>
        public void SetMap(int x, int y)
        {
            height = y;
            width = x;
            nodes.Clear();
            links.Clear();
     
            if (startingUp)
            {
                _connecionToFiles.SetSizeMap(width, height);
                _connecionToFiles.CountConnections();
                _connecionToFiles.Colorchange();
                startingUp = false; 
            }

            try
            {
                bitmap1 = new Bitmap(x, y);
                picbox1.Size = new Size(x, y);
                _connecionToFiles.Visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);
                CreatingBitmap();
            }
            catch(Exception)
            {

            }          
        }

        /// <summary>
        /// this mathod takes care for moving around over the map, there fore it first clears the lists and than fills it with the new nodes and links
        /// </summary>
        public void Onclick()
        {
            if (mouseMoved)
            {
                nodes.Clear();
                links.Clear();
           
                totMoveX += start.X - end.X;
                totMoveY += start.Y - end.Y;

                _connecionToFiles.Visualcontrol(width, zoom, start, end, null, false, mapView);

                mouseMoved = false;

                CreatingBitmap();
            }
        }


        /// <summary>
        /// method for zooming in
        /// </summary>
        public void ZoomIn()
        {
            zoom++;

            _connecionToFiles.l.Change(zoom, width, height);
            totMoveX += ((width / 2) * (zoom - 1)) - ((width / 2) * (zoom - 2));
            totMoveY += ((height / 2) * (zoom - 1)) - ((height / 2) * (zoom - 2));

            ZoomingBoth();
        }

        /// <summary>
        /// method for zooming out
        /// </summary>
        public void ZoomOut()
        {
            zoom--;

            _connecionToFiles.l.Changez(zoom, width, height);
            totMoveX += ((width / 2) * zoom) - ((width / 2) * (zoom + 1));
            totMoveY += ((height / 2) * zoom) - ((height / 2) * (zoom + 1));

            ZoomingBoth();
        }

        /// <summary>
        /// hulp method for zooming in and out
        /// </summary>
        public void ZoomingBoth()
        {
            nodes.Clear();
            links.Clear();

            _connecionToFiles.SetSizeMap(width * zoom, height * zoom);

            _connecionToFiles.Visualcontrol(width, zoom, new Point(0, 0), new Point(0, 0), null, false, mapView);
            zoomInOut.track.Value = zoom;
            CreatingBitmap();
        }


        /// <summary>
        /// Creates the bitmap and sets all points and links
        /// </summary>
        public void CreatingBitmap()
        {           
            Graphics g = Graphics.FromImage(bitmap1);

            g.FillRectangle(Brushes.White, 0, 0, picbox1.Width, picbox1.Height);

            Font font = new Font("Times New Roman", 12.0f);
           
            for (int n = 0; n < links.Count; n++)
            {
                if (links[n].paint && links[n].kleur == Color.Orange)
                {
                    Pen blackPen = new Pen(Color.FromArgb(255, 122, 0), 3);
                    g.DrawLine(blackPen, new Point(links[n].u.point.X - totMoveX + 2, links[n].u.point.Y - totMoveY + 2), new Point(links[n].v.point.X - totMoveX + 2, links[n].v.point.Y - totMoveY + 2));
                }
                else
                {
                    Pen blackPen = new Pen(links[n].kleur, 1);
                    g.DrawLine(blackPen, new Point(links[n].u.point.X - totMoveX + 3, links[n].u.point.Y - totMoveY + 3), new Point(links[n].v.point.X - totMoveX + 3, links[n].v.point.Y - totMoveY + 3));
                }
            }
             
            for (int m = 0; m < nodes.Count; m++) 
            {
                SolidBrush brush = new SolidBrush(nodes[m].Color);

                if (nodes[m].paint && nodes[m].dummynode == false)
                {
                    g.FillRectangle(brush, nodes[m].point.X - totMoveX, nodes[m].point.Y - totMoveY, 7, 7);

                    if ( nodes[m].priorityLinks)
                    {
                        g.DrawString(nodes[m].name_id, font, brush, (float)nodes[m].point.X - (float)totMoveX, (float)nodes[m].point.Y - (float)totMoveY);
                    }
                }

                picbox1.Image = bitmap1;
            }
        }
    }
}

