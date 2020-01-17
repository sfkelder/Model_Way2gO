namespace manderijntje
{
    partial class Indexpanel
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
            this.indexLBL = new System.Windows.Forms.Label();
            this.stationLBL = new System.Windows.Forms.Label();
            this.stationOnRouteLBL = new System.Windows.Forms.Label();
            this.connectionLBL = new System.Windows.Forms.Label();
            this.routeTravelLBL = new System.Windows.Forms.Label();
            this.whitePanel = new System.Windows.Forms.Panel();
            this.routeToTravelPanel = new System.Windows.Forms.Panel();
            this.connectionPanel = new System.Windows.Forms.Panel();
            this.stationOnRoutePanel = new System.Windows.Forms.Panel();
            this.stationPanel = new System.Windows.Forms.Panel();
            this.whitePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // indexLBL
            // 
            this.indexLBL.AutoSize = true;
            this.indexLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.indexLBL.Location = new System.Drawing.Point(20, 22);
            this.indexLBL.Name = "indexLBL";
            this.indexLBL.Size = new System.Drawing.Size(98, 37);
            this.indexLBL.TabIndex = 0;
            this.indexLBL.Text = "Index";
            // 
            // stationLBL
            // 
            this.stationLBL.AutoSize = true;
            this.stationLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stationLBL.Location = new System.Drawing.Point(54, 75);
            this.stationLBL.Name = "stationLBL";
            this.stationLBL.Size = new System.Drawing.Size(79, 25);
            this.stationLBL.TabIndex = 1;
            this.stationLBL.Text = "Station";
            // 
            // stationOnRouteLBL
            // 
            this.stationOnRouteLBL.AutoSize = true;
            this.stationOnRouteLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stationOnRouteLBL.Location = new System.Drawing.Point(54, 115);
            this.stationOnRouteLBL.Name = "stationOnRouteLBL";
            this.stationOnRouteLBL.Size = new System.Drawing.Size(172, 25);
            this.stationOnRouteLBL.TabIndex = 2;
            this.stationOnRouteLBL.Text = "Station on Route";
            // 
            // connectionLBL
            // 
            this.connectionLBL.AutoSize = true;
            this.connectionLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionLBL.Location = new System.Drawing.Point(68, 150);
            this.connectionLBL.Name = "connectionLBL";
            this.connectionLBL.Size = new System.Drawing.Size(121, 25);
            this.connectionLBL.TabIndex = 3;
            this.connectionLBL.Text = "Connection";
            // 
            // routeTravelLBL
            // 
            this.routeTravelLBL.AutoSize = true;
            this.routeTravelLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeTravelLBL.Location = new System.Drawing.Point(68, 184);
            this.routeTravelLBL.Name = "routeTravelLBL";
            this.routeTravelLBL.Size = new System.Drawing.Size(159, 25);
            this.routeTravelLBL.TabIndex = 4;
            this.routeTravelLBL.Text = "Route to Travel";
            // 
            // whitePanel
            // 
            this.whitePanel.BackColor = System.Drawing.Color.White;
            this.whitePanel.Controls.Add(this.routeToTravelPanel);
            this.whitePanel.Controls.Add(this.connectionPanel);
            this.whitePanel.Controls.Add(this.stationOnRoutePanel);
            this.whitePanel.Controls.Add(this.stationPanel);
            this.whitePanel.Controls.Add(this.indexLBL);
            this.whitePanel.Controls.Add(this.routeTravelLBL);
            this.whitePanel.Controls.Add(this.stationLBL);
            this.whitePanel.Controls.Add(this.connectionLBL);
            this.whitePanel.Controls.Add(this.stationOnRouteLBL);
            this.whitePanel.Location = new System.Drawing.Point(3, 3);
            this.whitePanel.Name = "whitePanel";
            this.whitePanel.Size = new System.Drawing.Size(384, 232);
            this.whitePanel.TabIndex = 5;
            // 
            // routeToTravelPanel
            // 
            this.routeToTravelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.routeToTravelPanel.Location = new System.Drawing.Point(29, 192);
            this.routeToTravelPanel.Name = "routeToTravelPanel";
            this.routeToTravelPanel.Size = new System.Drawing.Size(34, 10);
            this.routeToTravelPanel.TabIndex = 7;
            // 
            // connectionPanel
            // 
            this.connectionPanel.BackColor = System.Drawing.Color.Gray;
            this.connectionPanel.Location = new System.Drawing.Point(29, 158);
            this.connectionPanel.Name = "connectionPanel";
            this.connectionPanel.Size = new System.Drawing.Size(34, 10);
            this.connectionPanel.TabIndex = 6;
            // 
            // stationOnRoutePanel
            // 
            this.stationOnRoutePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.stationOnRoutePanel.Location = new System.Drawing.Point(29, 117);
            this.stationOnRoutePanel.Name = "stationOnRoutePanel";
            this.stationOnRoutePanel.Size = new System.Drawing.Size(20, 20);
            this.stationOnRoutePanel.TabIndex = 6;
            // 
            // stationPanel
            // 
            this.stationPanel.BackColor = System.Drawing.Color.Gray;
            this.stationPanel.Location = new System.Drawing.Point(29, 76);
            this.stationPanel.Name = "stationPanel";
            this.stationPanel.Size = new System.Drawing.Size(20, 20);
            this.stationPanel.TabIndex = 5;
            // 
            // Indexpanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.whitePanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Indexpanel";
            this.Size = new System.Drawing.Size(390, 238);
            this.whitePanel.ResumeLayout(false);
            this.whitePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label indexLBL;
        private System.Windows.Forms.Label stationLBL;
        private System.Windows.Forms.Label stationOnRouteLBL;
        private System.Windows.Forms.Label connectionLBL;
        private System.Windows.Forms.Label routeTravelLBL;
        private System.Windows.Forms.Panel whitePanel;
        private System.Windows.Forms.Panel stationOnRoutePanel;
        private System.Windows.Forms.Panel stationPanel;
        private System.Windows.Forms.Panel routeToTravelPanel;
        private System.Windows.Forms.Panel connectionPanel;
    }
}
