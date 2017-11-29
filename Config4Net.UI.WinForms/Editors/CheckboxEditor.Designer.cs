namespace Config4Net.UI.WinForms.Editors
{
    partial class CheckboxEditor
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
            this.chkContent = new System.Windows.Forms.CheckBox();
            this.pnlWorkingArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.chkContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 23);
            // 
            // chkContent
            // 
            this.chkContent.AutoSize = true;
            this.chkContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkContent.Location = new System.Drawing.Point(0, 0);
            this.chkContent.Name = "chkContent";
            this.chkContent.Size = new System.Drawing.Size(244, 23);
            this.chkContent.TabIndex = 0;
            this.chkContent.UseVisualStyleBackColor = true;
            // 
            // CheckboxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CheckboxEditor";
            this.PreferHeight = 22;
            this.Size = new System.Drawing.Size(300, 23);
            this.pnlWorkingArea.ResumeLayout(false);
            this.pnlWorkingArea.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkContent;
    }
}
