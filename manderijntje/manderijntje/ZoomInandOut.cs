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

    public partial class ZoomInandOut : UserControl
    {
        MapView map;
        Button zIn, zOut;
        public  TrackBar track;
        public ZoomInandOut(MapView map)
        {
            this.map = map;
            zIn = new Button();
            zOut = new Button();
            track = new TrackBar();
            zIn.Location = new Point(5, 0);
            zOut.Location = new Point(5, 130);
            track.Location = new Point(0, 22);
            zIn.Size = new Size(20, 20);
            zOut.Size = new Size(20, 20);
            track.Size = new Size(5, 108);
            track.Minimum = 1;
            track.Maximum = 9;
            track.Orientation = Orientation.Vertical;
            zIn.Text = "+";
            zOut.Text = "-";
            zIn.UseCompatibleTextRendering = true;
            zOut.UseCompatibleTextRendering = true;
            zIn.Font = new Font("Lucida Console", 10.0f);
            zOut.Font = new Font("Lucida Console", 10.0f);
            this.Controls.Add(zIn);
            this.Controls.Add(zOut);
            this.Controls.Add(track);
            InitializeComponent();

            zIn.Click += zIn_Click;
            zOut.Click += zIn_Click;
            track.Click += zIn_Click;

        }

        public void zIn_Click(object o, EventArgs ea)
        {

            Button clickedButton = o as Button;
            TrackBar trackbar = o as TrackBar;

            if (clickedButton == zIn)
            {
                if(map.zoom < 9)
                {
                    track.Value = map.zoom;
                    map.zoomIn();
                }
             

            }else if(clickedButton == zOut)
            {
                if(map.zoom > 1)
                {
                    track.Value = map.zoom;
                    map.zoomOut();
                }
               
            }
            else if(trackbar == track)
            {
                if(map.zoom < track.Value)
                {
                    map.zoom = track.Value -1;
                    map.zoomIn();
                }
                else if(map.zoom > track.Value)
                {
                    map.zoom = track.Value + 1;
                    map.zoomOut();
                }
               
            }


        }
    }
}
