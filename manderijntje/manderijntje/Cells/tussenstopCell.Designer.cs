namespace manderijntje
{
    partial class tussenstopCell
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
            this.vertrekTijdLBL = new System.Windows.Forms.Label();
            this.stationLBL = new System.Windows.Forms.Label();
            this.perronLBL = new System.Windows.Forms.Label();
            this.richtingLBL = new System.Windows.Forms.Label();
            this.lijnImage = new System.Windows.Forms.PictureBox();
            this.typeVervoerIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lijnImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeVervoerIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // vertrekTijdLBL
            // 
            this.vertrekTijdLBL.AutoSize = true;
            this.vertrekTijdLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vertrekTijdLBL.Location = new System.Drawing.Point(27, 57);
            this.vertrekTijdLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vertrekTijdLBL.Name = "vertrekTijdLBL";
            this.vertrekTijdLBL.Size = new System.Drawing.Size(76, 29);
            this.vertrekTijdLBL.TabIndex = 0;
            this.vertrekTijdLBL.Text = "12:01";
            // 
            // stationLBL
            // 
            this.stationLBL.AutoSize = true;
            this.stationLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stationLBL.Location = new System.Drawing.Point(166, 31);
            this.stationLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stationLBL.Name = "stationLBL";
            this.stationLBL.Size = new System.Drawing.Size(239, 29);
            this.stationLBL.TabIndex = 1;
            this.stationLBL.Text = "Rotterdam Centraal";
            // 
            // perronLBL
            // 
            this.perronLBL.AutoSize = true;
            this.perronLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.perronLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.perronLBL.Location = new System.Drawing.Point(478, 31);
            this.perronLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.perronLBL.Name = "perronLBL";
            this.perronLBL.Size = new System.Drawing.Size(111, 29);
            this.perronLBL.TabIndex = 2;
            this.perronLBL.Text = "Spoor 1a";
            // 
            // richtingLBL
            // 
            this.richtingLBL.AutoSize = true;
            this.richtingLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richtingLBL.Location = new System.Drawing.Point(166, 82);
            this.richtingLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.richtingLBL.Name = "richtingLBL";
            this.richtingLBL.Size = new System.Drawing.Size(264, 29);
            this.richtingLBL.TabIndex = 3;
            this.richtingLBL.Text = "NS Sprinter to Schiphol";
            // 
            // lijnImage
            // 
            this.lijnImage.Location = new System.Drawing.Point(111, 0);
            this.lijnImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lijnImage.Name = "lijnImage";
            this.lijnImage.Size = new System.Drawing.Size(46, 142);
            this.lijnImage.TabIndex = 4;
            this.lijnImage.TabStop = false;
            // 
            // typeVervoerIcon
            // 
            this.typeVervoerIcon.Location = new System.Drawing.Point(555, 79);
            this.typeVervoerIcon.Name = "typeVervoerIcon";
            this.typeVervoerIcon.Size = new System.Drawing.Size(24, 33);
            this.typeVervoerIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.typeVervoerIcon.TabIndex = 5;
            this.typeVervoerIcon.TabStop = false;
            // 
            // tussenstopCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.typeVervoerIcon);
            this.Controls.Add(this.lijnImage);
            this.Controls.Add(this.richtingLBL);
            this.Controls.Add(this.perronLBL);
            this.Controls.Add(this.stationLBL);
            this.Controls.Add(this.vertrekTijdLBL);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "tussenstopCell";
            this.Size = new System.Drawing.Size(606, 142);
            ((System.ComponentModel.ISupportInitialize)(this.lijnImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeVervoerIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label vertrekTijdLBL;
        private System.Windows.Forms.Label stationLBL;
        private System.Windows.Forms.Label perronLBL;
        private System.Windows.Forms.Label richtingLBL;
        private System.Windows.Forms.PictureBox lijnImage;
        private System.Windows.Forms.PictureBox typeVervoerIcon;
    }
}
