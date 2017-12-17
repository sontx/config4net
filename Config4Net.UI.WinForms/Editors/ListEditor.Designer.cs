namespace Config4Net.UI.WinForms.Editors
{
    partial class ListEditor
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
            this.labContent = new System.Windows.Forms.Label();
            this.pnlWorkingArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.labContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 23);
            // 
            // labContent
            // 
            this.labContent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labContent.Location = new System.Drawing.Point(0, 0);
            this.labContent.Name = "labContent";
            this.labContent.Size = new System.Drawing.Size(244, 23);
            this.labContent.TabIndex = 0;
            this.labContent.Text = "Click to edit...";
            this.labContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labContent.Click += new System.EventHandler(this.labContent_Click);
            // 
            // ListEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ListEditor";
            this.PreferHeight = 22;
            this.Size = new System.Drawing.Size(300, 23);
            this.pnlWorkingArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labContent;
    }
}
