using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace manderijntje
{
    public partial class Form1 : Form
    {
        //List<reisOpties> reisOpties = new List<reisOpties>();

        DataControl dataControl;
        connecties visueelControl;
        MapView mapView;
        Routing r = new Routing();
        List<Route> tripOptions = new List<Route>();
        List<departureTimeModel> timeList = new List<departureTimeModel>();
        public tripOptionsCell tripOptionscell { get; set; }
        DateTime chosenTime { get; set; }
        string departureLocation, destinationLocation, departureTime, depLocation, desLocation;
        bool inputControl = false, optiesControl = false, detailsControl = false, optionSelected = false, changeInput = false;
        VisueelModel visual = new VisueelModel();

        public Form1()
        {
            InitializeComponent();
            dataControl = new DataControl();
            visueelControl = new connecties(dataControl.GetDataModel());
            mapView = new MapView(visueelControl);
            mapView.BackColor = Color.Blue;
            this.Controls.Add(mapView);
            setupView();
 

            Console.WriteLine("Nodes: " + visual.nodes.Count);
            for (int i = 0; i < visual.nodes.Count; i++)
            {
                Console.WriteLine("Naam: " + visual.nodes[i].name_id);
            }

            //test
            Route route = r.GetRoute("Utrecht Centraal", "Den Haag Centraal", DateTime.Now, dataControl.GetDataModel());
            foreach (Node station in route.shortestPath)
            {
                Console.WriteLine(station.stationnaam);
            }
            Console.ReadLine();
            //test end
        }

        // Calls every method that needs to be called to setup the view Correctly
        private void setupView()
        {
            changeBackIcon(false);
            show(inputPanel);
            hideshowBack();
            fillTimeInput();
            setElement();
            SizeChanged += new EventHandler(screenSizeChanged);
            beginInput.GotFocus += new EventHandler(this.removeText);
            beginInput.LostFocus += new EventHandler(this.addText);
            eindInput.GotFocus += new EventHandler(this.removeText);
            eindInput.LostFocus += new EventHandler(this.addText);
            detailsUserControl.Visible = false;
            beginInput.Text = "Departure";
            beginInput.ForeColor = Color.LightGray;
            eindInput.Text = "Destination";
            eindInput.ForeColor = Color.LightGray;
            tijdInput.DropDownStyle = ComboBoxStyle.DropDownList;
            tijdInput.DataSource = timeList;
            tijdInput.DisplayMember = "departureTime";
        }
        
        // When screenSize is changed, call the method "setElement"
        private void screenSizeChanged(object sender, EventArgs e)
        {
            setElement();
        }

        // Set the locations and size of elements corretly
        private void setElement()
        {
            sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height);
            hideBar.Size = new Size(hideBar.Width, this.Height);
            hideBarOrangePanel.Size = new Size(hideBarOrangePanel.Width, hideBar.Height);
            flowLayoutPanel.Size = new Size(flowLayoutPanel.Width, mapView.Height);
            detailsUserControl.Size = new Size(detailsUserControl.Width, mapView.Height);
            if (Height > 450)
            {
                hideArrowIcon.Location = new Point(hideArrowIcon.Location.X, (hideBar.Height / 2) - (logoHeader.Height));
                inputPanel.Location = new Point(inputPanel.Location.X, hideArrowIcon.Location.Y - (inputPanel.Height / 2));
            }
        }

        // Removes the placeholder text in the right inputBoxes
        private void removeText(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == "Departure" || textbox.Text == "Destination")
            {
                textbox.Text = "";
                textbox.ForeColor = Color.Black;
            }
        }

        // Adds the placeholder tet in the right inputBoxes
        private void addText(object sender, EventArgs e)
        {
            bool departureInputBool = true;
            TextBox textbox = (TextBox)sender;
            sender.Equals(eindInput);
            if (sender.Equals(eindInput))
                departureInputBool = false;

            if (string.IsNullOrWhiteSpace(textbox.Text))
            {
                if (departureInputBool)
                    textbox.Text = "Departure";
                else
                    textbox.Text = "Destination";
                textbox.ForeColor = Color.LightGray;
            }
        }

        // Checks if the departure location is filled in correctly
        private static bool checkDepartureLocation(string departureLocation, List<autoSuggestModel> stationList)
        {
            foreach (autoSuggestModel item in stationList)
            {
                if (item.stationName == departureLocation)
                    return true;
            }
            return false;
        }

        // Checks if the destination location is filled in correctly
        private static bool checkDestinationLocation(string destinationLocation, List<autoSuggestModel> stationList)
        {
            foreach (autoSuggestModel item in stationList)
            {
                if (item.stationName == destinationLocation)
                    return true;
            }
            return false;
        }

        // Checks in which textBox an error is
        private void checkFout(string departureLocation, string destinationLocation, bool departureBool, bool destinationBool)
        {
            if (departureLocation == "Departure")
                highlightTextBox(beginInput, "Departure is empty");
            else if (destinationLocation == "Destination")
                highlightTextBox(eindInput, "Destination is empty");
            else if (!departureBool)
                highlightTextBox(beginInput, "Departure is wrong");
            else if (!destinationBool)
                highlightTextBox(eindInput, "Destination is wrong");
        }

        // Highlight textBox with the error
        private void highlightTextBox(TextBox textbox, string text)
        {
            MessageBox.Show(text + ", Try again", "Something went wrong");
            textbox.ForeColor = Color.Red;
        }

        // Fill the timeInput with generated times
        private void fillTimeInput()
        {
            DateTime previouseMin = Round(DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(5));
            DateTime extraMinutes;
            for (int i = 0; i < 20; i++)
            {
                extraMinutes = previouseMin.AddMinutes(5);
                timeList.Add(new departureTimeModel() { departureTime = extraMinutes.ToString("HH:mm") });
                previouseMin = extraMinutes;
            }
        }

        //
        // Round time to minutes that cant be diveded by 5
        //
        private DateTime Round(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        
        // Check if the departure or destination textBox is empty
        private static bool checkIfEmpty(string departureLocation, string destinationLocation)
        {
            if (departureLocation != "Departure" && destinationLocation != "Destination")
                return true;
            return false;
        }

        // If there is no error it will call the "setupTripOptions" method for further setup for displaying some tripOptions
        private void searchButton_Click(object sender, EventArgs e)
        {
            autoSuggestie autosuggest = new autoSuggestie(this);
            departureLocation = beginInput.Text;
            destinationLocation = eindInput.Text;
            departureTime = tijdInput.Text;
            if (checkDepartureLocation(departureLocation, autosuggest.stationList) &&
                    checkDestinationLocation(destinationLocation, autosuggest.stationList))
            {
                //v.vertrekModel(beginLocatie, eindLocatie, departureTime);
                setupTripOptions();
            }
            else
            {
                checkFout(departureLocation, destinationLocation,
                    checkDepartureLocation(departureLocation, autosuggest.stationList),
                    checkDestinationLocation(destinationLocation, autosuggest.stationList));
            }
        }

        // Shows flowControl with all the possible tripOptions
        public void setupTripOptions()
        {
            show(flowLayoutPanel);
            flowLayoutPanel.Location = new Point(0, logoHeader.Height);
            clearFlowControl(this.flowLayoutPanel);
            tripOptions.Clear();

            List<string> list = new List<string>();
            list.Add(departureLocation);
            list.Add(destinationLocation);
            //visueelControl.visualcontrol(this.Height, 0, 0, new Point(0, 0), new Point(0, 0), list, true, visual.nodes);

            chosenTime = Convert.ToDateTime(departureTime);
            tripOptions.Add(r.GetRoute(departureLocation, destinationLocation, chosenTime, dataControl.GetDataModel()));
            Route route = r.GetRoute(departureLocation, destinationLocation, chosenTime, dataControl.GetDataModel());
            foreach (Node station in route.shortestPath)
            {
                Console.WriteLine(station.stationnaam);
            }
            fillTripOptions(new tripOptionsCell[tripOptions.Count()]);
        }

        // Fills the flowcontrol with the usercontrol called "tripOptionsCell" and gives the needed data to tripOptionsCell.
        private void fillTripOptions(tripOptionsCell[] listItems)
        {
            for (int i = 0; i < tripOptions.Count; i++)
            {

                listItems[i] = new tripOptionsCell(this);
                listItems[i].beginTijd = Convert.ToString(tripOptions[i].startTime.ToShortTimeString());
                listItems[i].eindTijd = Convert.ToString(tripOptions[i].endTime.ToShortTimeString());
                listItems[i].typeVervoer = tripOptions[i].shortestPath[0].vervoersmiddels;
                listItems[i].totaleTijd = Convert.ToString(tripOptions[i].endTime.Subtract(tripOptions[i].startTime));
                listItems[i].aantalOverstappen = tripOptions[i].transfers.ToString();
                listItems[i].shortestPath = tripOptions[i].shortestPath;


                //listItems[i].perron = reisOpties[i].shortestPath[0].perron;
                //listItems[i].naamVervoer = reisOpties[i].shortestPath[0].;

                //listItems[i].vervoerder = reisOpties[i].shortestPath[0].vervoersmiddels;
                //listItems[i].busLijn = reisOpties[i].busLijn;
                //listItems[i].tussenstop = reisOpties[i].tussenstop;
                //listItems[i].orange = false;


                // Test Only

                /*listItems[i] = new Cell(this);
                listItems[i].beginTijd = reisOpties[i].beginTijd;
                listItems[i].eindTijd = reisOpties[i].eindTijd;
                listItems[i].vervoerder = reisOpties[i].vervoerder;
                listItems[i].typeVervoer = reisOpties[i].typeVervoer;
                listItems[i].naamVervoer = reisOpties[i].naamVervoer;
                listItems[i].busLijn = reisOpties[i].busLijn;
                listItems[i].totaleTijd = reisOpties[i].totaleTijd;
                listItems[i].aantalOverstappen = reisOpties[i].aantalOverstappen;
                listItems[i].perron = reisOpties[i].perron;
                listItems[i].tussenstop = reisOpties[i].tussenstop;
                listItems[i].orange = false;*/


                if (flowLayoutPanel.Controls.Count < 0)
                    clearFlowControl(this.flowLayoutPanel);
                else
                    flowLayoutPanel.Controls.Add(listItems[i]);
            }
        }

        // Will call the method for further setup of the TripDetails
        public void setupTripDetails()
        {
            showTripDetails();
            fillTransfersStops(new tussenstopCell[detailsUserControl.shortestPath.Count()]);
        }

        // Gives the right information from the tripOptionscell to the detailsUserControl
        private void showTripDetails()
        {
            this.detailsUserControl.Visible = true;
            if (!detailsControl)
            {
                this.detailsUserControl.Location = new Point(flowLayoutPanel.Location.X + flowLayoutPanel.Width, flowLayoutPanel.Location.Y);
                show(detailsControl);
            }
            this.detailsUserControl.tussenstopsPanel.Controls.Clear();
            this.detailsUserControl.beginTijd = tripOptionscell.beginTijd;
            this.detailsUserControl.eindTijd = tripOptionscell.eindTijd;
            this.detailsUserControl.tussenstop = tripOptionscell.tussenstop;
            this.detailsUserControl.totaleTijd = tripOptionscell.totaleTijd;
            this.detailsUserControl.aantalOverstappen = tripOptionscell.aantalOverstappen;
            this.detailsUserControl.perron = tripOptionscell.perron;
            this.detailsUserControl.shortestPath = tripOptionscell.shortestPath;
        }

        // Fills the flowcontrol with the usercontrol called "tussenstopCell" and gives the needed data to tussenstopCell.
        private void fillTransfersStops(tussenstopCell[] listItems)
        {
            for (int i = 0; i < this.detailsUserControl.shortestPath.Count; i++)
            {
                listItems[i] = new tussenstopCell();
                listItems[i].stationNaam = detailsUserControl.shortestPath[i].stationnaam;
                listItems[i].richting = detailsUserControl.shortestPath[i].routnaam;
                listItems[i].typeVervoer = detailsUserControl.shortestPath[i].vervoersmiddels;

                //listItems[i].perron = detailsUserControl.shortestPath[i].perron;
                //listItems[i].vertrekTijd = detailsUserControl.shortestPath[i].;


                //Test only

                /*listItems[i] = new tussenstopCell();
                listItems[i].vertrekTijd = this.detailsUserControl.tussenstop[i].vertrekTijd;
                listItems[i].stationNaam = this.detailsUserControl.tussenstop[i].station;
                listItems[i].perron = this.detailsUserControl.tussenstop[i].perron;
                listItems[i].richting = this.detailsUserControl.tussenstop[i].richtingVervoer;
                listItems[i].typeVervoer = this.detailsUserControl.tussenstop[i].typeVervoer;*/

                if (i == 0)
                    listItems[i].first = true;
                else if (i == this.detailsUserControl.shortestPath.Count - 1)
                    listItems[i].last = true;
                else
                    listItems[i].mid = true;

                if (this.detailsUserControl.tussenstopsPanel.Controls.Count < 0)
                    clearFlowControl(detailsUserControl);
                else
                    this.detailsUserControl.tussenstopsPanel.Controls.Add(listItems[i]);
            }
        }

        // After departureInput of the user it will show an autosuggestion or removes the autosuggestions
        private void departureInput_TextChanged(object sender, EventArgs e)
        {
            autoSuggestie autosuggest = new autoSuggestie(this);
            if (beginInput.Text != "" && changeInput == false)
            {
                autosuggest.checkInput(beginInput.Text);
                if (autosuggest.suggestionsList.Count > 0)
                {
                    autosuggestVisible();
                    autosuggest.setupSuggesties(sender);
                }
                else
                {
                    autosuggesInVisible();
                    autosuggest.clearAutosuggest();
                }
            }
            else
            {
                autosuggesInVisible();
            }
        }

        // After destinationInput of the user it will show an autosuggestion or removes the autosuggestions
        private void destinationInput_TextChanged(object sender, EventArgs e)
        {
            autoSuggestie autosuggest = new autoSuggestie(this);
            if (eindInput.Text != "" && changeInput == false)
            {
                autosuggest.checkInput(eindInput.Text);
                if (autosuggest.suggestionsList.Count > 0)
                {
                    autosuggestVisible();
                    autosuggest.setupSuggesties(sender);
                }
                else
                {
                    autosuggesInVisible();
                    autosuggest.clearAutosuggest();
                }
            }
            else
            {
                autosuggesInVisible();
            }
        }

        // Fills the flowcontrol with the usercontrol called "autoSuggesCell" and gives the needed data to autoSuggesCell.
        public void fillAutosuggestie(autoSuggesCell[] listItems, bool departureInput, List<autoSuggestModel> suggestionsList)
        {
            for (int i = 0; i < suggestionsList.Count; i++)
            {
                listItems[i] = new autoSuggesCell(this.autoSuggestie1, this);
                listItems[i].stationName = suggestionsList[i].stationName;
                listItems[i].stationType = suggestionsList[i].stationType;
                listItems[i].departureInput = departureInput;
                if (this.autoSuggestie1.autosuggestFlowControl.Controls.Count < 0)
                    clearFlowControl(this.autoSuggestie1.autosuggestFlowControl);
                else
                    this.autoSuggestie1.autosuggestFlowControl.Controls.Add(listItems[i]);
            }
        }

        // Set the autosuggestion userControl on the right Y coordinate en setup the right height of the userControl
        public void setLocationAutosuggestion(int yLocation, int aantalElementen)
        {
            if (aantalElementen >= 5)
            {
                this.autoSuggestie1.Height = 40 * 5;
            }
            else
            {
                this.autoSuggestie1.Height = 40 * aantalElementen;
            }
            this.autoSuggestie1.Location = new Point(autoSuggestie1.Location.X, yLocation);
        }

        // Clears flowControl
        public void clearFlowControl(object sender)
        {
            if (sender.Equals(detailsUserControl))
                this.detailsUserControl.tussenstopsPanel.Controls.Clear();
            else if (sender.Equals(flowLayoutPanel))
                this.flowLayoutPanel.Controls.Clear();
            else
                this.autoSuggestie1.autosuggestFlowControl.Controls.Clear();
        }

        // Shows autosuggestion UserControl
        public void autosuggestVisible()
        {
            this.autoSuggestie1.Visible = true;
        }
        //
        // Removes autosuggestion UserControl
        //
        public void autosuggesInVisible()
        {
            this.autoSuggestie1.Visible = false;
        }

        // Will set the right bools for removal of the userControls
        private void backIcon_Click(object sender, EventArgs e)
        {
            detailsControl = false;
            inputControl = true;
            inputPanel.Visible = true;
            hideshowBack();
        }

        // Will remove the right userControls
        private void hideshowBack()
        {
            if (inputControl)
            {
                logoHeader.Width = 390;
                hideBarLocation(logoHeader.Width, logoHeader.Height);
                backIcon.Visible = false;
                detailsUserControl.Visible = false;
                flowLayoutPanel.Visible = false;
                optionSelected = false;
                optiesControl = false;
            }
            else
            {
                backIcon.Visible = true;
            }
        }

        // Gives the mapVIew the right location and size
        private void sizeMap(int x, int y, int width, int height)
        {
            mapView.Size = new Size(width, height);
            mapView.Location = new Point(x, y);
            mapView.setMap(width, height);
        }

        //
        // Hides active views
        //
        private void hideArrowIcon_Click(object sender, EventArgs e)
        {
            if (inputControl)
            {
                inputPanel.Visible = false;
                hide(inputPanel);
                sizeMap(hideBar.Width, logoHeader.Height, this.Width, this.Height);

            }
            else if (optiesControl)
            {
                flowLayoutPanel.Visible = false;
                hide(flowLayoutPanel);
                sizeMap(hideBar.Width, logoHeader.Height, this.Width, this.Height);

            }
            else if (detailsControl)
            {
                detailsUserControl.Visible = false;
                hide(detailsControl);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height);
            }
            else if (!inputControl && !optiesControl && !detailsControl && !optionSelected)
            {
                inputPanel.Visible = true;
                show(inputPanel);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height);
            }
            else if (!inputControl && !optiesControl && !detailsControl && optionSelected)
            {
                flowLayoutPanel.Visible = true;
                show(flowLayoutPanel);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height);
            }
        }

        // Changed departureInput to Destionation and the otherway around
        private void changeTextImage_Click(object sender, EventArgs e)
        {
            if (checkIfEmpty(beginInput.Text, eindInput.Text))
            {
                changeInput = true;
                depLocation = beginInput.Text;
                desLocation = eindInput.Text;
                Color bKleur = beginInput.ForeColor;
                Color eKleur = eindInput.ForeColor;
                beginInput.Text = desLocation;
                beginInput.ForeColor = eKleur;
                eindInput.Text = depLocation;
                eindInput.ForeColor = bKleur;
                changeInput = false;
            }
        }

        // Shows the right UserControl
        private void show(object sender)
        {
            if (sender.Equals(inputPanel))
            {
                inputPanel.Visible = true;
                inputControl = true;
                optiesControl = false;
                detailsControl = false;
                hideshowBack();
                hideBarLocation(logoHeader.Width, logoHeader.Height);
                changeBackIcon(false);
            }
            else if (sender.Equals(flowLayoutPanel))
            {
                flowLayoutPanel.Visible = true;
                inputControl = false;
                optiesControl = true;
                detailsControl = false;
                hideshowBack();
                hideBarLocation(logoHeader.Width, logoHeader.Height);
                changeBackIcon(false);
            }
            else if (sender.Equals(detailsControl))
            {
                detailsUserControl.Visible = true;
                inputControl = false;
                optiesControl = false;
                detailsControl = true;
                hideshowBack();
                logoHeader.Width = logoHeader.Width + detailsUserControl.Width;
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width, this.Height);
                hideBarLocation(logoHeader.Width, logoHeader.Height);
                changeBackIcon(false);
            }
        }
        
        // Changes the arrow image in the hideBa
        private void changeBackIcon(bool Forward)
        {
            if (Forward)
                hideArrowIcon.Image = Properties.Resources.FowardArrow;
            else
                hideArrowIcon.Image = Properties.Resources.BackwardArrow;
        }

        // Hides userControl or the flowcontrolPanel that needs to not be visbible
        public void hide(object sender)
        {
            if (sender.Equals(inputPanel))
            {
                inputControl = false;
                optiesControl = false;
                detailsControl = false;
                hideBarLocation(0, logoHeader.Height);
                changeBackIcon(true);
            }
            else if (sender.Equals(flowLayoutPanel))
            {
                inputControl = false;
                optiesControl = false;
                detailsControl = false;
                optionSelected = true;
                inputPanel.Visible = false;
                changeBackIcon(true);
                hideBarLocation(0, logoHeader.Height);
            }
            else if (sender.Equals(detailsControl))
            {
                inputControl = false;
                optiesControl = true;
                detailsControl = false;
                hideBarLocation(logoHeader.Width, logoHeader.Height);
                logoHeader.Width = 390;
                show(flowLayoutPanel);
                changeBackIcon(false);
            }
        }

        // Set Location of the hideBar
        private void hideBarLocation(int x, int y)
        {
            hideBar.Location = new Point(x - hideBarOrangePanel.Width, y);
            setElement();
        }


        //
        //
        // FAKE DATA
        // FAKE DATA
        // FAKE DATA
        //
        //

        //
        // FAKE DATA
        //
        private List<reisOpties> fakeLijst(DateTime chosenTime)
        {
            List<reisOpties> opties = new List<reisOpties>();
            for (int i = 0; i < 20; i++)
            {
                chosenTime = chosenTime.AddMinutes(6);
                string eindTijd = chosenTime.AddMinutes(19).ToString("HH:mm");
                opties.Add(FakeData(chosenTime.ToString("HH:mm"), eindTijd, "NS", "TREIN", "Intercity", "", "0:19", "0", "1a"));
            }
            return opties;
        }

        //
        // FAKE DATA
        //
        List<tussenStops> l = new List<tussenStops>();
        private reisOpties FakeData(string huidigTijd, string eindTijd,
           string vervoerder, string typeVervoer, string naamVervoer, string busLijn, string totaleTijd, string aantalOverstappen, string perron)
        {
            l.Clear();
            l.Add(fakeTussenStops("Nijmegen", perron, "19:19", "19:20", "Trein", "Arnhem"));
            l.Add(fakeTussenStops("Nijmegen", perron, "19:20", "19:35", "Trein", "Ede-Wageningen"));
            l.Add(fakeTussenStops("Nijmegen", perron, "19:35", "19:50", "Trein", "Driebergen-Zeist"));
            l.Add(fakeTussenStops("Nijmegen", perron, "19:35", "20:05", "Bus", "Utrecht Centraal"));
            l.Add(fakeTussenStops("Nijmegen", perron, "20:12", "20:20", "", "Oorsprong-Park"));
            reisOpties r = new reisOpties(huidigTijd, eindTijd, vervoerder, typeVervoer, naamVervoer, busLijn,
                    totaleTijd, aantalOverstappen, perron, l, false);
            return r;
        }

        //
        // FAKE DATA
        //
        private tussenStops fakeTussenStops(string station, string perron, string aankomstTijd, string departureTime,
           string typeVervoer, string richtingVervoer)
        {
            tussenStops t = new tussenStops(station, perron, aankomstTijd, departureTime, typeVervoer, richtingVervoer);
            return t;
        }
    }
}


