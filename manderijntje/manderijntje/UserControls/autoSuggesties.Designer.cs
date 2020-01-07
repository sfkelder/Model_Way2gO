namespace manderijntje
{
    partial class autoSuggestie
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
            this.autosuggestFlowControl = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // autosuggestFlowControl
            // 
            this.autosuggestFlowControl.AutoScroll = true;
            this.autosuggestFlowControl.Location = new System.Drawing.Point(0, 0);
            this.autosuggestFlowControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.autosuggestFlowControl.Name = "autosuggestFlowControl";
            this.autosuggestFlowControl.Size = new System.Drawing.Size(280, 231);
            this.autosuggestFlowControl.TabIndex = 1;
            // 
            // autoSuggestie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autosuggestFlowControl);
            this.Name = "autoSuggestie";
            this.Size = new System.Drawing.Size(280, 231);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel autosuggestFlowControl;
    }
}
