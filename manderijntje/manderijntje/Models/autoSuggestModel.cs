﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manderijntje
{
    public class autoSuggestModel
    {
        public string stationName { get; set; }
        public string stationType { get; set; }

        public autoSuggestModel(string stationName, string stationType)
        {
            this.stationName = stationName;
            this.stationType = stationType;
        }
    }
}
