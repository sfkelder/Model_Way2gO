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
        List<VisueelNode> nodes;

        int totverschuivingX, totverschuivingY, zoom = 0, zoomgrote = 50, height = 0, width = 0;
        Point start, end;
        connecties Connecties;


        public MapView()
        {
            InitializeComponent();
            Size = new Size(width, height);
            BackColor = Color.Blue;
            this.Paint += this.painting;
            this.MouseClick += this.onclick;
            this.MouseDown += (object o, MouseEventArgs mea) => { start = mea.Location; };
            this.MouseUp += (object o, MouseEventArgs mea) => { end = mea.Location; };
        }

        public void GetVisueel(connecties c)
        {
            Connecties = c;
        }

        public void setMap(int x, int y)
        {
            height = y;
            width = x;
            Invalidate();
        }

        public void onclick(object o, MouseEventArgs ea)
        {
            if (ea.Button == MouseButtons.Right)
            {
                zoom--;

                Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false);
            }
            else if (ea.Button == MouseButtons.Middle)
            {
                zoom++;

                Connecties.visualcontrol(width, zoom, zoomgrote, new Point(0, 0), new Point(0, 0), null, false);
            }
            else
            {
                Connecties.visualcontrol(width, zoom, zoomgrote, start, end, null, false);
                totverschuivingX += start.X - end.X;
                totverschuivingY += start.Y - end.Y;

            }
        }
         

        public void getsublist(List<VisueelNode> p)
        {
            nodes = new List<VisueelNode>();

            foreach (VisueelNode point in p)
            {
                nodes.Add(point);
            }

        }

        public void painting(object o, PaintEventArgs pea)
        {
            for (int m = 0; m < nodes.Count - 1; m++)
            {
                if (nodes[m].paint == true)
                {
                    pea.Graphics.FillRectangle(Brushes.Black, nodes[m].punt.X - totverschuivingX, nodes[m].punt.Y - totverschuivingY, 10, 10);
                }
            }
        }
    }
}

