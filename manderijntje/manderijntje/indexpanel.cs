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
    public partial class Indexpanel : UserControl
    {
        Bitmap bitmap;
        PictureBox picbox;

        //constructor method
        public Indexpanel()
        {                  
            this.BackColor = Color.FromArgb(255, 122, 0);
            picbox = new PictureBox();
            picbox.Size = new Size(300, 200);
            bitmap = new Bitmap(200, 100);
            
            Controls.Add(picbox);
            painting();

            InitializeComponent();  
        }

        //painting event index
        public void painting()
        {
            Graphics g = Graphics.FromImage(bitmap);

            Font font = new Font("Lucida Console", 8.0f);
            Font font1 = new Font("Lucida Console", 13.0f);
            Pen blackPen = new Pen(Color.Gray, 3);
            Pen OrangePen = new Pen(Color.FromArgb(255, 122, 0), 3);
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 122, 0));

            g.FillRectangle(Brushes.White, 2, 2, 191, 91);

            g.DrawString("Index", font1, brush, new Point(65, 8));

            g.FillRectangle(Brushes.Gray, 35, 30, 10, 10);
            g.FillRectangle(brush, 35, 45, 10, 10);
            g.DrawLine(blackPen, 20, 65, 45, 65);
            g.DrawLine(OrangePen, 20, 78, 45, 78);

            g.DrawString("station", font, Brushes.Black, new Point(50 , 30));
            g.DrawString("station on Route", font, Brushes.Black, new Point(50, 45));

            g.DrawString("Connection", font, Brushes.Black, new Point(50,61));
            g.DrawString("Route to Travel", font, Brushes.Black, new Point(50, 74));


            picbox.Image = bitmap;

        }
    }
}
