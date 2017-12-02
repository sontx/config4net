using Config4Net.UI.WinForms.Editors.Helpers;

namespace Config4Net.UI.WinForms.Editors
{
    partial class DefaultEditor
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
            this.labLabel = new Config4Net.UI.WinForms.Editors.Helpers.NoPaddingLabel();
            this.pnlWorkingArea = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // labLabel
            // 
            this.labLabel.AutoEllipsis = true;
            this.labLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.labLabel.Location = new System.Drawing.Point(0, 0);
            this.labLabel.Margin = new System.Windows.Forms.Padding(0);
            this.labLabel.Name = "labLabel";
            this.labLabel.RightAlignment = false;
            this.labLabel.Size = new System.Drawing.Size(56, 35);
            this.labLabel.TabIndex = 0;
            this.labLabel.Text = "Config:";
            this.labLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWorkingArea.Location = new System.Drawing.Point(56, 0);
            this.pnlWorkingArea.Margin = new System.Windows.Forms.Padding(0);
            this.pnlWorkingArea.Name = "pnlWorkingArea";
            this.pnlWorkingArea.Size = new System.Drawing.Size(364, 35);
            this.pnlWorkingArea.TabIndex = 1;
            // 
            // DefaultEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlWorkingArea);
            this.Controls.Add(this.labLabel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DefaultEditor";
            this.Size = new System.Drawing.Size(420, 35);
            this.ResumeLayout(false);

        }

        #endregion

        private NoPaddingLabel labLabel;
        protected System.Windows.Forms.Panel pnlWorkingArea;
    }
}
