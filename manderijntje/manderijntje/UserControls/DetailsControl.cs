﻿using System;
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
    public partial class DetailsControl : UserControl
    {
        private string _departureTime;
        private string _destinationTime;
        private string _totalTime;
        private string _transfers;
        private string _platform;
        private List<Node> _shortestPath;

        public string departureTime
        {
            get { return _departureTime; }
            set { _departureTime = value; }
        }

        public string destinationTime
        {
            get { return _destinationTime; }
            set { _destinationTime = value; timesLBL.Text = _departureTime + " - " + value; }
        }

        public string totalTime
        {
            get { return _totalTime; }
            set { _totalTime = value; totaltimeLBL.Text = value;
            }
        }

        public string transfers
        {
            get { return _transfers; }
            set { _transfers = value; transfersLBL.Text = value + "x"; }
        }

        public string platform
        {
            get { return _platform; }
            set { _platform = value; platformLBL.Text = value; }
        }

        public List<Node> shortestPath
        {
            get { return _shortestPath; }
            set { _shortestPath = value; }
        }

        public DetailsControl()
        {
            InitializeComponent();
        }
    }
}
