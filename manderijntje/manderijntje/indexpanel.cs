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
        //Label indexbox;

        public Indexpanel()
        {
            /*
            this.Paint += this.painting; 
            this.BackColor = Color.FromArgb(255, 122, 0);
            indexbox = new Label();
            indexbox.Location = new Point(100, 15);
            indexbox.Text = "Index";
            indexbox.Font = new Font("Lucida Console", 13.0f);
            indexbox.ForeColor = Color.FromArgb(255, 122, 0);
            indexbox.BackColor = Color.White;
            this.Controls.Add(indexbox);
            */
            InitializeComponent();
        }

        public void painting(object o, PaintEventArgs pea)
        {
            /*Pen blackPen = new Pen(Color.Gray, 3);
            Pen OrangePen = new Pen(Color.FromArgb(255, 122, 0), 3);
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 122, 0));

            pea.Graphics.FillRectangle(Brushes.Gray, indexLBL.Location.X, stationLBL.Location.Y, 10, 10);
            pea.Graphics.FillRectangle(brush, indexLBL.Location.X, stationOnRouteLBL.Location.Y, 10, 10);

            pea.Graphics.DrawLine(blackPen, indexLBL.Location.X, connectionLBL.Location.Y, 45, 65);
            pea.Graphics.DrawLine(OrangePen, indexLBL.Location.X, connectionLBL.Location.Y, 45, 78);
            */
        }
    }
}
