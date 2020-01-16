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
        Label indexbox;

        public Indexpanel()
        {
           
            this.Paint += this.painting;
            this.BackColor = Color.Orange;
            indexbox = new Label();
            indexbox.Location = new Point(100, 15);
            indexbox.Text = "Index";
            indexbox.Font = new Font("Lucida Console", 13.0f);
            indexbox.ForeColor = Color.Orange;
            indexbox.BackColor = Color.White;
            this.Controls.Add(indexbox);
            InitializeComponent();

    
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        public void painting(object o, PaintEventArgs pea)
        {
            Font font = new Font("Lucida Console", 8.0f);
            Pen blackPen = new Pen(Color.Gray, 3);
            Pen OrangePen = new Pen(Color.Orange, 3);

            pea.Graphics.FillRectangle(Brushes.White, 2, 2, 191, 91);

            pea.Graphics.FillRectangle(Brushes.Gray, 35, 30, 10, 10);
            pea.Graphics.FillRectangle(Brushes.Orange, 35, 45, 10, 10);
            pea.Graphics.DrawLine(blackPen, 20, 65, 45, 65);
            pea.Graphics.DrawLine(OrangePen, 20, 78, 45, 78);

            pea.Graphics.DrawString("station", font, Brushes.Black, new Point(50 , 30));
            pea.Graphics.DrawString("station on Route", font, Brushes.Black, new Point(50, 45));

            pea.Graphics.DrawString("Connection", font, Brushes.Black, new Point(50,61));
            pea.Graphics.DrawString("Route to Travel", font, Brushes.Black, new Point(50, 74));

        }
    }
}
