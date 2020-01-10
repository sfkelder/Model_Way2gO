namespace manderijntje
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logoHeader = new System.Windows.Forms.Panel();
            this.backIcon = new System.Windows.Forms.PictureBox();
            this.logoIcon = new System.Windows.Forms.PictureBox();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.planTripLabel = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.tijdPanel = new System.Windows.Forms.Panel();
            this.vertrekLabel = new System.Windows.Forms.Label();
            this.tijdInput = new System.Windows.Forms.ComboBox();
            this.textboxPanel = new System.Windows.Forms.Panel();
            this.changeTextImage = new System.Windows.Forms.PictureBox();
            this.beginInput = new System.Windows.Forms.TextBox();
            this.eindInput = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.hideBar = new System.Windows.Forms.Panel();
            this.hideArrowIcon = new System.Windows.Forms.PictureBox();
            this.hideBarOrangePanel = new System.Windows.Forms.Panel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.autoSuggestie1 = new manderijntje.autoSuggestie();
            this.detailsUserControl = new manderijntje.DetailsControl();
            this.logoHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoIcon)).BeginInit();
            this.inputPanel.SuspendLayout();
            this.tijdPanel.SuspendLayout();
            this.textboxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.changeTextImage)).BeginInit();
            this.hideBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hideArrowIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // logoHeader
            // 
            this.logoHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.logoHeader.Controls.Add(this.backIcon);
            this.logoHeader.Controls.Add(this.logoIcon);
            this.logoHeader.Location = new System.Drawing.Point(0, 0);
            this.logoHeader.Margin = new System.Windows.Forms.Padding(0);
            this.logoHeader.Name = "logoHeader";
            this.logoHeader.Size = new System.Drawing.Size(390, 59);
            this.logoHeader.TabIndex = 1;
            // 
            // backIcon
            // 
            this.backIcon.Image = global::manderijntje.Properties.Resources.backArrowWhite;
            this.backIcon.Location = new System.Drawing.Point(24, 22);
            this.backIcon.Name = "backIcon";
            this.backIcon.Size = new System.Drawing.Size(20, 20);
            this.backIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backIcon.TabIndex = 2;
            this.backIcon.TabStop = false;
            this.backIcon.Click += new System.EventHandler(this.backIcon_Click);
            // 
            // logoIcon
            // 
            this.logoIcon.Image = global::manderijntje.Properties.Resources.Way2GoLogo;
            this.logoIcon.Location = new System.Drawing.Point(153, 0);
            this.logoIcon.Name = "logoIcon";
            this.logoIcon.Size = new System.Drawing.Size(71, 58);
            this.logoIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoIcon.TabIndex = 1;
            this.logoIcon.TabStop = false;
            // 
            // inputPanel
            // 
            this.inputPanel.Controls.Add(this.planTripLabel);
            this.inputPanel.Controls.Add(this.searchButton);
            this.inputPanel.Controls.Add(this.tijdPanel);
            this.inputPanel.Controls.Add(this.textboxPanel);
            this.inputPanel.Location = new System.Drawing.Point(87, 265);
            this.inputPanel.Margin = new System.Windows.Forms.Padding(2);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(238, 185);
            this.inputPanel.TabIndex = 8;
            // 
            // planTripLabel
            // 
            this.planTripLabel.AutoSize = true;
            this.planTripLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.planTripLabel.Location = new System.Drawing.Point(2, 6);
            this.planTripLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.planTripLabel.Name = "planTripLabel";
            this.planTripLabel.Size = new System.Drawing.Size(154, 26);
            this.planTripLabel.TabIndex = 3;
            this.planTripLabel.Text = "Plan your trip";
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(39, 144);
            this.searchButton.Margin = new System.Windows.Forms.Padding(2);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(147, 36);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "Get your Trip";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // tijdPanel
            // 
            this.tijdPanel.Controls.Add(this.vertrekLabel);
            this.tijdPanel.Controls.Add(this.tijdInput);
            this.tijdPanel.Location = new System.Drawing.Point(0, 111);
            this.tijdPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tijdPanel.Name = "tijdPanel";
            this.tijdPanel.Size = new System.Drawing.Size(234, 31);
            this.tijdPanel.TabIndex = 6;
            // 
            // vertrekLabel
            // 
            this.vertrekLabel.AutoSize = true;
            this.vertrekLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vertrekLabel.Location = new System.Drawing.Point(3, 5);
            this.vertrekLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.vertrekLabel.Name = "vertrekLabel";
            this.vertrekLabel.Size = new System.Drawing.Size(48, 13);
            this.vertrekLabel.TabIndex = 6;
            this.vertrekLabel.Text = "Vertrek";
            // 
            // tijdInput
            // 
            this.tijdInput.FormattingEnabled = true;
            this.tijdInput.Location = new System.Drawing.Point(75, 1);
            this.tijdInput.Margin = new System.Windows.Forms.Padding(2);
            this.tijdInput.Name = "tijdInput";
            this.tijdInput.Size = new System.Drawing.Size(111, 21);
            this.tijdInput.TabIndex = 5;
            // 
            // textboxPanel
            // 
            this.textboxPanel.Controls.Add(this.changeTextImage);
            this.textboxPanel.Controls.Add(this.beginInput);
            this.textboxPanel.Controls.Add(this.eindInput);
            this.textboxPanel.Location = new System.Drawing.Point(2, 43);
            this.textboxPanel.Margin = new System.Windows.Forms.Padding(2);
            this.textboxPanel.Name = "textboxPanel";
            this.textboxPanel.Size = new System.Drawing.Size(234, 64);
            this.textboxPanel.TabIndex = 4;
            // 
            // changeTextImage
            // 
            this.changeTextImage.Image = global::manderijntje.Properties.Resources.changeText;
            this.changeTextImage.Location = new System.Drawing.Point(194, 10);
            this.changeTextImage.Name = "changeTextImage";
            this.changeTextImage.Size = new System.Drawing.Size(35, 35);
            this.changeTextImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.changeTextImage.TabIndex = 14;
            this.changeTextImage.TabStop = false;
            this.changeTextImage.Click += new System.EventHandler(this.changeTextImage_Click);
            // 
            // beginInput
            // 
            this.beginInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.beginInput.Location = new System.Drawing.Point(2, 2);
            this.beginInput.Margin = new System.Windows.Forms.Padding(2);
            this.beginInput.Name = "beginInput";
            this.beginInput.Size = new System.Drawing.Size(182, 20);
            this.beginInput.TabIndex = 0;
            this.beginInput.TextChanged += new System.EventHandler(this.departureInput_TextChanged);
            // 
            // eindInput
            // 
            this.eindInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eindInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eindInput.Location = new System.Drawing.Point(3, 32);
            this.eindInput.Margin = new System.Windows.Forms.Padding(2);
            this.eindInput.Name = "eindInput";
            this.eindInput.Size = new System.Drawing.Size(181, 20);
            this.eindInput.TabIndex = 1;
            this.eindInput.TextChanged += new System.EventHandler(this.destinationInput_TextChanged);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Location = new System.Drawing.Point(383, 62);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(381, 685);
            this.flowLayoutPanel.TabIndex = 9;
            // 
            // hideBar
            // 
            this.hideBar.BackColor = System.Drawing.Color.Transparent;
            this.hideBar.Controls.Add(this.hideArrowIcon);
            this.hideBar.Controls.Add(this.hideBarOrangePanel);
            this.hideBar.Location = new System.Drawing.Point(1322, 0);
            this.hideBar.Name = "hideBar";
            this.hideBar.Size = new System.Drawing.Size(31, 737);
            this.hideBar.TabIndex = 13;
            // 
            // hideArrowIcon
            // 
            this.hideArrowIcon.BackColor = System.Drawing.Color.Transparent;
            this.hideArrowIcon.Image = global::manderijntje.Properties.Resources.BackwardArrow;
            this.hideArrowIcon.Location = new System.Drawing.Point(-7, 287);
            this.hideArrowIcon.Name = "hideArrowIcon";
            this.hideArrowIcon.Size = new System.Drawing.Size(35, 35);
            this.hideArrowIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.hideArrowIcon.TabIndex = 0;
            this.hideArrowIcon.TabStop = false;
            this.hideArrowIcon.Click += new System.EventHandler(this.hideArrowIcon_Click);
            // 
            // hideBarOrangePanel
            // 
            this.hideBarOrangePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.hideBarOrangePanel.Location = new System.Drawing.Point(0, 0);
            this.hideBarOrangePanel.Name = "hideBarOrangePanel";
            this.hideBarOrangePanel.Size = new System.Drawing.Size(10, 734);
            this.hideBarOrangePanel.TabIndex = 0;
            // 
            // autoSuggestie1
            // 
            this.autoSuggestie1.Location = new System.Drawing.Point(87, 456);
            this.autoSuggestie1.Margin = new System.Windows.Forms.Padding(4);
            this.autoSuggestie1.Name = "autoSuggestie1";
            this.autoSuggestie1.Size = new System.Drawing.Size(186, 124);
            this.autoSuggestie1.TabIndex = 15;
            // 
            // detailsUserControl
            // 
            this.detailsUserControl.aantalOverstappen = null;
            this.detailsUserControl.beginTijd = null;
            this.detailsUserControl.eindTijd = null;
            this.detailsUserControl.Location = new System.Drawing.Point(771, 62);
            this.detailsUserControl.Margin = new System.Windows.Forms.Padding(4);
            this.detailsUserControl.Name = "detailsUserControl";
            this.detailsUserControl.perron = null;
            this.detailsUserControl.shortestPath = null;
            this.detailsUserControl.Size = new System.Drawing.Size(422, 685);
            this.detailsUserControl.TabIndex = 14;
            this.detailsUserControl.totaleTijd = null;
            this.detailsUserControl.tussenstop = null;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1365, 726);
            this.Controls.Add(this.autoSuggestie1);
            this.Controls.Add(this.detailsUserControl);
            this.Controls.Add(this.hideBar);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.logoHeader);
            this.Name = "Form1";
            this.logoHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.backIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoIcon)).EndInit();
            this.inputPanel.ResumeLayout(false);
            this.inputPanel.PerformLayout();
            this.tijdPanel.ResumeLayout(false);
            this.tijdPanel.PerformLayout();
            this.textboxPanel.ResumeLayout(false);
            this.textboxPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.changeTextImage)).EndInit();
            this.hideBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hideArrowIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel logoHeader;
        public System.Windows.Forms.Panel inputPanel;
        private System.Windows.Forms.Label planTripLabel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Panel tijdPanel;
        private System.Windows.Forms.Label vertrekLabel;
        private System.Windows.Forms.ComboBox tijdInput;
        public System.Windows.Forms.Panel textboxPanel;
        public System.Windows.Forms.TextBox beginInput;
        public System.Windows.Forms.TextBox eindInput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Panel hideBar;
        private System.Windows.Forms.Panel hideBarOrangePanel;
        private System.Windows.Forms.PictureBox logoIcon;
        private System.Windows.Forms.PictureBox hideArrowIcon;
        private System.Windows.Forms.PictureBox backIcon;
        private System.Windows.Forms.PictureBox changeTextImage;
        private System.Windows.Forms.ImageList imageList1;
        private DetailsControl detailsUserControl;
        private autoSuggestie autoSuggestie1;
    }
}

