namespace Config4Net.UI.WinForms.Editors
{
    partial class TextEditor
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
            this.txtContent = new System.Windows.Forms.TextBox();
            this.pnlWorkingArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.txtContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 23);
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(244, 22);
            this.txtContent.TabIndex = 0;
            this.txtContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContent_KeyDown);
            this.txtContent.Leave += new System.EventHandler(this.txtContent_Leave);
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TextEditor";
            this.PreferHeight = 22;
            this.Size = new System.Drawing.Size(300, 23);
            this.pnlWorkingArea.ResumeLayout(false);
            this.pnlWorkingArea.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtContent;
    }
}
