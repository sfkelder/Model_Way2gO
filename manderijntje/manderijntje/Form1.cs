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
        List<Route> tripOptions = new List<Route>();
        List<Node> nodeList = new List<Node>();
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
            mapView.mapView = mapView;
            mapView.BackColor = Color.Blue;
            this.Controls.Add(mapView);
            setupView();

          

            //test
            Route route = Routing.GetRoute("Utrecht Centraal", "Gouda", DateTime.Now, dataControl.GetDataModel());
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
            DataModel datamodel = dataControl.GetDataModel();
            nodeList = datamodel.nodesrouting;
            SizeChanged += new EventHandler(screenSizeChanged);
            departureInput.GotFocus += new EventHandler(this.removeText);
            departureInput.LostFocus += new EventHandler(this.addText);
            destinationInput.GotFocus += new EventHandler(this.removeText);
            destinationInput.LostFocus += new EventHandler(this.addText);
            detailsUserControl.Visible = false;
            departureInput.Text = "Departure";
            departureInput.ForeColor = Color.LightGray;
            destinationInput.Text = "Destination";
            destinationInput.ForeColor = Color.LightGray;
            timeInput.DropDownStyle = ComboBoxStyle.DropDownList;
            timeInput.DataSource = timeList;
            timeInput.DisplayMember = "departureTime";
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
            tripOptionsFlowControl.Size = new Size(tripOptionsFlowControl.Width, mapView.Height);
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
            sender.Equals(destinationInput);
            if (sender.Equals(destinationInput))
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

        // Checks if the location is filled in correctly and exist
        private static bool checkLocation(string departureLocation, string destinationLocation, List<Node> nodesList)
        {
            foreach (Node node in nodesList)
            {
                if (departureLocation.Length != 0)
                {
                    if (node.stationnaam == departureLocation)
                        return true;
                }
                else
                {
                    if (node.stationnaam == destinationLocation)
                        return true;
                }
            }
            return false;
        }

        // Checks in which textBox an error is
        private void checkFout(string departureLocation, string destinationLocation, bool departureBool, bool destinationBool)
        {
            if (departureLocation == "Departure")
                highlightTextBox(departureInput, "Departure is empty");
            else if (destinationLocation == "Destination")
                highlightTextBox(destinationInput, "Destination is empty");
            else if (!departureBool)
                highlightTextBox(departureInput, "Departure is wrong");
            else if (!destinationBool)
                highlightTextBox(destinationInput, "Destination is wrong");
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
            departureLocation = departureInput.Text;
            destinationLocation = destinationInput.Text;
            departureTime = timeInput.Text;
            if (checkLocation(departureLocation, "", nodeList) &&
                    checkLocation("", destinationLocation, nodeList))
            {
                //v.vertrekModel(beginLocatie, eindLocatie, departureTime);
                setupTripOptions();
            }
            else
            {
                checkFout(departureLocation, destinationLocation,
                    checkLocation(departureLocation, "", nodeList),
                    checkLocation("", destinationLocation, nodeList));
            }
        }

        // Shows flowControl with all the possible tripOptions
        public void setupTripOptions()
        {
            show(tripOptionsFlowControl);
            tripOptionsFlowControl.Location = new Point(0, logoHeader.Height);
            clearFlowControl(this.tripOptionsFlowControl);
            tripOptions.Clear();

            List<string> list = new List<string>();
            list.Add(departureLocation);
            list.Add(destinationLocation);
            //visueelControl.visualcontrol(this.Height, 0, 0, new Point(0, 0), new Point(0, 0), list, true, visual.nodes, null);

            chosenTime = Convert.ToDateTime(departureTime);
            tripOptions.Add(Routing.GetRoute(departureLocation, destinationLocation, chosenTime, dataControl.GetDataModel()));
            Route route = Routing.GetRoute(departureLocation, destinationLocation, chosenTime, dataControl.GetDataModel());
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


                if (tripOptionsFlowControl.Controls.Count < 0)
                    clearFlowControl(this.tripOptionsFlowControl);
                else
                    tripOptionsFlowControl.Controls.Add(listItems[i]);
            }
        }

        // Will call the method for further setup of the TripDetails
        public void setupTripDetails()
        {
            showTripDetails();
            fillTransfersStops(new transferCell[detailsUserControl.shortestPath.Count()]);
        }

        // Gives the right information from the tripOptionscell to the detailsUserControl
        private void showTripDetails()
        {
            this.detailsUserControl.Visible = true;
            if (!detailsControl)
            {
                this.detailsUserControl.Location = new Point(tripOptionsFlowControl.Location.X + tripOptionsFlowControl.Width, tripOptionsFlowControl.Location.Y);
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
        private void fillTransfersStops(transferCell[] listItems)
        {
            for (int i = 0; i < this.detailsUserControl.shortestPath.Count; i++)
            {
                listItems[i] = new transferCell();
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
            autoSuggestion autosuggest = new autoSuggestion(this);
            autosuggest.setList(nodeList);
            if (departureInput.Text != "" && changeInput == false)
            {
                autosuggest.checkInput(departureInput.Text);
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
            autoSuggestion autosuggest = new autoSuggestion(this);
            autosuggest.setList(nodeList);
            if (destinationInput.Text != "" && changeInput == false)
            {
                autosuggest.checkInput(destinationInput.Text);
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
        public void fillAutosuggestie(autoSuggestionCell[] listItems, bool departureInput, List<autoSuggestionModel> suggestionsList)
        {
            for (int i = 0; i < suggestionsList.Count; i++)
            {
                listItems[i] = new autoSuggestionCell(this.autoSuggestion, this);
                listItems[i].stationName = suggestionsList[i].stationName;
                listItems[i].stationType = suggestionsList[i].stationType;
                listItems[i].departureInput = departureInput;
                if (this.autoSuggestion.autosuggestFlowControl.Controls.Count < 0)
                    clearFlowControl(this.autoSuggestion.autosuggestFlowControl);
                else
                    this.autoSuggestion.autosuggestFlowControl.Controls.Add(listItems[i]);
            }
        }

        // Set the autosuggestion userControl on the right Y coordinate en setup the right height of the userControl
        public void setLocationAutosuggestion(int yLocation, int aantalElementen)
        {
            if (aantalElementen >= 5)
            {
                this.autoSuggestion.Height = 40 * 5;
            }
            else
            {
                this.autoSuggestion.Height = 40 * aantalElementen;
            }
            this.autoSuggestion.Location = new Point(autoSuggestion.Location.X, yLocation);
        }

        // Clears flowControl
        public void clearFlowControl(object sender)
        {
            if (sender.Equals(detailsUserControl))
                this.detailsUserControl.tussenstopsPanel.Controls.Clear();
            else if (sender.Equals(tripOptionsFlowControl))
                this.tripOptionsFlowControl.Controls.Clear();
            else
                this.autoSuggestion.autosuggestFlowControl.Controls.Clear();
        }

        // Shows autosuggestion UserControl
        public void autosuggestVisible()
        {
            this.autoSuggestion.Visible = true;
        }
        //
        // Removes autosuggestion UserControl
        //
        public void autosuggesInVisible()
        {
            this.autoSuggestion.Visible = false;
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
                tripOptionsFlowControl.Visible = false;
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
                tripOptionsFlowControl.Visible = false;
                hide(tripOptionsFlowControl);
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
                tripOptionsFlowControl.Visible = true;
                show(tripOptionsFlowControl);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height);
            }
        }

        // Changed departureInput to Destionation and the otherway around
        private void changeTextImage_Click(object sender, EventArgs e)
        {
            if (checkIfEmpty(departureInput.Text, destinationInput.Text))
            {
                changeInput = true;
                depLocation = departureInput.Text;
                desLocation = destinationInput.Text;
                Color bKleur = departureInput.ForeColor;
                Color eKleur = destinationInput.ForeColor;
                departureInput.Text = desLocation;
                departureInput.ForeColor = eKleur;
                destinationInput.Text = depLocation;
                destinationInput.ForeColor = bKleur;
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
            else if (sender.Equals(tripOptionsFlowControl))
            {
                tripOptionsFlowControl.Visible = true;
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
            else if (sender.Equals(tripOptionsFlowControl))
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
                show(tripOptionsFlowControl);
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
        private List<tripOptionsModel> fakeLijst(DateTime chosenTime)
        {
            List<tripOptionsModel> opties = new List<tripOptionsModel>();
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
        List<transferModel> l = new List<transferModel>();
        private tripOptionsModel FakeData(string huidigTijd, string eindTijd,
           string vervoerder, string typeVervoer, string naamVervoer, string busLijn, string totaleTijd, string aantalOverstappen, string perron)
        {
            l.Clear();
            l.Add(fakeTussenStops("Nijmegen", perron, "19:19", "19:20", "Trein", "Arnhem"));
            l.Add(fakeTussenStops("Nijmegen", perron, "19:20", "19:35", "Trein", "Ede-Wageningen"));
            l.Add(fakeTussenStops("Nijmegen", perron, "19:35", "19:50", "Trein", "Driebergen-Zeist"));
            l.Add(fakeTussenStops("Nijmegen", perron, "19:35", "20:05", "Bus", "Utrecht Centraal"));
            l.Add(fakeTussenStops("Nijmegen", perron, "20:12", "20:20", "", "Oorsprong-Park"));
            tripOptionsModel r = new tripOptionsModel(huidigTijd, eindTijd, vervoerder, typeVervoer, naamVervoer, busLijn,
                    totaleTijd, aantalOverstappen, perron, l, false);
            return r;
        }

        //
        // FAKE DATA
        //
        private transferModel fakeTussenStops(string station, string perron, string aankomstTijd, string departureTime,
           string typeVervoer, string richtingVervoer)
        {
            transferModel t = new transferModel(station, perron, aankomstTijd, departureTime, typeVervoer, richtingVervoer);
            return t;
        }
    }
}


