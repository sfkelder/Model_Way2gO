using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manderijntje
{

    public partial class ZoomInandOut : UserControl
    {
        MapView map;
        Button zIn, zOut;
        public  TrackBar track;

        /// <summary>
        /// Constructor method ZoomInandOut
        /// </summary>
        /// <param name="map">Acces to the MapView file</param>
        public ZoomInandOut(MapView map)
        {
            this.map = map;
           
            zIn = new Button();
            zIn.Location = new Point(5, 0);
            zIn.Size = new Size(20, 20);
            zIn.Text = "+";
            zIn.UseCompatibleTextRendering = true;
            zIn.Font = new Font("Lucida Console", 10.0f);
            this.Controls.Add(zIn);

            zOut = new Button();
            zOut.Location = new Point(5, 130);
            zOut.Size = new Size(20, 20);
            zOut.Text = "-";
            zOut.UseCompatibleTextRendering = true;
            zOut.Font = new Font("Lucida Console", 10.0f);
            this.Controls.Add(zOut);

            track = new TrackBar();
            track.Location = new Point(0, 22);          
            track.Size = new Size(5, 108);
            track.Minimum = 1;
            track.Maximum = 9;
            track.Orientation = Orientation.Vertical;
            this.Controls.Add(track);

            InitializeComponent();

            zIn.Click += zIn_Click;
            zOut.Click += zIn_Click;
            track.Click += zIn_Click;
        }

        /// <summary>
        /// When the slider is moved or a button is clicked this method will run
        /// </summary>
        /// <param name="o">object</param>
        /// <param name="ea">EvantArgs</param>
        public void zIn_Click(object o, EventArgs ea)
        {

            Button clickedButton = o as Button;
            TrackBar trackbar = o as TrackBar;

            if (clickedButton == zIn)
            {
                if(map.zoom < 9)
                {
                    track.Value = map.zoom;
                    map.ZoomIn();
                }
             
                 
            }else if(clickedButton == zOut)
            {
                if(map.zoom > 1)
                {
                    track.Value = map.zoom;
                    map.ZoomOut();
                }
               
            }
            else if(trackbar == track)
            {
                if(map.zoom < track.Value)
                {
                    map.zoom = track.Value -1;
                    map.ZoomIn();
                }
                else if(map.zoom > track.Value)
                {
                    map.zoom = track.Value + 1;
                    map.ZoomOut();
                }
               
            }


        }
    }
}
