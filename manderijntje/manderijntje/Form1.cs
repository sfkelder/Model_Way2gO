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
        Connecion_to_files visueelControl;
        MapView mapView;
        Indexpanel indexpanel;
        ZoomInandOut zoomInandOut;
        List<Route> tripOptions = new List<Route>();
        List<Node> nodeList = new List<Node>();
        List<departureTimeModel> timeList = new List<departureTimeModel>();
        List<autoSuggestionModel> backupList = new List<autoSuggestionModel>();
        public tripOptionsCell tripOptionscell { get; set; }
        DateTime chosenTime { get; set; }
        string departureLocation, destinationLocation, departureTime, depLocation, desLocation;
        int biggestLBL, biggestLBLIndex;
        bool inputControl = false, optiesControl = false, detailsControl = false, optionSelected = false, changeInput = false;

        private bool demoDani = false;

        
        public Form1()
        {
            InitializeComponent();
            dataControl = new DataControl();
            visueelControl = new Connecion_to_files(dataControl.GetDataModel());
            mapView = new MapView(visueelControl);
            indexpanel = new Indexpanel();
            zoomInandOut = new ZoomInandOut(mapView);
            mapView.mapView = mapView;
            mapView.zoomInOut = zoomInandOut;
            this.Controls.Add(indexpanel);
            this.Controls.Add(zoomInandOut);
            this.Controls.Add(mapView);
            setupView();

            if (demoDani)
            {
                List<Node> demoNodes = new List<Node>();
                demoNodes.Add(new Node(0.0, 0.0, "Ronald Reagon Washington", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Crystal City", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Pentagon City", "", 0));
                demoNodes.Add(new Node(0.0, 0.0,  "Pentagon", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "L Enfant Plaza", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Waterfront", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Navy Yard Ballpark", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Anacostia", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Congress Heights", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Southern Avenue", "", 0));
                demoNodes.Add(new Node(0.0, 0.0, "Naylor Road", "", 0));
                visueelControl.visualcontrol(this.Height, 0, new Point(0, 0), new Point(0, 0), demoNodes, true, mapView);
            }
        }

        // Calls every method that needs to be called to setup the view Correctly
        private void setupView()
        {
            WindowState = FormWindowState.Maximized;
            changeBackIcon(false);
            show(inputPanel);
            hideshowBack();
            fillTimeInput();
            setElement();
            DataModel datamodel = dataControl.GetDataModel();
            nodeList = datamodel.nodes;
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
            sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width - (2*hideBar.Width), this.Height - (logoHeader.Height));
            hideBar.Size = new Size(hideBar.Width, this.Height);
            hideBarOrangePanel.Size = new Size(hideBarOrangePanel.Width, hideBar.Height);
            tripOptionsFlowControl.Size = new Size(tripOptionsFlowControl.Width, mapView.Height - 20);
            detailsUserControl.Size = new Size(detailsUserControl.Width, mapView.Height - 30);
            detailsUserControl.transfersPanel.Height = detailsUserControl.Height - 83;
            detailsUserControl.transfersPanel.Width = detailsUserControl.Width;
            detailsUserControl.transfersPanel.Location = new Point(detailsUserControl.transfersPanel.Location.X, detailsUserControl.transfersPanel.Location.Y);
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
                    if (node.stationname == departureLocation)
                        return true;
                }
                else
                {
                    if (node.stationname == destinationLocation)
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

            if (demoDani)
            {
                // Demo case
                setupTripOptions();
            }
            else
            {
                if (checkLocation(departureLocation, destinationLocation, true, nodeList) &&
                    checkLocation(departureLocation, destinationLocation, false, nodeList))
                    setupTripOptions();
                else
                    checkFout(departureLocation, destinationLocation,
                        checkLocation(departureLocation, destinationLocation, true, nodeList),
                        checkLocation(departureLocation, destinationLocation, false, nodeList));
            }
        }

        // Shows flowControl with all the possible tripOptions
        public void setupTripOptions()
        {
            show(tripOptionsFlowControl);
            tripOptionsFlowControl.Location = new Point(0, logoHeader.Height);
            clearFlowControl(tripOptionsFlowControl);
            tripOptions.Clear();

            chosenTime = Convert.ToDateTime(departureTime);

            // Demo case
            if (demoDani)
            {
                fillTripOptions(new tripOptionsCell[20]);
            }
            else
            {
                foreach (Route route in Routing.GetRoute(departureLocation, destinationLocation, chosenTime,
                dataControl.GetDataModel()))
                {
                    tripOptions.Add(route);
                }
                fillTripOptions(new tripOptionsCell[tripOptions.Count()]);
            }
        }

        // Fills the flowcontrol with the usercontrol called "tripOptionsCell" and gives the needed data to tripOptionsCell.
        private void fillTripOptions(tripOptionsCell[] listItems)
        {
            // Demo case
            if (demoDani)
            {
                // Demo case
                DateTime departureTime = DateTime.ParseExact("09:33", "hh:mm", System.Globalization.CultureInfo.CurrentCulture);
                DateTime destinationTime = DateTime.ParseExact("10:12", "hh:mm", System.Globalization.CultureInfo.CurrentCulture);
                DateTime totalTime = DateTime.ParseExact("00:39", "hh:mm", System.Globalization.CultureInfo.CurrentCulture);

                for (int i = 0; i < 20; i++)
                {
                    // Demo Case
                    departureTime = departureTime.AddMinutes(12);
                    destinationTime = destinationTime.AddMinutes(12);
                    listItems[i] = new tripOptionsCell(this);
                    listItems[i].departureTime = departureTime.ToShortTimeString();
                    listItems[i].destinationTime = destinationTime.ToShortTimeString();
                    listItems[i].totalTime = totalTime.ToShortTimeString();

                    if (tripOptionsFlowControl.Controls.Count < 0)
                        clearFlowControl(tripOptionsFlowControl);
                    else
                        tripOptionsFlowControl.Controls.Add(listItems[i]);
                }
            }
            else
            {
                for (int i = 0; i < listItems.Count(); i++)
                {
                    listItems[i] = new tripOptionsCell(this);
                    listItems[i].departureTime = tripOptions[i].startTime.ToShortTimeString();
                    listItems[i].destinationTime = tripOptions[i].endTime.ToShortTimeString();
                    listItems[i].nameTransport = "Train";
                    TimeSpan span = tripOptions[i].endTime.Subtract(tripOptions[i].startTime);
                    DateTime timeAdded = tripOptions[i].startTime.AddHours(span.Hours).AddMinutes(span.Minutes).AddDays(span.Days);

                    if (timeAdded.Day > tripOptions[i].startTime.Day)
                    {
                        // If it takes 1 or more days change LBL size, height and span format.
                        if (span.Days == 1)
                        {
                            listItems[i].totaltimeLBL.Location = new Point(260, listItems[i].totaltimeLBL.Location.Y);
                            listItems[i].totaltimeLBL.Size = new Size(70, listItems[i].totaltimeLBL.Height);
                            listItems[i].clockIcon.Location = new Point(listItems[i].totaltimeLBL.Location.X - 23, listItems[i].clockIcon.Location.Y);
                            listItems[i].totalTime = span.ToString(@"dd").TrimStart(' ', 'd', 'h', 'm', 's', '0') + "d " + span.ToString(@"hh").TrimStart(' ', 'd', 'h', 'm', 's') + "h " + span.ToString(@"mm").TrimStart(' ', 'd', 'h', 'm', 's') + "m";
                            listItems[i].destinationTime = tripOptions[i].endTime.ToShortTimeString() + " next day";
                        }
                        else if (span.Days > 1)
                        {
                            listItems[i].totaltimeLBL.Location = new Point(260, listItems[i].totaltimeLBL.Location.Y);
                            listItems[i].totaltimeLBL.Size = new Size(70, listItems[i].totaltimeLBL.Height);
                            listItems[i].clockIcon.Location = new Point(listItems[i].totaltimeLBL.Location.X - 23, listItems[i].clockIcon.Location.Y);
                            listItems[i].totalTime = span.ToString(@"dd").TrimStart(' ', 'd', 'h', 'm', 's', '0') + "d " + span.ToString(@"hh").TrimStart(' ', 'd', 'h', 'm', 's') + "h " + span.ToString(@"mm").TrimStart(' ', 'd', 'h', 'm', 's') + "m";
                            listItems[i].destinationTime = tripOptions[i].endTime.ToShortTimeString() + " " + span.Days + " days later";
                        }
                        else
                        {
                            listItems[i].destinationTime = tripOptions[i].endTime.ToShortTimeString() + " next day";
                            listItems[i].totalTime = span.ToString(@"hh").TrimStart(' ', 'd', 'h', 'm', 's') + "h " + span.ToString(@"mm").TrimStart(' ', 'd', 'h', 'm', 's') + "m";
                        } 
                    }
                    else
                        listItems[i].totalTime = span.ToString(@"hh").TrimStart(' ', 'd', 'h', 'm', 's') + "h " + span.ToString(@"mm").TrimStart(' ', 'd', 'h', 'm', 's') + "m";


                    listItems[i].shortestPath = tripOptions[i].shortestPath;

                    if (tripOptionsFlowControl.Controls.Count < 0)
                        clearFlowControl(tripOptionsFlowControl);
                    else
                        tripOptionsFlowControl.Controls.Add(listItems[i]);
                }
            } 
        }

        // Will call the method for further setup of the TripDetails
        public void setupTripDetails()
        {
            showTripDetails();
            //Demo case
            if (demoDani)
            {
                fillTransfersStops(new transferCell[11]);
            }
            else
            {
                fillTransfersStops(new transferCell[detailsUserControl.shortestPath.Count()]);
            }
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
            detailsUserControl.shortestPath = tripOptionscell.shortestPath;

            visueelControl.visualcontrol(this.Height, 0, new Point(0, 0), new Point(0, 0), tripOptionscell.shortestPath, true, mapView);
            mapView.painting();
        }

        // Fills the flowcontrol with the usercontrol called "transferCell" and gives the needed data to transferCell.
        private void fillTransfersStops(transferCell[] listItems)
        {
            if (demoDani)
            {
                clearFlowControl(detailsUserControl);

                listItems[0] = new transferCell();
                listItems[0].stationName = "Ronald Reagan Washington";
                listItems[0].typeTransport = "train";
                listItems[0].departureTime = "09:33";
                listItems[0].first = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[0]);

                listItems[1] = new transferCell();
                listItems[1].stationName = "Crystal City Station";
                listItems[1].typeTransport = "train";
                listItems[1].departureTime = "09:36";
                listItems[1].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[1]);

                listItems[2] = new transferCell();
                listItems[2].stationName = "Pentagon City Station";
                listItems[2].typeTransport = "train";
                listItems[2].departureTime = "09:38";
                listItems[2].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[2]);

                listItems[3] = new transferCell();
                listItems[3].stationName = "Pentagon";
                listItems[3].typeTransport = "train";
                listItems[3].departureTime = "09:39";
                listItems[3].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[3]);

                listItems[4] = new transferCell();
                listItems[4].stationName = "L'Enfant Plaza Metro Station";
                listItems[4].typeTransport = "train";
                listItems[4].departureTime = "10:33";
                listItems[4].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[4]);

                listItems[5] = new transferCell();
                listItems[5].stationName = "Waterfront Station";
                listItems[5].typeTransport = "train";
                listItems[5].departureTime = "10:35";
                listItems[5].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[5]);

                listItems[6] = new transferCell();
                listItems[6].stationName = "Navy Yard-Ballpark Station";
                listItems[6].typeTransport = "train";
                listItems[6].departureTime = "10:37";
                listItems[6].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[6]);

                listItems[7] = new transferCell();
                listItems[7].stationName = "Anacostia Station";
                listItems[7].typeTransport = "train";
                listItems[7].departureTime = "10:40";
                listItems[7].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[7]);

                listItems[8] = new transferCell();
                listItems[8].stationName = "Congress Heights Station";
                listItems[8].typeTransport = "train";
                listItems[8].departureTime = "10:43";
                listItems[8].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[8]);

                listItems[9] = new transferCell();
                listItems[9].stationName = "Southern Avenue";
                listItems[9].typeTransport = "train";
                listItems[9].departureTime = "10:45";
                listItems[9].mid = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[9]);

                listItems[10] = new transferCell();
                listItems[10].stationName = "Naylor Road Station";
                listItems[10].typeTransport = "train";
                listItems[10].departureTime = "10:48";
                listItems[10].last = true;
                detailsUserControl.transfersPanel.Controls.Add(listItems[10]);
            }
            else
            {
                for (int i = 0; i < detailsUserControl.shortestPath.Count; i++)
                {
                    listItems[i] = new transferCell();
                    listItems[i].stationName = detailsUserControl.shortestPath[i].stationname;
                    listItems[i].typeTransport = "Train";
                    listItems[i].departureTime = detailsUserControl.shortestPath[i].minCostToStart.ToShortTimeString();

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
        }

        // After departureInput of the user it will show an autosuggestion or removes the autosuggestions
        private void departureInput_TextChanged(object sender, EventArgs e)
        {
            autoSuggestion autosuggest = new autoSuggestion(this);
            autosuggest.setList(nodeList);
            departureInput.ForeColor = Color.Black;
            if (departureInput.Text != "" && changeInput == false)
            {
                autosuggest.checkInput(departureInput.Text);
                if (autosuggest.suggestionsList.Count > 0)
                {
                    backupList = autosuggest.suggestionsList;
                    autosuggestVisible();
                    autosuggest.setupSuggesties(sender);
                }
                else
                {
                    if (backupList.Count() != 0)
                        autosuggest.showBackupList(backupList);
                    else
                    {
                        autosuggesInVisible();
                        autosuggest.clearAutosuggest();
                    }
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
            destinationInput.ForeColor = Color.Black;
            if (destinationInput.Text != "" && changeInput == false)
            {
                autosuggest.checkInput(destinationInput.Text);
                
                if (autosuggest.suggestionsList.Count > 0)
                {
                    backupList = autosuggest.suggestionsList;
                    autosuggestVisible();
                    autosuggest.setupSuggesties(sender);
                }
                else
                {
                    if (backupList.Count() != 0)
                        autosuggest.showBackupList(backupList);
                    else
                    {
                        autosuggesInVisible();
                        autosuggest.clearAutosuggest();
                    }
                }
            }
            else
                autosuggesInVisible();
        }

        // Fills the flowcontrol with the usercontrol called "autoSuggesCell" and gives the needed data to autoSuggesCell.
        public void fillAutosuggestie(autoSuggestionCell[] listItems, bool departureInput, List<autoSuggestionModel> suggestionsList)
        {
            biggestLBL = 0;
            biggestLBLIndex = 0;
            for (int i = 0; i < suggestionsList.Count; i++)
            {
                listItems[i] = new autoSuggestionCell(this);
                listItems[i].stationName = suggestionsList[i].stationName;
                listItems[i].stationType = suggestionsList[i].stationType;
                listItems[i].departureInput = departureInput;

                if (biggestLBL < listItems[i].stationLBL.Width)
                {
                    biggestLBL = listItems[i].stationLBL.Width;
                    biggestLBLIndex = i;
                }

                // Will prevent to display 2 usercontrols on the same height in the autoSuggestion
                if (listItems[i].stationLBL.Width + 25 <= destinationInput.Width)
                    listItems[i].Width = destinationInput.Width;
                else
                    listItems[i].Width = listItems[i].stationLBL.Width + 25;

                if (autoSuggestion.autoSuggestFlowControl.Controls.Count < 0)
                    clearFlowControl(autoSuggestion.autoSuggestFlowControl);
                else
                    autoSuggestion.autoSuggestFlowControl.Controls.Add(listItems[i]);
            }

            if (listItems[biggestLBLIndex].Width == destinationInput.Width)
                setSizeAutoSuggestion(destinationInput.Width + 10, autoSuggestion.Height);
            else
                setSizeAutoSuggestion(biggestLBL + 50, autoSuggestion.Height);
        }

        // Gives autosuggestion proper Size
        private void setSizeAutoSuggestion(int width, int heigth)
        {
            autoSuggestion.Size = new Size(width, heigth);
            autoSuggestion.autoSuggestFlowControl.Size = new Size(autoSuggestion.Width, autoSuggestion.Height);
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
            indexpanel.Size = new Size(274, 125);
            indexpanel.Location = new Point(x, y + height - 195);
            zoomInandOut.Size = new Size(35, 150);
            zoomInandOut.Location = new Point(x, y + 5);

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
                sizeMap(hideBar.Width, logoHeader.Height, this.Width, this.Height - logoHeader.Height);

            }
            else if (optiesControl)
            {
                tripOptionsFlowControl.Visible = false;
                hide(tripOptionsFlowControl);
                sizeMap(hideBar.Width, logoHeader.Height, this.Width, this.Height - logoHeader.Height);

            }
            else if (detailsControl)
            {
                detailsUserControl.Visible = false;
                hide(detailsControl);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height - logoHeader.Height);
            }
            else if (!inputControl && !optiesControl && !detailsControl && !optionSelected)
            {
                inputPanel.Visible = true;
                show(inputPanel);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height - logoHeader.Height);
            }
            else if (!inputControl && !optiesControl && !detailsControl && optionSelected)
            {
                tripOptionsFlowControl.Visible = true;
                show(tripOptionsFlowControl);
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height - logoHeader.Height);
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
                sizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - hideBar.Width - logoHeader.Width, this.Height - logoHeader.Height);
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


