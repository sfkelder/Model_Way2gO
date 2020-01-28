using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manderijntje
{
    public partial class Form1 : Form
    {
        DataControl dataControl;
        Connecion_to_files visueelControl;
        MapView mapView;
        IndexPanel indexPanel;
        ZoomInandOut zoomInandOut;
        List<Route> tripOptions = new List<Route>();
        List<Node> nodeList = new List<Node>();
        List<DepartureTimeModel> timeList = new List<DepartureTimeModel>();
        List<AutoSuggestionModel> backupList = new List<AutoSuggestionModel>();
        public TripOptionsCell tripOptionscell { get; set; }
        DateTime chosenTime { get; set; }
        string departureLocation, destinationLocation, departureTime, depLocation, desLocation;
        int biggestLBL, biggestLBLIndex;
        bool inputControl = false, optiesControl = false, detailsControl = false, optionSelected = false, changeInput = false;

        public Form1()
        {
            InitializeComponent();
            SetupView();
        }

        // Calls every method that needs to be called to setup the view correctly.
        private void SetupView()
        {
            dataControl = new DataControl();
            visueelControl = new Connecion_to_files(dataControl.GetDataModel());
            mapView = new MapView(visueelControl);
            indexPanel = new IndexPanel();
            zoomInandOut = new ZoomInandOut(mapView);
            mapView.mapView = mapView;
            mapView.zoomInOut = zoomInandOut;
            this.Controls.Add(indexPanel);
            this.Controls.Add(zoomInandOut);
            this.Controls.Add(mapView);
            WindowState = FormWindowState.Maximized;
            ChangeBackIcon(false);
            ShowUserControl(inputPanel);
            HideshowBack();
            FillTimeInput();
            SetElement();
            DataModel datamodel = dataControl.GetDataModel();
            nodeList = datamodel.nodes;
            SizeChanged += new EventHandler(ScreenSizeChanged);
            departureInput.GotFocus += new EventHandler(this.RemoveText);
            departureInput.LostFocus += new EventHandler(this.AddText);
            destinationInput.GotFocus += new EventHandler(this.RemoveText);
            destinationInput.LostFocus += new EventHandler(this.AddText);
            detailsUserControl.Visible = false;
            departureInput.Text = "Departure";
            departureInput.ForeColor = Color.LightGray;
            destinationInput.Text = "Destination";
            destinationInput.ForeColor = Color.LightGray;
            timeInput.DropDownStyle = ComboBoxStyle.DropDownList;
            timeInput.DataSource = timeList;
            timeInput.DisplayMember = "departureTime";
        }

        // When screenSize is changed, call the method "setElement".
        private void ScreenSizeChanged(object sender, EventArgs e)
        {
            SetElement();
        }

        // Set the locations and size of elements correctly.
        private void SetElement()
        {
            SizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width - (2 * hideBar.Width), this.Height - (logoHeader.Height));
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

        // Removes the placeholder text in the right inputBoxes.
        private void RemoveText(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == "Departure" || textbox.Text == "Destination")
            {
                textbox.Text = "";
                textbox.ForeColor = Color.Black;
            }
        }

        // Adds the placeholder text in the correct textBoxes.
        private void AddText(object sender, EventArgs e)
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

        // Checks if the location is filled in correctly and exist.
        private static bool CheckLocation(string departureLocation, string destinationLocation, bool checkDeparture, List<Node> nodesList)
        {
            if (departureLocation == destinationLocation)
                return false;

            foreach (Node node in nodesList)
            {
                if (checkDeparture)
                {
                    if (node.stationName == departureLocation)
                        return true;
                }
                else
                {
                    if (node.stationName == destinationLocation)
                        return true;
                }
            }
            return false;
        }

        // Checks in which textBox there an error occurred and calls the method highlightTextBox.
        private void CheckFout(string departureLocation, string destinationLocation, bool departureBool, bool destinationBool)
        {
            if (departureLocation == destinationLocation)
                HighlightTextBox(destinationInput, "Destination location can't be the same as the departure location");
            else if (departureLocation == "Departure")
                HighlightTextBox(departureInput, "Departure textBox is empty");
            else if (destinationLocation == "Destination")
                HighlightTextBox(destinationInput, "Destination textBox is empty");
            else if (!departureBool)
                HighlightTextBox(departureInput, "Departure location is wrong");
            else if (!destinationBool)
                HighlightTextBox(destinationInput, "Destination location is wrong");
        }

        // Highlight textBox with the error and show a messageBox with the correct text.
        private void HighlightTextBox(TextBox textbox, string text)
        {
            MessageBox.Show(text + ", please try again", "Something went wrong");
            textbox.ForeColor = Color.Red;
        }

        // Generates 20 times and add them in the timeList.
        private void FillTimeInput()
        {
            DateTime previouseMin = Round(DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(5));
            DateTime extraMinutes;
            for (int i = 0; i < 20; i++)
            {
                extraMinutes = previouseMin.AddMinutes(5);
                timeList.Add(new DepartureTimeModel() { departureTime = extraMinutes.ToString("HH:mm") });
                previouseMin = extraMinutes;
            }
        }

        // Round a DateTime to a DateTime with minutes that you can divide with 5.
        private DateTime Round(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        // Check if the departure or destination textBox is empty.
        private static bool CheckIfEmpty(string departureLocation, string destinationLocation)
        {
            if (departureLocation != "Departure" && destinationLocation != "Destination")
                return false;
            return true;
        }

        // If there is no error it will call the "setupTripOptions" method for further setup for displaying tripOptions.
        private void searchButtonClick(object sender, EventArgs e)
        {
            departureLocation = departureInput.Text;
            destinationLocation = destinationInput.Text;
            departureTime = timeInput.Text;
            if (CheckLocation(departureLocation, destinationLocation, true, nodeList) &&
                 CheckLocation(departureLocation, destinationLocation, false, nodeList))
                SetupTripOptions();
            else
                CheckFout(departureLocation, destinationLocation,
                    CheckLocation(departureLocation, destinationLocation, true, nodeList),
                    CheckLocation(departureLocation, destinationLocation, false, nodeList));
        }

        // Shows the tripOptionsFlowControl with all the possible tripOptions.
        public void SetupTripOptions()
        {
            ShowUserControl(tripOptionsFlowControl);
            tripOptionsFlowControl.Location = new Point(0, logoHeader.Height);
            ClearFlowControl(tripOptionsFlowControl);
            tripOptions.Clear();
            chosenTime = Convert.ToDateTime(departureTime);
            foreach (Route route in Routing.GetRoute(departureLocation, destinationLocation, chosenTime,
            dataControl.GetDataModel()))
            {
                tripOptions.Add(route);
            }
            FillTripOptions(new TripOptionsCell[tripOptions.Count()]);
        }

        // Fills the flowcontrol with the usercontrol called "tripOptionsCell" and gives the needed data to tripOptionsCell.
        private void FillTripOptions(TripOptionsCell[] listItems)
        {
            for (int i = 0; i < listItems.Count(); i++)
            {
                listItems[i] = new TripOptionsCell(this);
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
                    ClearFlowControl(tripOptionsFlowControl);
                else
                    tripOptionsFlowControl.Controls.Add(listItems[i]);
            }
        }

        // Will call the method for further setup of the TripDetails.
        public void SetupTripDetails()
        {
            ShowTripDetails();
            FillTransfersStops(new TransferCell[detailsUserControl.shortestPath.Count()]);
        }

        // Gives the right information from the tripOptionscell to the detailsUserControl.
        private void ShowTripDetails()
        {
            detailsUserControl.Visible = true;
            if (!detailsControl)
            {
                detailsUserControl.Location = new Point(tripOptionsFlowControl.Location.X + tripOptionsFlowControl.Width, tripOptionsFlowControl.Location.Y);
                ShowUserControl(detailsControl);
            }
            detailsUserControl.transfersPanel.Controls.Clear();
            detailsUserControl.departureTime = tripOptionscell.departureTime;
            detailsUserControl.destinationTime = tripOptionscell.destinationTime;
            detailsUserControl.totalTime = tripOptionscell.totalTime;
            detailsUserControl.shortestPath = tripOptionscell.shortestPath;

            visueelControl.Visualcontrol(this.Height, 0, new Point(0, 0), new Point(0, 0), tripOptionscell.shortestPath, true, mapView);
            mapView.CreatingBitmap();
        }

        // Fills the tripOptionsFlowControl with the usercontrol called "transferCell" and gives the needed data to transferCell.
        private void FillTransfersStops(TransferCell[] listItems)
        {

            for (int i = 0; i < detailsUserControl.shortestPath.Count; i++)
            {
                listItems[i] = new TransferCell();
                listItems[i].stationName = detailsUserControl.shortestPath[i].stationName;
                listItems[i].typeTransport = "Train";
                listItems[i].departureTime = detailsUserControl.shortestPath[i].minCostToStart.ToShortTimeString();

                if (i == 0)
                    listItems[i].first = true;
                else if (i == detailsUserControl.shortestPath.Count - 1)
                    listItems[i].last = true;
                else
                    listItems[i].mid = true;

                if (detailsUserControl.transfersPanel.Controls.Count < 0)
                    ClearFlowControl(detailsUserControl);
                else
                    detailsUserControl.transfersPanel.Controls.Add(listItems[i]);
            }

        }

        // After departureInput of the user it will show an autosuggestion or removes the autosuggestions.
        private void DepartureInputTextChanged(object sender, EventArgs e)
        {
            AutoSuggestion autosuggest = new AutoSuggestion(this);
            autosuggest.SetList(nodeList);
            departureInput.ForeColor = Color.Black;
            if (departureInput.Text != "" && changeInput == false)
            {
                autosuggest.CheckInput(departureInput.Text);
                if (autosuggest.suggestionsList.Count > 0)
                {
                    backupList = autosuggest.suggestionsList;
                    AutosuggestVisible();
                    autosuggest.SetupSuggestions(sender);
                }
                else
                {
                    // If there are no suggestions show the last suggested autoSuggestion.
                    if (backupList.Count() != 0)
                        autosuggest.ShowBackupList(backupList);
                    else
                    {
                        AutosuggesInVisible();
                        autosuggest.ClearAutosuggest();
                    }
                }
            }
            else
                AutosuggesInVisible();
        }

        // After destinationInput of the user it will show an autosuggestion or removes the autosuggestions.
        private void DestinationInputTextChanged(object sender, EventArgs e)
        {
            AutoSuggestion autosuggest = new AutoSuggestion(this);
            autosuggest.SetList(nodeList);
            destinationInput.ForeColor = Color.Black;
            if (destinationInput.Text != "" && changeInput == false)
            {
                autosuggest.CheckInput(destinationInput.Text);

                if (autosuggest.suggestionsList.Count > 0)
                {
                    backupList = autosuggest.suggestionsList;
                    AutosuggestVisible();
                    autosuggest.SetupSuggestions(sender);
                }
                else
                {
                    // If there are no suggestions show the last suggested autoSuggestion.
                    if (backupList.Count() != 0)
                        autosuggest.ShowBackupList(backupList);
                    else
                    {
                        AutosuggesInVisible();
                        autosuggest.ClearAutosuggest();
                    }
                }
            }
            else
                AutosuggesInVisible();
        }

        // Fills the flowcontrol with the usercontrol called "autoSuggesCell" and gives the needed data to autoSuggesCell.
        public void FillAutosuggestion(AutoSuggestionCell[] listItems, bool departureInput, List<AutoSuggestionModel> suggestionsList)
        {
            // biggestLBL and biggestLBLIndex is needed to autoSize the autoSuggestion with the right width.
            biggestLBL = 0;
            biggestLBLIndex = 0;
            for (int i = 0; i < suggestionsList.Count; i++)
            {
                listItems[i] = new AutoSuggestionCell(this);
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
                    ClearFlowControl(autoSuggestion.autoSuggestFlowControl);
                else
                    autoSuggestion.autoSuggestFlowControl.Controls.Add(listItems[i]);
            }

            // Will give the autoSuggestion the correct width.
            if (listItems[biggestLBLIndex].Width == destinationInput.Width)
                SetSizeAutoSuggestion(destinationInput.Width + 10, autoSuggestion.Height);
            else
                SetSizeAutoSuggestion(biggestLBL + 50, autoSuggestion.Height);
        }

        // Gives autosuggestion proper size.
        private void SetSizeAutoSuggestion(int width, int heigth)
        {
            autoSuggestion.Size = new Size(width, heigth);
            autoSuggestion.autoSuggestFlowControl.Size = new Size(autoSuggestion.Width, autoSuggestion.Height);
        }

        // Set the autosuggestion userControl on the right Y coordinate and will give the autoSuggestion the right height.
        public void SetLocationAutosuggestion(int yLocation, int aantalElementen)
        {
            if (aantalElementen >= 5)
                autoSuggestion.Height = 40 * 5;
            else
                autoSuggestion.Height = 40 * aantalElementen;
            autoSuggestion.Location = new Point(autoSuggestion.Location.X, yLocation);
        }

        // Clears flowControl.
        public void ClearFlowControl(object sender)
        {
            if (sender.Equals(detailsUserControl))
                detailsUserControl.transfersPanel.Controls.Clear();
            else if (sender.Equals(tripOptionsFlowControl))
                tripOptionsFlowControl.Controls.Clear();
            else
                autoSuggestion.autoSuggestFlowControl.Controls.Clear();
        }

        // Shows autosuggestion UserControl.
        public void AutosuggestVisible()
        {
            autoSuggestion.Visible = true;
        }

        // Removes autosuggestion UserControl.
        public void AutosuggesInVisible()
        {
            autoSuggestion.Visible = false;
        }

        // Will set the right bools for removal of the userControls.
        private void BackIconClick(object sender, EventArgs e)
        {
            detailsControl = false;
            inputControl = true;
            inputPanel.Visible = true;
            HideshowBack();
        }

        // Will remove the right userControls.
        private void HideshowBack()
        {
            if (inputControl)
            {
                logoHeader.Width = 390;
                HideBarLocation(logoHeader.Width, logoHeader.Height);
                backIcon.Visible = false;
                detailsUserControl.Visible = false;
                tripOptionsFlowControl.Visible = false;
                optionSelected = false;
                optiesControl = false;
            }
            else
                backIcon.Visible = true;
        }

        // Gives the mapView, indexPanel and zoomInandOut the right location and size.
        private void SizeMap(int x, int y, int width, int height)
        {
            mapView.Size = new Size(width, height);
            mapView.Location = new Point(x, y);
            mapView.SetMap(width, height);
            indexPanel.Size = new Size(274, 125);
            indexPanel.Location = new Point(x, y + height - 195);
            zoomInandOut.Size = new Size(35, 150);
            zoomInandOut.Location = new Point(x, y + 5);
        }

        // Hides active views and set mapView size correct for the new layout.
        private void HideArrowIconClick(object sender, EventArgs e)
        {
            if (inputControl)
            {
                inputPanel.Visible = false;
                HideUserControl(inputPanel);
                SizeMap(hideBar.Width, logoHeader.Height, this.Width, this.Height - logoHeader.Height);

            }
            else if (optiesControl)
            {
                tripOptionsFlowControl.Visible = false;
                HideUserControl(tripOptionsFlowControl);
                SizeMap(hideBar.Width, logoHeader.Height, this.Width, this.Height - logoHeader.Height);

            }
            else if (detailsControl)
            {
                detailsUserControl.Visible = false;
                HideUserControl(detailsControl);
                SizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height - logoHeader.Height);
            }
            else if (!inputControl && !optiesControl && !detailsControl && !optionSelected)
            {
                inputPanel.Visible = true;
                ShowUserControl(inputPanel);
                SizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height - logoHeader.Height);
            }
            else if (!inputControl && !optiesControl && !detailsControl && optionSelected)
            {
                tripOptionsFlowControl.Visible = true;
                ShowUserControl(tripOptionsFlowControl);
                SizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - logoHeader.Width, this.Height - logoHeader.Height);
            }
        }

        // Shows the right UserControl and set the bools.
        private void ShowUserControl(object sender)
        {
            if (sender.Equals(inputPanel))
            {
                inputPanel.Visible = true;
                inputControl = true;
                optiesControl = false;
                detailsControl = false;
                HideshowBack();
                HideBarLocation(logoHeader.Width, logoHeader.Height);
                ChangeBackIcon(false);
            }
            else if (sender.Equals(tripOptionsFlowControl))
            {
                tripOptionsFlowControl.Visible = true;
                inputControl = false;
                optiesControl = true;
                detailsControl = false;
                HideshowBack();
                HideBarLocation(logoHeader.Width, logoHeader.Height);
                ChangeBackIcon(false);
            }
            else if (sender.Equals(detailsControl))
            {
                detailsUserControl.Visible = true;
                inputControl = false;
                optiesControl = false;
                detailsControl = true;
                HideshowBack();
                logoHeader.Width = logoHeader.Width + detailsUserControl.Width;
                SizeMap(logoHeader.Width + hideBar.Width, logoHeader.Height, this.Width - hideBar.Width - logoHeader.Width, this.Height - logoHeader.Height);
                HideBarLocation(logoHeader.Width, logoHeader.Height);
                ChangeBackIcon(false);
            }
        }

        // Changes departureInput to Destionation and the otherway around.
        private void ChangeTextImageClick(object sender, EventArgs e)
        {
            changeInput = true;
            depLocation = departureInput.Text;
            desLocation = destinationInput.Text;
            Color bKleur = departureInput.ForeColor;
            Color eKleur = destinationInput.ForeColor;
            destinationInput.ForeColor = bKleur;
            departureInput.ForeColor = eKleur;

            if (!CheckIfEmpty(departureInput.Text, destinationInput.Text))
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

        // Changes the arrow image in the hideBar.
        private void ChangeBackIcon(bool Forward)
        {
            if (Forward)
                hideArrowIcon.Image = manderijntje.Properties.Resources.FowardArrow;
            else
                hideArrowIcon.Image = manderijntje.Properties.Resources.BackwardArrow;
        }

        // Hides userControl or the flowcontrolPanel that need to be Invisbible and set the correct bools.
        public void HideUserControl(object sender)
        {
            if (sender.Equals(inputPanel))
            {
                inputControl = false;
                optiesControl = false;
                detailsControl = false;
                HideBarLocation(0, logoHeader.Height);
                ChangeBackIcon(true);
            }
            else if (sender.Equals(tripOptionsFlowControl))
            {
                inputControl = false;
                optiesControl = false;
                detailsControl = false;
                optionSelected = true;
                inputPanel.Visible = false;
                ChangeBackIcon(true);
                HideBarLocation(0, logoHeader.Height);
            }
            else if (sender.Equals(detailsControl))
            {
                inputControl = false;
                optiesControl = true;
                detailsControl = false;
                HideBarLocation(logoHeader.Width, logoHeader.Height);
                logoHeader.Width = 390;
                ShowUserControl(tripOptionsFlowControl);
                ChangeBackIcon(false);
            }
        }

        // Set Location of the hideBar.
        private void HideBarLocation(int x, int y)
        {
            hideBar.Location = new Point(x - hideBarOrangePanel.Width, y);
            SetElement();
        }
    }
}


