namespace manderijntje
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
            this.carrierLBL = new System.Windows.Forms.Label();
            this.totaltimeLBL = new System.Windows.Forms.Label();
            this.transferLBL = new System.Windows.Forms.Label();
            this.clockIcon = new System.Windows.Forms.PictureBox();
            this.transferIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferIcon)).BeginInit();
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
            // carrierLBL
            // 
            this.carrierLBL.AutoSize = true;
            this.carrierLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carrierLBL.ForeColor = System.Drawing.Color.DimGray;
            this.carrierLBL.Location = new System.Drawing.Point(37, 81);
            this.carrierLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.carrierLBL.Name = "carrierLBL";
            this.carrierLBL.Size = new System.Drawing.Size(187, 36);
            this.carrierLBL.TabIndex = 2;
            this.carrierLBL.Text = "NS - Intercity";
            this.carrierLBL.Click += new System.EventHandler(this.carrierLBL_Click);
            // 
            // totaltimeLBL
            // 
            this.totaltimeLBL.AutoSize = true;
            this.totaltimeLBL.BackColor = System.Drawing.Color.Transparent;
            this.totaltimeLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totaltimeLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.totaltimeLBL.Location = new System.Drawing.Point(511, 29);
            this.totaltimeLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.totaltimeLBL.Name = "totaltimeLBL";
            this.totaltimeLBL.Size = new System.Drawing.Size(78, 36);
            this.totaltimeLBL.TabIndex = 3;
            this.totaltimeLBL.Text = "0:19";
            this.totaltimeLBL.Click += new System.EventHandler(this.totaltimeLBL_Click);
            // 
            // transferLBL
            // 
            this.transferLBL.AutoSize = true;
            this.transferLBL.BackColor = System.Drawing.Color.Transparent;
            this.transferLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transferLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.transferLBL.Location = new System.Drawing.Point(645, 29);
            this.transferLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.transferLBL.Name = "transferLBL";
            this.transferLBL.Size = new System.Drawing.Size(49, 36);
            this.transferLBL.TabIndex = 4;
            this.transferLBL.Text = "0x";
            this.transferLBL.Click += new System.EventHandler(this.transferLBL_Click);
            // 
            // clockIcon
            // 
            this.clockIcon.Image = global::manderijntje.Properties.Resources.OrangeClock;
            this.clockIcon.Location = new System.Drawing.Point(464, 26);
            this.clockIcon.Margin = new System.Windows.Forms.Padding(4);
            this.clockIcon.Name = "clockIcon";
            this.clockIcon.Size = new System.Drawing.Size(38, 38);
            this.clockIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clockIcon.TabIndex = 8;
            this.clockIcon.TabStop = false;
            // 
            // transferIcon
            // 
            this.transferIcon.Image = global::manderijntje.Properties.Resources.OverstappenOrange;
            this.transferIcon.Location = new System.Drawing.Point(607, 30);
            this.transferIcon.Margin = new System.Windows.Forms.Padding(4);
            this.transferIcon.Name = "transferIcon";
            this.transferIcon.Size = new System.Drawing.Size(38, 37);
            this.transferIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.transferIcon.TabIndex = 9;
            this.transferIcon.TabStop = false;
            // 
            // tripOptionsCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.transferIcon);
            this.Controls.Add(this.clockIcon);
            this.Controls.Add(this.transferLBL);
            this.Controls.Add(this.totaltimeLBL);
            this.Controls.Add(this.carrierLBL);
            this.Controls.Add(this.TimeLBL);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "tripOptionsCell";
            this.Size = new System.Drawing.Size(733, 140);
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label TimeLBL;
        private System.Windows.Forms.Label carrierLBL;
        private System.Windows.Forms.Label totaltimeLBL;
        private System.Windows.Forms.Label transferLBL;
        private System.Windows.Forms.PictureBox clockIcon;
        private System.Windows.Forms.PictureBox transferIcon;
    }
}
