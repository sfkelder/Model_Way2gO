﻿namespace Manderijntje
{
    partial class TransferCell
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
            this.departuretimeLBL = new System.Windows.Forms.Label();
            this.stationLBL = new System.Windows.Forms.Label();
            this.lineImage = new System.Windows.Forms.PictureBox();
            this.typetransportIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lineImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typetransportIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // departuretimeLBL
            // 
            this.departuretimeLBL.AutoSize = true;
            this.departuretimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departuretimeLBL.Location = new System.Drawing.Point(36, 39);
            this.departuretimeLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.departuretimeLBL.Name = "departuretimeLBL";
            this.departuretimeLBL.Size = new System.Drawing.Size(96, 36);
            this.departuretimeLBL.TabIndex = 0;
            this.departuretimeLBL.Text = "12:01";
            // 
            // stationLBL
            // 
            this.stationLBL.AutoSize = true;
            this.stationLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stationLBL.Location = new System.Drawing.Point(210, 41);
            this.stationLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.stationLBL.Name = "stationLBL";
            this.stationLBL.Size = new System.Drawing.Size(269, 33);
            this.stationLBL.TabIndex = 1;
            this.stationLBL.Text = "Rotterdam Centraal";
            // 
            // lineImage
            // 
            this.lineImage.Image = manderijntje.Properties.Resources.startTrack;
            this.lineImage.Location = new System.Drawing.Point(150, 0);
            this.lineImage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lineImage.Name = "lineImage";
            this.lineImage.Size = new System.Drawing.Size(35, 116);
            this.lineImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.lineImage.TabIndex = 4;
            this.lineImage.TabStop = false;
            // 
            // typetransportIcon
            // 
            this.typetransportIcon.Image = manderijntje.Properties.Resources.OrangeTrain;
            this.typetransportIcon.Location = new System.Drawing.Point(741, 46);
            this.typetransportIcon.Margin = new System.Windows.Forms.Padding(4);
            this.typetransportIcon.Name = "typetransportIcon";
            this.typetransportIcon.Size = new System.Drawing.Size(22, 31);
            this.typetransportIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.typetransportIcon.TabIndex = 5;
            this.typetransportIcon.TabStop = false;
            // 
            // transferCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.typetransportIcon);
            this.Controls.Add(this.lineImage);
            this.Controls.Add(this.stationLBL);
            this.Controls.Add(this.departuretimeLBL);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "transferCell";
            this.Size = new System.Drawing.Size(808, 116);
            ((System.ComponentModel.ISupportInitialize)(this.lineImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typetransportIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label departuretimeLBL;
        private System.Windows.Forms.Label stationLBL;
        private System.Windows.Forms.PictureBox lineImage;
        private System.Windows.Forms.PictureBox typetransportIcon;
    }
}
