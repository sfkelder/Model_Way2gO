namespace manderijntje
{
    partial class Cell
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
            this.eindTijdLBL = new System.Windows.Forms.Label();
            this.VervoerderLBL = new System.Windows.Forms.Label();
            this.totaleTijdLBL = new System.Windows.Forms.Label();
            this.aantalOverstappenLBL = new System.Windows.Forms.Label();
            this.PerronLBL = new System.Windows.Forms.Label();
            this.clockIcon = new System.Windows.Forms.PictureBox();
            this.overstappenIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overstappenIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // eindTijdLBL
            // 
            this.eindTijdLBL.AutoSize = true;
            this.eindTijdLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eindTijdLBL.Location = new System.Drawing.Point(28, 23);
            this.eindTijdLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.eindTijdLBL.Name = "eindTijdLBL";
            this.eindTijdLBL.Size = new System.Drawing.Size(169, 29);
            this.eindTijdLBL.TabIndex = 1;
            this.eindTijdLBL.Text = " 12:01 - 12:01";
            this.eindTijdLBL.Click += new System.EventHandler(this.eindTijdLBL_Click);
            // 
            // VervoerderLBL
            // 
            this.VervoerderLBL.AutoSize = true;
            this.VervoerderLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VervoerderLBL.ForeColor = System.Drawing.Color.DimGray;
            this.VervoerderLBL.Location = new System.Drawing.Point(28, 65);
            this.VervoerderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VervoerderLBL.Name = "VervoerderLBL";
            this.VervoerderLBL.Size = new System.Drawing.Size(149, 29);
            this.VervoerderLBL.TabIndex = 2;
            this.VervoerderLBL.Text = "NS - Intercity";
            // 
            // totaleTijdLBL
            // 
            this.totaleTijdLBL.AutoSize = true;
            this.totaleTijdLBL.BackColor = System.Drawing.Color.Transparent;
            this.totaleTijdLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totaleTijdLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.totaleTijdLBL.Location = new System.Drawing.Point(353, 22);
            this.totaleTijdLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totaleTijdLBL.Name = "totaleTijdLBL";
            this.totaleTijdLBL.Size = new System.Drawing.Size(62, 29);
            this.totaleTijdLBL.TabIndex = 3;
            this.totaleTijdLBL.Text = "0:19";
            // 
            // aantalOverstappenLBL
            // 
            this.aantalOverstappenLBL.AutoSize = true;
            this.aantalOverstappenLBL.BackColor = System.Drawing.Color.Transparent;
            this.aantalOverstappenLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aantalOverstappenLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.aantalOverstappenLBL.Location = new System.Drawing.Point(443, 23);
            this.aantalOverstappenLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.aantalOverstappenLBL.Name = "aantalOverstappenLBL";
            this.aantalOverstappenLBL.Size = new System.Drawing.Size(39, 29);
            this.aantalOverstappenLBL.TabIndex = 4;
            this.aantalOverstappenLBL.Text = "0x";
            // 
            // PerronLBL
            // 
            this.PerronLBL.AutoSize = true;
            this.PerronLBL.BackColor = System.Drawing.Color.Transparent;
            this.PerronLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PerronLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.PerronLBL.Location = new System.Drawing.Point(484, 23);
            this.PerronLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PerronLBL.Name = "PerronLBL";
            this.PerronLBL.Size = new System.Drawing.Size(41, 29);
            this.PerronLBL.TabIndex = 5;
            this.PerronLBL.Text = "1a";
            // 
            // clockIcon
            // 
            this.clockIcon.Image = manderijntje.Properties.Resources.OrangeClock;
            this.clockIcon.Location = new System.Drawing.Point(333, 25);
            this.clockIcon.Name = "clockIcon";
            this.clockIcon.Size = new System.Drawing.Size(20, 20);
            this.clockIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clockIcon.TabIndex = 8;
            this.clockIcon.TabStop = false;
            // 
            // overstappenIcon
            // 
            this.overstappenIcon.Image = manderijntje.Properties.Resources.OverstappenOrange;
            this.overstappenIcon.Location = new System.Drawing.Point(422, 27);
            this.overstappenIcon.Name = "overstappenIcon";
            this.overstappenIcon.Size = new System.Drawing.Size(20, 20);
            this.overstappenIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.overstappenIcon.TabIndex = 9;
            this.overstappenIcon.TabStop = false;
            // 
            // Cell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.overstappenIcon);
            this.Controls.Add(this.clockIcon);
            this.Controls.Add(this.PerronLBL);
            this.Controls.Add(this.aantalOverstappenLBL);
            this.Controls.Add(this.totaleTijdLBL);
            this.Controls.Add(this.VervoerderLBL);
            this.Controls.Add(this.eindTijdLBL);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Cell";
            this.Size = new System.Drawing.Size(550, 112);
            ((System.ComponentModel.ISupportInitialize)(this.clockIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overstappenIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label eindTijdLBL;
        private System.Windows.Forms.Label VervoerderLBL;
        private System.Windows.Forms.Label totaleTijdLBL;
        private System.Windows.Forms.Label aantalOverstappenLBL;
        private System.Windows.Forms.Label PerronLBL;
        private System.Windows.Forms.PictureBox clockIcon;
        private System.Windows.Forms.PictureBox overstappenIcon;
    }
}
