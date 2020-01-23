namespace Manderijntje
{
    partial class autoSuggestion
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
            this.autoSuggestFlowControl = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // autoSuggestFlowControl
            // 
            this.autoSuggestFlowControl.AutoScroll = true;
            this.autoSuggestFlowControl.Location = new System.Drawing.Point(0, 0);
            this.autoSuggestFlowControl.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.autoSuggestFlowControl.Name = "autoSuggestFlowControl";
            this.autoSuggestFlowControl.Size = new System.Drawing.Size(373, 289);
            this.autoSuggestFlowControl.TabIndex = 0;
            // 
            // autoSuggestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoSuggestFlowControl);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "autoSuggestion";
            this.Size = new System.Drawing.Size(373, 289);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel autoSuggestFlowControl;
    }
}
