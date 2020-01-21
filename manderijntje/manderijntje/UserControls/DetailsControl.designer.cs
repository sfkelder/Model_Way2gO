namespace manderijntje
{
    partial class DetailsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerDetails = new System.Windows.Forms.Panel();
            this.saveRoute = new System.Windows.Forms.PictureBox();
            this.clockIcon = new System.Windows.Forms.PictureBox();
            this.totaltimeLBL = new System.Windows.Forms.Label();
            this.arrivalLBL = new System.Windows.Forms.Label();
            this.timesLBL = new System.Windows.Forms.Label();
            this.departureLBL = new System.Windows.Forms.Label();
            this.transfersPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.headerDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveRoute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // headerDetails
            // 
            this.headerDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.headerDetails.Controls.Add(this.saveRoute);
            this.headerDetails.Controls.Add(this.clockIcon);
            this.headerDetails.Controls.Add(this.totaltimeLBL);
            this.headerDetails.Controls.Add(this.arrivalLBL);
            this.headerDetails.Controls.Add(this.timesLBL);
            this.headerDetails.Controls.Add(this.departureLBL);
            this.headerDetails.Location = new System.Drawing.Point(0, 0);
            this.headerDetails.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.headerDetails.Name = "headerDetails";
            this.headerDetails.Size = new System.Drawing.Size(845, 140);
            this.headerDetails.TabIndex = 0;
            // 
            // saveRoute
            // 
            this.saveRoute.Image = global::manderijntje.Properties.Resources.saveIconWhite;
            this.saveRoute.Location = new System.Drawing.Point(771, 80);
            this.saveRoute.Name = "saveRoute";
            this.saveRoute.Size = new System.Drawing.Size(33, 36);
            this.saveRoute.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.saveRoute.TabIndex = 13;
            this.saveRoute.TabStop = false;
            this.saveRoute.Click += new System.EventHandler(this.saveRoute_Click);
            // 
            // clockIcon
            // 
            this.clockIcon.Image = global::manderijntje.Properties.Resources.WhiteClock;
            this.clockIcon.Location = new System.Drawing.Point(670, 29);
            this.clockIcon.Margin = new System.Windows.Forms.Padding(4);
            this.clockIcon.Name = "clockIcon";
            this.clockIcon.Size = new System.Drawing.Size(38, 38);
            this.clockIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clockIcon.TabIndex = 10;
            this.clockIcon.TabStop = false;
            // 
            // totaltimeLBL
            // 
            this.totaltimeLBL.AutoSize = true;
            this.totaltimeLBL.BackColor = System.Drawing.Color.Transparent;
            this.totaltimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totaltimeLBL.ForeColor = System.Drawing.Color.White;
            this.totaltimeLBL.Location = new System.Drawing.Point(717, 31);
            this.totaltimeLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.totaltimeLBL.Name = "totaltimeLBL";
            this.totaltimeLBL.Size = new System.Drawing.Size(78, 36);
            this.totaltimeLBL.TabIndex = 6;
            this.totaltimeLBL.Text = "0:19";
            // 
            // arrivalLBL
            // 
            this.arrivalLBL.AutoSize = true;
            this.arrivalLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arrivalLBL.ForeColor = System.Drawing.Color.White;
            this.arrivalLBL.Location = new System.Drawing.Point(188, 29);
            this.arrivalLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.arrivalLBL.Name = "arrivalLBL";
            this.arrivalLBL.Size = new System.Drawing.Size(101, 36);
            this.arrivalLBL.TabIndex = 2;
            this.arrivalLBL.Text = "Arrival";
            // 
            // timesLBL
            // 
            this.timesLBL.AutoSize = true;
            this.timesLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timesLBL.ForeColor = System.Drawing.Color.White;
            this.timesLBL.Location = new System.Drawing.Point(32, 80);
            this.timesLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.timesLBL.Name = "timesLBL";
            this.timesLBL.Size = new System.Drawing.Size(235, 36);
            this.timesLBL.TabIndex = 1;
            this.timesLBL.Text = "12:01 --> 12:20";
            // 
            // departureLBL
            // 
            this.departureLBL.AutoSize = true;
            this.departureLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departureLBL.ForeColor = System.Drawing.Color.White;
            this.departureLBL.Location = new System.Drawing.Point(32, 29);
            this.departureLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.departureLBL.Name = "departureLBL";
            this.departureLBL.Size = new System.Drawing.Size(146, 36);
            this.departureLBL.TabIndex = 0;
            this.departureLBL.Text = "Departure";
            // 
            // transfersPanel
            // 
            this.transfersPanel.AutoScroll = true;
            this.transfersPanel.Location = new System.Drawing.Point(0, 146);
            this.transfersPanel.Margin = new System.Windows.Forms.Padding(0);
            this.transfersPanel.Name = "transfersPanel";
            this.transfersPanel.Size = new System.Drawing.Size(845, 1139);
            this.transfersPanel.TabIndex = 1;
            // 
            // DetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.transfersPanel);
            this.Controls.Add(this.headerDetails);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "DetailsControl";
            this.Size = new System.Drawing.Size(845, 1285);
            this.headerDetails.ResumeLayout(false);
            this.headerDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveRoute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerDetails;
        private System.Windows.Forms.Label arrivalLBL;
        private System.Windows.Forms.Label timesLBL;
        private System.Windows.Forms.Label departureLBL;
        private System.Windows.Forms.Label totaltimeLBL;
        public System.Windows.Forms.FlowLayoutPanel transfersPanel;
        private System.Windows.Forms.PictureBox clockIcon;
        private System.Windows.Forms.PictureBox saveRoute;
    }
}
