namespace Manderijntje
{
    partial class AutoSuggestionCell
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
            this.stationLBL = new System.Windows.Forms.Label();
            this.stationTypeIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.stationTypeIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // stationLBL
            // 
            this.stationLBL.AutoSize = true;
            this.stationLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stationLBL.Location = new System.Drawing.Point(52, 15);
            this.stationLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.stationLBL.Name = "stationLBL";
            this.stationLBL.Size = new System.Drawing.Size(249, 29);
            this.stationLBL.TabIndex = 0;
            this.stationLBL.Text = "Amsterdam Centraal";
            this.stationLBL.Click += new System.EventHandler(this.StationLBLClick);
            // 
            // stationTypeIcon
            // 
            this.stationTypeIcon.Image = global::Manderijntje.Properties.Resources.OrangeTrain;
            this.stationTypeIcon.Location = new System.Drawing.Point(16, 12);
            this.stationTypeIcon.Margin = new System.Windows.Forms.Padding(4);
            this.stationTypeIcon.Name = "stationTypeIcon";
            this.stationTypeIcon.Size = new System.Drawing.Size(27, 32);
            this.stationTypeIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.stationTypeIcon.TabIndex = 1;
            this.stationTypeIcon.TabStop = false;
            this.stationTypeIcon.Click += new System.EventHandler(this.StationTypeIconClick);
            // 
            // autoSuggestionCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stationTypeIcon);
            this.Controls.Add(this.stationLBL);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "autoSuggestionCell";
            this.Size = new System.Drawing.Size(528, 59);
            ((System.ComponentModel.ISupportInitialize)(this.stationTypeIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label stationLBL;
        public System.Windows.Forms.PictureBox stationTypeIcon;
    }
}
