﻿namespace manderijntje
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
            this.transferIcon = new System.Windows.Forms.PictureBox();
            this.clockIcon = new System.Windows.Forms.PictureBox();
            this.platformLBL = new System.Windows.Forms.Label();
            this.transfersLBL = new System.Windows.Forms.Label();
            this.totaltimeLBL = new System.Windows.Forms.Label();
            this.arrivalLBL = new System.Windows.Forms.Label();
            this.timesLBL = new System.Windows.Forms.Label();
            this.departureLBL = new System.Windows.Forms.Label();
            this.transfersPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.headerDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transferIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // headerDetails
            // 
            this.headerDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.headerDetails.Controls.Add(this.transferIcon);
            this.headerDetails.Controls.Add(this.clockIcon);
            this.headerDetails.Controls.Add(this.platformLBL);
            this.headerDetails.Controls.Add(this.transfersLBL);
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
            // transferIcon
            // 
            this.transferIcon.Image = global::manderijntje.Properties.Resources.OverstappenWhite;
            this.transferIcon.Location = new System.Drawing.Point(663, 29);
            this.transferIcon.Margin = new System.Windows.Forms.Padding(4);
            this.transferIcon.Name = "transferIcon";
            this.transferIcon.Size = new System.Drawing.Size(38, 36);
            this.transferIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.transferIcon.TabIndex = 11;
            this.transferIcon.TabStop = false;
            // 
            // clockIcon
            // 
            this.clockIcon.Image = global::manderijntje.Properties.Resources.WhiteClock;
            this.clockIcon.Location = new System.Drawing.Point(522, 28);
            this.clockIcon.Margin = new System.Windows.Forms.Padding(4);
            this.clockIcon.Name = "clockIcon";
            this.clockIcon.Size = new System.Drawing.Size(38, 38);
            this.clockIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clockIcon.TabIndex = 10;
            this.clockIcon.TabStop = false;
            // 
            // platformLBL
            // 
            this.platformLBL.AutoSize = true;
            this.platformLBL.BackColor = System.Drawing.Color.Transparent;
            this.platformLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.platformLBL.ForeColor = System.Drawing.Color.White;
            this.platformLBL.Location = new System.Drawing.Point(762, 29);
            this.platformLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.platformLBL.Name = "platformLBL";
            this.platformLBL.Size = new System.Drawing.Size(50, 36);
            this.platformLBL.TabIndex = 8;
            this.platformLBL.Text = "1a";
            // 
            // transfersLBL
            // 
            this.transfersLBL.AutoSize = true;
            this.transfersLBL.BackColor = System.Drawing.Color.Transparent;
            this.transfersLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transfersLBL.ForeColor = System.Drawing.Color.White;
            this.transfersLBL.Location = new System.Drawing.Point(703, 28);
            this.transfersLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.transfersLBL.Name = "transfersLBL";
            this.transfersLBL.Size = new System.Drawing.Size(49, 36);
            this.transfersLBL.TabIndex = 7;
            this.transfersLBL.Text = "0x";
            // 
            // totaltimeLBL
            // 
            this.totaltimeLBL.AutoSize = true;
            this.totaltimeLBL.BackColor = System.Drawing.Color.Transparent;
            this.totaltimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totaltimeLBL.ForeColor = System.Drawing.Color.White;
            this.totaltimeLBL.Location = new System.Drawing.Point(569, 30);
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
            ((System.ComponentModel.ISupportInitialize)(this.transferIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerDetails;
        private System.Windows.Forms.Label arrivalLBL;
        private System.Windows.Forms.Label timesLBL;
        private System.Windows.Forms.Label departureLBL;
        private System.Windows.Forms.Label platformLBL;
        private System.Windows.Forms.Label transfersLBL;
        private System.Windows.Forms.Label totaltimeLBL;
        public System.Windows.Forms.FlowLayoutPanel transfersPanel;
        private System.Windows.Forms.PictureBox transferIcon;
        private System.Windows.Forms.PictureBox clockIcon;
    }
}
