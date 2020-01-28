namespace Manderijntje
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
            this.planTripLBL = new System.Windows.Forms.Label();
            this.searchBTN = new System.Windows.Forms.Button();
            this.tijdPanel = new System.Windows.Forms.Panel();
            this.timeLBL = new System.Windows.Forms.Label();
            this.timeInput = new System.Windows.Forms.ComboBox();
            this.textboxPanel = new System.Windows.Forms.Panel();
            this.changeTextImage = new System.Windows.Forms.PictureBox();
            this.departureInput = new System.Windows.Forms.TextBox();
            this.destinationInput = new System.Windows.Forms.TextBox();
            this.tripOptionsFlowControl = new System.Windows.Forms.FlowLayoutPanel();
            this.hideBar = new System.Windows.Forms.Panel();
            this.hideArrowIcon = new System.Windows.Forms.PictureBox();
            this.hideBarOrangePanel = new System.Windows.Forms.Panel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.autoSuggestion = new AutoSuggestion();
            this.detailsUserControl = new DetailsControl();
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
            this.backIcon.Image = manderijntje.Properties.Resources.backArrowWhite;
            this.backIcon.Location = new System.Drawing.Point(24, 22);
            this.backIcon.Name = "backIcon";
            this.backIcon.Size = new System.Drawing.Size(20, 20);
            this.backIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backIcon.TabIndex = 2;
            this.backIcon.TabStop = false;
            this.backIcon.Click += new System.EventHandler(this.BackIconClick);
            // 
            // logoIcon
            // 
            this.logoIcon.Image = manderijntje.Properties.Resources.Way2GoLogo;
            this.logoIcon.Location = new System.Drawing.Point(153, 0);
            this.logoIcon.Name = "logoIcon";
            this.logoIcon.Size = new System.Drawing.Size(71, 58);
            this.logoIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoIcon.TabIndex = 1;
            this.logoIcon.TabStop = false;
            // 
            // inputPanel
            // 
            this.inputPanel.Controls.Add(this.planTripLBL);
            this.inputPanel.Controls.Add(this.searchBTN);
            this.inputPanel.Controls.Add(this.tijdPanel);
            this.inputPanel.Controls.Add(this.textboxPanel);
            this.inputPanel.Location = new System.Drawing.Point(87, 265);
            this.inputPanel.Margin = new System.Windows.Forms.Padding(2);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(238, 185);
            this.inputPanel.TabIndex = 8;
            // 
            // planTripLBL
            // 
            this.planTripLBL.AutoSize = true;
            this.planTripLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.planTripLBL.Location = new System.Drawing.Point(2, 6);
            this.planTripLBL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.planTripLBL.Name = "planTripLBL";
            this.planTripLBL.Size = new System.Drawing.Size(289, 51);
            this.planTripLBL.TabIndex = 3;
            this.planTripLBL.Text = "Plan your trip";
            // 
            // searchBTN
            // 
            this.searchBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.searchBTN.FlatAppearance.BorderSize = 0;
            this.searchBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBTN.ForeColor = System.Drawing.Color.White;
            this.searchBTN.Location = new System.Drawing.Point(39, 144);
            this.searchBTN.Margin = new System.Windows.Forms.Padding(2);
            this.searchBTN.Name = "searchBTN";
            this.searchBTN.Size = new System.Drawing.Size(147, 36);
            this.searchBTN.TabIndex = 2;
            this.searchBTN.Text = "Get your Trip";
            this.searchBTN.UseVisualStyleBackColor = false;
            this.searchBTN.Click += new System.EventHandler(this.searchButtonClick);
            // 
            // tijdPanel
            // 
            this.tijdPanel.Controls.Add(this.timeLBL);
            this.tijdPanel.Controls.Add(this.timeInput);
            this.tijdPanel.Location = new System.Drawing.Point(0, 111);
            this.tijdPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tijdPanel.Name = "tijdPanel";
            this.tijdPanel.Size = new System.Drawing.Size(234, 31);
            this.tijdPanel.TabIndex = 6;
            // 
            // timeLBL
            // 
            this.timeLBL.AutoSize = true;
            this.timeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLBL.Location = new System.Drawing.Point(3, 5);
            this.timeLBL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.timeLBL.Name = "timeLBL";
            this.timeLBL.Size = new System.Drawing.Size(64, 26);
            this.timeLBL.TabIndex = 6;
            this.timeLBL.Text = "Time";
            // 
            // timeInput
            // 
            this.timeInput.FormattingEnabled = true;
            this.timeInput.Location = new System.Drawing.Point(54, 1);
            this.timeInput.Margin = new System.Windows.Forms.Padding(2);
            this.timeInput.Name = "timeInput";
            this.timeInput.Size = new System.Drawing.Size(132, 33);
            this.timeInput.TabIndex = 5;
            // 
            // textboxPanel
            // 
            this.textboxPanel.Controls.Add(this.changeTextImage);
            this.textboxPanel.Controls.Add(this.departureInput);
            this.textboxPanel.Controls.Add(this.destinationInput);
            this.textboxPanel.Location = new System.Drawing.Point(2, 43);
            this.textboxPanel.Margin = new System.Windows.Forms.Padding(2);
            this.textboxPanel.Name = "textboxPanel";
            this.textboxPanel.Size = new System.Drawing.Size(234, 64);
            this.textboxPanel.TabIndex = 4;
            // 
            // changeTextImage
            // 
            this.changeTextImage.Image = manderijntje.Properties.Resources.changeText;
            this.changeTextImage.Location = new System.Drawing.Point(194, 10);
            this.changeTextImage.Name = "changeTextImage";
            this.changeTextImage.Size = new System.Drawing.Size(35, 35);
            this.changeTextImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.changeTextImage.TabIndex = 14;
            this.changeTextImage.TabStop = false;
            this.changeTextImage.Click += new System.EventHandler(this.ChangeTextImageClick);
            // 
            // departureInput
            // 
            this.departureInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departureInput.Location = new System.Drawing.Point(2, 2);
            this.departureInput.Margin = new System.Windows.Forms.Padding(2);
            this.departureInput.Name = "departureInput";
            this.departureInput.Size = new System.Drawing.Size(182, 32);
            this.departureInput.TabIndex = 0;
            this.departureInput.TextChanged += new System.EventHandler(this.DepartureInputTextChanged);
            // 
            // destinationInput
            // 
            this.destinationInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.destinationInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destinationInput.Location = new System.Drawing.Point(3, 32);
            this.destinationInput.Margin = new System.Windows.Forms.Padding(2);
            this.destinationInput.Name = "destinationInput";
            this.destinationInput.Size = new System.Drawing.Size(181, 32);
            this.destinationInput.TabIndex = 1;
            this.destinationInput.TextChanged += new System.EventHandler(this.DestinationInputTextChanged);
            // 
            // tripOptionsFlowControl
            // 
            this.tripOptionsFlowControl.AutoScroll = true;
            this.tripOptionsFlowControl.Location = new System.Drawing.Point(383, 62);
            this.tripOptionsFlowControl.Name = "tripOptionsFlowControl";
            this.tripOptionsFlowControl.Size = new System.Drawing.Size(381, 685);
            this.tripOptionsFlowControl.TabIndex = 9;
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
            this.hideArrowIcon.Image = manderijntje.Properties.Resources.BackwardArrow;
            this.hideArrowIcon.Location = new System.Drawing.Point(-7, 287);
            this.hideArrowIcon.Name = "hideArrowIcon";
            this.hideArrowIcon.Size = new System.Drawing.Size(35, 35);
            this.hideArrowIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.hideArrowIcon.TabIndex = 0;
            this.hideArrowIcon.TabStop = false;
            this.hideArrowIcon.Click += new System.EventHandler(this.HideArrowIconClick);
            // 
            // hideBarOrangePanel
            // 
            this.hideBarOrangePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.hideBarOrangePanel.Location = new System.Drawing.Point(0, 0);
            this.hideBarOrangePanel.Name = "hideBarOrangePanel";
            this.hideBarOrangePanel.Size = new System.Drawing.Size(10, 734);
            this.hideBarOrangePanel.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // autoSuggestion
            // 
            this.autoSuggestion.Location = new System.Drawing.Point(87, 456);
            this.autoSuggestion.Margin = new System.Windows.Forms.Padding(4);
            this.autoSuggestion.Name = "autoSuggestion";
            this.autoSuggestion.Size = new System.Drawing.Size(186, 124);
            this.autoSuggestion.TabIndex = 15;
            // 
            // detailsUserControl
            // 
            this.detailsUserControl.departureTime = null;
            this.detailsUserControl.destinationTime = null;
            this.detailsUserControl.Location = new System.Drawing.Point(771, 62);
            this.detailsUserControl.Margin = new System.Windows.Forms.Padding(4);
            this.detailsUserControl.Name = "detailsUserControl";
            this.detailsUserControl.shortestPath = null;
            this.detailsUserControl.Size = new System.Drawing.Size(422, 685);
            this.detailsUserControl.TabIndex = 14;
            this.detailsUserControl.totalTime = null;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1365, 726);
            this.Controls.Add(this.autoSuggestion);
            this.Controls.Add(this.detailsUserControl);
            this.Controls.Add(this.hideBar);
            this.Controls.Add(this.tripOptionsFlowControl);
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.logoHeader);
            this.Name = "Way2Go";
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
        private System.Windows.Forms.Label planTripLBL;
        private System.Windows.Forms.Button searchBTN;
        private System.Windows.Forms.Panel tijdPanel;
        private System.Windows.Forms.Label timeLBL;
        private System.Windows.Forms.ComboBox timeInput;
        public System.Windows.Forms.Panel textboxPanel;
        public System.Windows.Forms.TextBox departureInput;
        public System.Windows.Forms.TextBox destinationInput;
        private System.Windows.Forms.FlowLayoutPanel tripOptionsFlowControl;
        private System.Windows.Forms.Panel hideBar;
        private System.Windows.Forms.Panel hideBarOrangePanel;
        private System.Windows.Forms.PictureBox logoIcon;
        private System.Windows.Forms.PictureBox hideArrowIcon;
        private System.Windows.Forms.PictureBox backIcon;
        private System.Windows.Forms.PictureBox changeTextImage;
        private System.Windows.Forms.ImageList imageList1;
        private DetailsControl detailsUserControl;
        private AutoSuggestion autoSuggestion;
    }
}

