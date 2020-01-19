namespace manderijntje
{
    partial class transferCell
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
            this.directionLBL = new System.Windows.Forms.Label();
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
            this.departuretimeLBL.Location = new System.Drawing.Point(36, 67);
            this.departuretimeLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.departuretimeLBL.Name = "departuretimeLBL";
            this.departuretimeLBL.Size = new System.Drawing.Size(96, 36);
            this.departuretimeLBL.TabIndex = 0;
            this.departuretimeLBL.Text = "12:01";
            // 
            // stationLBL
            // 
            this.stationLBL.AutoSize = true;
            this.stationLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stationLBL.Location = new System.Drawing.Point(221, 39);
            this.stationLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.stationLBL.Name = "stationLBL";
            this.stationLBL.Size = new System.Drawing.Size(289, 36);
            this.stationLBL.TabIndex = 1;
            this.stationLBL.Text = "Rotterdam Centraal";
            // 
            // directionLBL
            // 
            this.directionLBL.AutoSize = true;
            this.directionLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directionLBL.Location = new System.Drawing.Point(221, 102);
            this.directionLBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.directionLBL.Name = "directionLBL";
            this.directionLBL.Size = new System.Drawing.Size(328, 36);
            this.directionLBL.TabIndex = 3;
            this.directionLBL.Text = "NS Sprinter to Schiphol";
            // 
            // lineImage
            // 
            this.lineImage.Location = new System.Drawing.Point(148, 0);
            this.lineImage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lineImage.Name = "lineImage";
            this.lineImage.Size = new System.Drawing.Size(61, 178);
            this.lineImage.TabIndex = 4;
            this.lineImage.TabStop = false;
            // 
            // typetransportIcon
            // 
            this.typetransportIcon.Image = global::manderijntje.Properties.Resources.OrangeTrain;
            this.typetransportIcon.Location = new System.Drawing.Point(752, 44);
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
            this.Controls.Add(this.directionLBL);
            this.Controls.Add(this.stationLBL);
            this.Controls.Add(this.departuretimeLBL);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "transferCell";
            this.Size = new System.Drawing.Size(808, 178);
            ((System.ComponentModel.ISupportInitialize)(this.lineImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typetransportIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label departuretimeLBL;
        private System.Windows.Forms.Label stationLBL;
        private System.Windows.Forms.Label directionLBL;
        private System.Windows.Forms.PictureBox lineImage;
        private System.Windows.Forms.PictureBox typetransportIcon;
    }
}
