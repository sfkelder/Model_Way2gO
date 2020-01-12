using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace manderijntje
{
    public partial class Form1 : Form
    {
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

                test end 
            */
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
            detailsUserControl.transfersPanel.Height = detailsUserControl.Height - 174;
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
        private static bool checkLocation(string departureLocation, string destinationLocation, bool checkDeparture, List<Node> nodesList)
        {
            if (departureLocation == destinationLocation)
                return false;

            foreach (Node node in nodesList)
            {
                if (checkDeparture)
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
            if (departureLocation == destinationLocation)
                highlightTextBox(destinationInput, "Destination location can't be the same as the departure location");
            else if (departureLocation == "Departure")
                highlightTextBox(departureInput, "Departure textBox is empty");
            else if (destinationLocation == "Destination")
                highlightTextBox(destinationInput, "Destination textBox is empty");
            else if (!departureBool)
                highlightTextBox(departureInput, "Departure location is wrong");
            else if (!destinationBool)
                highlightTextBox(destinationInput, "Destination location is wrong");
        }

        // Highlight textBox with the error
        private void highlightTextBox(TextBox textbox, string text)
        {
            MessageBox.Show(text + ", please try again", "Something went wrong");
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

        // Round time to minutes that cant be diveded by 5
        private DateTime Round(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        
        // Check if the departure or destination textBox is empty
        private static bool checkIfEmpty(string departureLocation, string destinationLocation)
        {
            if (departureLocation != "Departure" && destinationLocation != "Destination")
                return false;
            return true;
        }

        // If there is no error it will call the "setupTripOptions" method for further setup for displaying some tripOptions
        private void searchButton_Click(object sender, EventArgs e)
        {
            departureLocation = departureInput.Text;
            destinationLocation = destinationInput.Text;
            departureTime = timeInput.Text;
            if (checkLocation(departureLocation, destinationLocation, true, nodeList) &&
                    checkLocation(departureLocation, destinationLocation, false, nodeList))
                setupTripOptions();
            else
                checkFout(departureLocation, destinationLocation,
                    checkLocation(departureLocation, destinationLocation, true, nodeList),
                    checkLocation(departureLocation, destinationLocation, false, nodeList));
        }

        // Shows flowControl with all the possible tripOptions
        public void setupTripOptions()
        {
            show(tripOptionsFlowControl);
            tripOptionsFlowControl.Location = new Point(0, logoHeader.Height);
            clearFlowControl(tripOptionsFlowControl);
            tripOptions.Clear();

            List<string> list = new List<string>();
            list.Add(departureLocation);
            list.Add(destinationLocation);

            // Will crash the build
            // visueelControl.visualcontrol(this.Height, 0, 0, new Point(0, 0), new Point(0, 0), list, true, , );

            chosenTime = Convert.ToDateTime(departureTime);
            tripOptions.Add(Routing.GetRoute(departureLocation, destinationLocation, chosenTime, dataControl.GetDataModel()));
            fillTripOptions(new tripOptionsCell[tripOptions.Count()]);
        }

        // Fills the flowcontrol with the usercontrol called "tripOptionsCell" and gives the needed data to tripOptionsCell.
        private void fillTripOptions(tripOptionsCell[] listItems)
        {
            for (int i = 0; i < tripOptions.Count; i++)
            {

                listItems[i] = new tripOptionsCell(this);
                listItems[i].departureTime = tripOptions[i].startTime.ToShortTimeString();
                listItems[i].destinationTime = tripOptions[i].endTime.ToShortTimeString();
                listItems[i].typeCarrier = tripOptions[i].shortestPath[0].vehicle;
                listItems[i].totalTime = (tripOptions[i].endTime.Subtract(tripOptions[i].startTime)).ToString(@"hh\:mm");
                listItems[i].transferCount = tripOptions[i].transfers.ToString();
                listItems[i].shortestPath = tripOptions[i].shortestPath;

                // Needs to have platform and nameTransport from node
                //listItems[i].platform = reisOpties[i].shortestPath[0].perron;
                //listItems[i].nameTransport = reisOpties[i].shortestPath[0].;

                // Optional to have carrier and busLine from node

                //listItems[i].carrier = reisOpties[i].shortestPath[0].vervoersmiddels;
                //listItems[i].busLine = reisOpties[i].busLijn;

                if (tripOptionsFlowControl.Controls.Count < 0)
                    clearFlowControl(tripOptionsFlowControl);
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
            detailsUserControl.Visible = true;
            if (!detailsControl)
            {
                detailsUserControl.Location = new Point(tripOptionsFlowControl.Location.X + tripOptionsFlowControl.Width, tripOptionsFlowControl.Location.Y);
                show(detailsControl);
            }
            detailsUserControl.transfersPanel.Controls.Clear();
            detailsUserControl.departureTime = tripOptionscell.departureTime;
            detailsUserControl.destinationTime = tripOptionscell.destinationTime;
            detailsUserControl.totalTime = tripOptionscell.totalTime;
            detailsUserControl.transfers = tripOptionscell.transferCount;
            detailsUserControl.platform = tripOptionscell.platform;
            detailsUserControl.shortestPath = tripOptionscell.shortestPath;
        }

        // Fills the flowcontrol with the usercontrol called "tussenstopCell" and gives the needed data to tussenstopCell.
        private void fillTransfersStops(transferCell[] listItems)
        {
            for (int i = 0; i < detailsUserControl.shortestPath.Count; i++)
            {
                listItems[i] = new transferCell();
                listItems[i].stationName = detailsUserControl.shortestPath[i].stationnaam;
                listItems[i].toStation = detailsUserControl.shortestPath[i].routnaam;
                listItems[i].typeTransport = detailsUserControl.shortestPath[i].vehicle;

                // Needs to have platform and departureTime from node

                //listItems[i].platform = detailsUserControl.shortestPath[i].;
                //listItems[i].departureTime = detailsUserControl.shortestPath[i].;

                if (i == 0)
                    listItems[i].first = true;
                else if (i == detailsUserControl.shortestPath.Count - 1)
                    listItems[i].last = true;
                else
                    listItems[i].mid = true;

                if (detailsUserControl.transfersPanel.Controls.Count < 0)
                    clearFlowControl(detailsUserControl);
                else
                    detailsUserControl.transfersPanel.Controls.Add(listItems[i]);
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
                autosuggesInVisible();
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
                autosuggesInVisible();
        }

        // Fills the flowcontrol with the usercontrol called "autoSuggesCell" and gives the needed data to autoSuggesCell.
        public void fillAutosuggestie(autoSuggestionCell[] listItems, bool departureInput, List<autoSuggestionModel> suggestionsList)
        {
            for (int i = 0; i < suggestionsList.Count; i++)
            {
                listItems[i] = new autoSuggestionCell(this);
                listItems[i].stationName = suggestionsList[i].stationName;
                listItems[i].stationType = suggestionsList[i].stationType;
                listItems[i].departureInput = departureInput;
                if (autoSuggestion.autoSuggestFlowControl.Controls.Count < 0)
                    clearFlowControl(autoSuggestion.autoSuggestFlowControl);
                else
                    autoSuggestion.autoSuggestFlowControl.Controls.Add(listItems[i]);
            }
        }

        // Set the autosuggestion userControl on the right Y coordinate en setup the right height of the userControl
        public void setLocationAutosuggestion(int yLocation, int aantalElementen)
        {
            if (aantalElementen >= 5)
                autoSuggestion.Height = 40 * 5;
            else
                autoSuggestion.Height = 40 * aantalElementen;
            autoSuggestion.Location = new Point(autoSuggestion.Location.X, yLocation);
        }

        // Clears flowControl
        public void clearFlowControl(object sender)
        {
            if (sender.Equals(detailsUserControl))
                detailsUserControl.transfersPanel.Controls.Clear();
            else if (sender.Equals(tripOptionsFlowControl))
                tripOptionsFlowControl.Controls.Clear();
            else
                autoSuggestion.autoSuggestFlowControl.Controls.Clear();
        }

        // Shows autosuggestion UserControl
        public void autosuggestVisible()
        {
            autoSuggestion.Visible = true;
        }
        //
        // Removes autosuggestion UserControl
        //
        public void autosuggesInVisible()
        {
            autoSuggestion.Visible = false;
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
                backIcon.Visible = true;
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
            changeInput = true;
            depLocation = departureInput.Text;
            desLocation = destinationInput.Text;
            Color bKleur = departureInput.ForeColor;
            Color eKleur = destinationInput.ForeColor;
            destinationInput.ForeColor = bKleur;
            departureInput.ForeColor = eKleur;

            if (!checkIfEmpty(departureInput.Text, destinationInput.Text))
            { 
                departureInput.Text = desLocation;
                destinationInput.Text = depLocation;
            }
            else
            { 
                if (depLocation != "Departure")
                {
                    destinationInput.Text = depLocation;
                    departureInput.Text = "Departure";
                }
                if (desLocation != "Destination")
                {
                    departureInput.Text = desLocation;
                    destinationInput.Text = "Destination";
                }
            }
            changeInput = false;
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
    }
}


