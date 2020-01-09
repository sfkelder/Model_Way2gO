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

        
        List<reisOpties> reisOpties = new List<reisOpties>();
        List<tijdenModel> tijdenList = new List<tijdenModel>();
        Vertrek v = new Vertrek();
        public Cell cell { get; set; }
        DateTime gekozenTijd { get; set; }
        string beginLocatie, eindLocatie, vertrekTijd, bLocatie, eLocatie;
        bool inputControl = false, optiesControl = false, detailsControl = false, optionSelected = false, veranderInput = false;

        public Form1()
        {
            dataControl = new DataControl();
            visueelControl = new connecties(dataControl.GetDataModel());

            InitializeComponent();
            setupView();  
        }

        //
        // ZET HET SCHERM KLAAR
        //
        private void setupView()
        {
            changeBackIcon(false);
            show(inputPanel);
            hideshowBack();
            vultijdInput();
            beginInput.GotFocus += new EventHandler(this.verwijderText);
            beginInput.LostFocus += new EventHandler(this.voegText);
            eindInput.GotFocus += new EventHandler(this.verwijderText);
            eindInput.LostFocus += new EventHandler(this.voegText);
            detailsUserControl.Visible = false;
            beginInput.Text = "Departure";
            beginInput.ForeColor = Color.LightGray;
            eindInput.Text = "Destination";
            eindInput.ForeColor = Color.LightGray;
            tijdInput.DropDownStyle = ComboBoxStyle.DropDownList;
            tijdInput.DataSource = tijdenList;
            tijdInput.DisplayMember = "vertrekTijd";  
        }

        //
        // VERWIJDERD DE PLACEHOLDER TEXT
        //
        private void verwijderText(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == "Departure" || textbox.Text == "Destination")
            {
                textbox.Text = "";
                textbox.ForeColor = Color.Black;
            }     
        }

        //
        // VOEGT TEXT TOE ALS PLACEHOLDER VOOR DE TEXT INPUT
        //
        private void voegText(object sender, EventArgs e)
        {
            bool beginInputBool = true;
            TextBox textbox = (TextBox)sender;
            sender.Equals(eindInput);
            if (sender.Equals(eindInput))
                beginInputBool = false;

            if (string.IsNullOrWhiteSpace(textbox.Text))
            {
                if (beginInputBool)
                    textbox.Text = "Departure";
                else
                    textbox.Text = "Destination";
                textbox.ForeColor = Color.LightGray;
            }
        }

        //
        // KIJKT OF begin locatie (GOED) is INGEVULD
        //

        private static bool checkBeginLocatie(string bLocatie, List<autoSuggestModel> stationList)
        {
            foreach (autoSuggestModel item in stationList)
            {
                if (item.stationNaam == bLocatie)
                    return true;
            }
            return false;
        }

        //
        // KIJKT OF eind locatie (GOED) is INGEVULD
        //
        private static bool checkEindLocatie(string eLocatie, List<autoSuggestModel> stationList)
        {
            foreach (autoSuggestModel item in stationList)
            {
                if (item.stationNaam == eLocatie)
                    return true;
            }
            return false;
        }

        //
        // Kijkt waar de fout zit
        //
        private void checkFout(string bLocatie, string eLocatie, bool beginBool, bool eindBool)
        {
            if (bLocatie == "Departure")
                highlightTextBox(beginInput, "Departure is empty");
            else if (eLocatie == "Destination")
                highlightTextBox(eindInput, "Destination is empty");
            else if (!beginBool)
                highlightTextBox(beginInput, "Departure is wrong");
            else if (!eindBool)
                highlightTextBox(eindInput, "Destination is wrong");
        }

        //
        // Highlight en geeft de pop up aan wat en waar de fout zit
        //

        private void highlightTextBox(TextBox textbox, string text)
        {
            MessageBox.Show(text + ", Try again", "Something went wrong");
            textbox.ForeColor = Color.Red;
        }

        //
        // VULT TIJD INPUT MET TIJDEN
        //
        private void vultijdInput()
        {
            DateTime vorigeMin = Afronden(DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), TimeSpan.FromMinutes(5));
            DateTime extraMin;
            for (int i = 0; i < 20; i++)
            {
                extraMin = vorigeMin.AddMinutes(5);
                tijdenList.Add(new tijdenModel() { vertrekTijd = extraMin.ToString("HH:mm") });
                vorigeMin = extraMin;
            }
        }

        private static bool checkIfEmpty(string bLocatie, string eLocatie)
        {
            if (bLocatie != "Departure" && eLocatie != "Destination")
                return true;
            return false;
        }

        //
        // STUURT DATA DOOR NAAR DE FAKE DATA GENERATOR
        //
        private void searchButton_Click(object sender, EventArgs e)
        {
            autoSuggestie autosuggest = new autoSuggestie(this);
            beginLocatie = beginInput.Text;
            eindLocatie = eindInput.Text;
            vertrekTijd = tijdInput.Text;
            if (checkBeginLocatie(beginLocatie, autosuggest.stationList) &&
                    checkEindLocatie(eindLocatie, autosuggest.stationList))
            {
                v.vertrekModel(beginLocatie, eindLocatie, vertrekTijd);
                gekozenTijd = Convert.ToDateTime(vertrekTijd);
                setupReisOpties();
            }
            else
            {
                checkFout(beginLocatie, eindLocatie, 
                    checkBeginLocatie(beginLocatie, autosuggest.stationList),
                    checkEindLocatie(eindLocatie, autosuggest.stationList));
            }
        }

        //
        // ROND DE TIJDINPUT AF NAAR 5-TALLEN
        //
        DateTime Afronden(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        //
        // Laat de flowcontrol met alle reisopties zien.
        //
        public void setupReisOpties()
        {
            show(flowLayoutPanel);
            flowLayoutPanel.Location = new Point(0, logoHeader.Height);
            clearFlowControl(this.flowLayoutPanel);
            reisOpties.Clear();
            reisOpties = fakeLijst(gekozenTijd);
            vullReisOpties(new Cell[reisOpties.Count()]);
        }

        //
        // Vult de flowcontrol met usercontrols genaamd "Cell" en geeft de juiste data mee aan de Cell.
        //
        private void vullReisOpties(Cell[] listItems)
        {
            for (int i = 0; i < reisOpties.Count; i++)
            {
                listItems[i] = new Cell(this);
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
                listItems[i].orange = false;
                if (flowLayoutPanel.Controls.Count < 0)
                    clearFlowControl(this.flowLayoutPanel);
                else
                    flowLayoutPanel.Controls.Add(listItems[i]);
            }
        }

        //
        // Geeft de juiste gegevens door aan de detailsView.
        //
        private void showReisDetails()
        {
            this.detailsUserControl.Visible = true;
            if (!detailsControl)
            {
                this.detailsUserControl.Location = new Point(flowLayoutPanel.Location.X + flowLayoutPanel.Width, flowLayoutPanel.Location.Y);
                show(detailsControl);
            }
            this.detailsUserControl.tussenstopsPanel.Controls.Clear();
            this.detailsUserControl.beginTijd = cell.beginTijd;
            this.detailsUserControl.eindTijd = cell.eindTijd;
            this.detailsUserControl.tussenstop = cell.tussenstop;
            this.detailsUserControl.totaleTijd = cell.totaleTijd;
            this.detailsUserControl.aantalOverstappen = cell.aantalOverstappen;
            this.detailsUserControl.perron = cell.perron;
        }

        //
        // Zorgt ervoor dat de juiste methodes worden aangeroepen waardoor de detailsView juist wordt weergegeven.
        //
        public void setupReisDetails()
        {
            showReisDetails();
            vullTussenStops(new tussenstopCell[this.detailsUserControl.tussenstop.Count()]);
        }

        //
        // Vult de flowcontrol met usercontrols genaamd "tussenstopCell" en geeft de juiste data mee aan de tussenstopCell.
        //
        private void vullTussenStops(tussenstopCell[] listItems)
        {
            for (int i = 0; i < this.detailsUserControl.tussenstop.Count; i++)
            {
                listItems[i] = new tussenstopCell();
                listItems[i].vertrekTijd = this.detailsUserControl.tussenstop[i].vertrekTijd;
                listItems[i].stationNaam = this.detailsUserControl.tussenstop[i].station;
                listItems[i].perron = this.detailsUserControl.tussenstop[i].perron;
                listItems[i].richting = this.detailsUserControl.tussenstop[i].richtingVervoer;
                listItems[i].typeVervoer = this.detailsUserControl.tussenstop[i].typeVervoer;
                if (i == 0)
                    listItems[i].eerste = true;
                else if (i == this.detailsUserControl.tussenstop.Count - 1)
                    listItems[i].laatste = true;
                else
                    listItems[i].midden = true;

                if (this.detailsUserControl.tussenstopsPanel.Controls.Count < 0)
                    clearFlowControl(detailsUserControl);
                else
                    this.detailsUserControl.tussenstopsPanel.Controls.Add(listItems[i]);
            }
        }

        //
        // Nadat de gebruiker iets heeft getypt in de beginInput TextBox, wordt een autosuggestie laten zien of wordt de autosuggesties juist weggehaald.
        //
        private void beginInput_TextChanged(object sender, EventArgs e)
        {
            autoSuggestie autosuggest = new autoSuggestie(this);
            if (beginInput.Text != "" && veranderInput == false)
            {
                autosuggest.checkInput(beginInput.Text);
                if (autosuggest.suggestieslist.Count > 0)
                {
                    autosuggestVisible();
                    autosuggest.setupSuggesties(sender);
                }
                else
                {
                    autosuggesInvVisible();
                    autosuggest.clearAutosuggest();
                }
            }
            else
            {
                autosuggesInvVisible();
            }
        }

        //
        // Nadat de gebruiker iets heeft getypt in de eindInput TextBox, wordt een autosuggestie laten zien of wordt de autosuggesties juist weggehaald.
        //
        private void eindInput_TextChanged(object sender, EventArgs e)
        {
            autoSuggestie autosuggest = new autoSuggestie(this);
            if (eindInput.Text != "" && veranderInput == false)
            {
                autosuggest.checkInput(eindInput.Text);
                if (autosuggest.suggestieslist.Count > 0)
                {
                    autosuggestVisible();
                    autosuggest.setupSuggesties(sender);
                }
                else
                {
                    autosuggesInvVisible();
                    autosuggest.clearAutosuggest();
                }
            }
            else
            {
                autosuggesInvVisible();
            }
        }

        //
        // Vult de flowcontrol met usercontrols genaamd "autoSuggesCell" en geeft de juiste data mee aan de autoSuggesCell.
        //
        public void vullAutosuggestie(autoSuggesCell[] listItems, bool bInput, List<autoSuggestModel> suggestieslist)
        {
            for (int i = 0; i < suggestieslist.Count; i++)
            {
                listItems[i] = new autoSuggesCell(this.autoSuggestie1, this);
                listItems[i].stationNaam = suggestieslist[i].stationNaam;
                listItems[i].stationType = suggestieslist[i].stationType;
                listItems[i].beginInput = bInput;
                if (this.autoSuggestie1.autosuggestFlowControl.Controls.Count < 0)
                    clearFlowControl(this.autoSuggestie1.autosuggestFlowControl);
                else
                    this.autoSuggestie1.autosuggestFlowControl.Controls.Add(listItems[i]);
            }
        }

        //
        // Laat de userControl op de juiste Y zien en laat zorgt ervoor dat de juist hoogte van de flowControl wordt ingesteld.
        //
        public void setFlowControl(int yLocation, int aantalElementen)
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

        //
        // Maakt FlowControlPanel Leeg.
        //
        public void clearFlowControl(object sender)
        {
            if (sender.Equals(detailsUserControl))
                this.detailsUserControl.tussenstopsPanel.Controls.Clear();
            else if (sender.Equals(flowLayoutPanel))
                this.flowLayoutPanel.Controls.Clear();
            else
                this.autoSuggestie1.autosuggestFlowControl.Controls.Clear();
        }

        //
        // Maakt de Suggestie userControl Visible.
        //
        public void autosuggestVisible()
        {
            this.autoSuggestie1.Visible = true;
        }
        //
        // Maakt de Suggestie userControl inVisible.
        //
        public void autosuggesInvVisible()
        {
            this.autoSuggestie1.Visible = false;
        }

        private void backIcon_Click(object sender, EventArgs e)
        {
            detailsControl = false;
            inputControl = true;
            inputPanel.Visible = true;
            hideshowBack();
        }

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

        //
        // Hides active views
        //
        private void hideArrowIcon_Click(object sender, EventArgs e)
        {
            Console.WriteLine("inputControl: " + inputControl);
            Console.WriteLine("optiesControl: " + optiesControl);
            Console.WriteLine("indetailsControlput: " + detailsControl);
            Console.WriteLine("optionSelected: " + optionSelected);

            if (inputControl)
            {
                inputPanel.Visible = false;
                hide(inputPanel);

            }
            else if (optiesControl)
            {
                flowLayoutPanel.Visible = false;
                hide(flowLayoutPanel);

            }
            else if (detailsControl)
            {
                detailsUserControl.Visible = false;
                hide(detailsControl);
            }
            else if (!inputControl && !optiesControl && !detailsControl && !optionSelected)
            {
                inputPanel.Visible = true;
                show(inputPanel);
            }
            else if (!inputControl && !optiesControl && !detailsControl && optionSelected)
            {
                flowLayoutPanel.Visible = true;
                show(flowLayoutPanel);
            }
        }

        //
        // VERANDERD BEGIN PUNT NAAR EIND PUNT EN ANDERSOM
        //
        private void changeTextImage_Click(object sender, EventArgs e)
        {
            if (checkIfEmpty(beginInput.Text, eindInput.Text))
            {
                veranderInput = true;
                bLocatie = beginInput.Text;
                eLocatie = eindInput.Text;
                beginInput.Text = eLocatie;
                eindInput.Text = bLocatie;
                veranderInput = false;
            }
        }

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
                this.logoHeader.Width = logoHeader.Width + detailsUserControl.Width;
                hideBarLocation(logoHeader.Width, logoHeader.Height);
                changeBackIcon(false);
            }
        }

        private void changeBackIcon(bool Forward)
        {
            if (Forward)
                hideArrowIcon.Image = manderijntje.Properties.Resources.FowardArrow;
            else
                hideArrowIcon.Image = manderijntje.Properties.Resources.BackwardArrow;
        }


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
        private void hideBarLocation(int x, int y)
        {
            hideBar.Location = new Point(x - hideBarOrangePanel.Width, y);
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
        private List<reisOpties> fakeLijst(DateTime gekozenTijd)
        {
            List<reisOpties> opties = new List<reisOpties>();
            for (int i = 0; i < 20; i++)
            {
                gekozenTijd = gekozenTijd.AddMinutes(6);
                string eindTijd = gekozenTijd.AddMinutes(19).ToString("HH:mm");
                opties.Add(FakeData(gekozenTijd.ToString("HH:mm"), eindTijd, "NS", "TREIN", "Intercity", "", "0:19", "0", "1a"));
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
        private tussenStops fakeTussenStops(string station, string perron, string aankomstTijd, string vertrekTijd,
           string typeVervoer, string richtingVervoer)
        {
            tussenStops t = new tussenStops(station, perron, aankomstTijd, vertrekTijd, typeVervoer, richtingVervoer);
            return t;
        }
    }
}


