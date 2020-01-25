namespace Manderijntje
{
    partial class tripOptionsCell
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
            this.TimeLBL = new System.Windows.Forms.Label();
            this.nameTransportLBL = new System.Windows.Forms.Label();
            this.totaltimeLBL = new System.Windows.Forms.Label();
            this.clockIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TimeLBL
            // 
            this.TimeLBL.AutoSize = true;
            this.TimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLBL.Location = new System.Drawing.Point(37, 29);
            this.TimeLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.TimeLBL.Name = "TimeLBL";
            this.TimeLBL.Size = new System.Drawing.Size(215, 36);
            this.TimeLBL.TabIndex = 1;
            this.TimeLBL.Text = " 12:01 - 12:01";
            this.TimeLBL.Click += new System.EventHandler(this.eindTijdLBL_Click);
            // 
            // nameTransportLBL
            // 
            this.nameTransportLBL.AutoSize = true;
            this.nameTransportLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTransportLBL.ForeColor = System.Drawing.Color.DimGray;
            this.nameTransportLBL.Location = new System.Drawing.Point(37, 81);
            this.nameTransportLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.nameTransportLBL.Name = "nameTransportLBL";
            this.nameTransportLBL.Size = new System.Drawing.Size(187, 36);
            this.nameTransportLBL.TabIndex = 2;
            this.nameTransportLBL.Text = "NS - Intercity";
            this.nameTransportLBL.Click += new System.EventHandler(this.carrierLBL_Click);
            // 
            // totaltimeLBL
            // 
            this.totaltimeLBL.AutoSize = true;
            this.totaltimeLBL.BackColor = System.Drawing.Color.Transparent;
            this.totaltimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totaltimeLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.totaltimeLBL.Location = new System.Drawing.Point(568, 31);
            this.totaltimeLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.totaltimeLBL.Name = "totaltimeLBL";
            this.totaltimeLBL.Size = new System.Drawing.Size(138, 36);
            this.totaltimeLBL.TabIndex = 3;
            this.totaltimeLBL.Text = "22h 30m";
            this.totaltimeLBL.Click += new System.EventHandler(this.totaltimeLBL_Click);
            // 
            // clockIcon
            // 
            this.clockIcon.Image = global::Manderijntje.Properties.Resources.OrangeClock;
            this.clockIcon.Location = new System.Drawing.Point(521, 28);
            this.clockIcon.Margin = new System.Windows.Forms.Padding(4);
            this.clockIcon.Name = "clockIcon";
            this.clockIcon.Size = new System.Drawing.Size(38, 38);
            this.clockIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clockIcon.TabIndex = 8;
            this.clockIcon.TabStop = false;
            // 
            // tripOptionsCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.clockIcon);
            this.Controls.Add(this.totaltimeLBL);
            this.Controls.Add(this.nameTransportLBL);
            this.Controls.Add(this.TimeLBL);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "tripOptionsCell";
            this.Size = new System.Drawing.Size(733, 140);
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label TimeLBL;
        private System.Windows.Forms.Label nameTransportLBL;
        public System.Windows.Forms.Label totaltimeLBL;
        public System.Windows.Forms.PictureBox clockIcon;
    }
}
