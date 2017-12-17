namespace Config4Net.UI.WinForms.Editors
{
    partial class ColorEditor
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
            this.pckContent = new Config4Net.UI.WinForms.Editors.Helpers.ColorPicker();
            this.pnlWorkingArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.pckContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 23);
            // 
            // pckContent
            // 
            this.pckContent.BackColor = System.Drawing.Color.Black;
            this.pckContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pckContent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pckContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pckContent.ForeColor = System.Drawing.Color.White;
            this.pckContent.Location = new System.Drawing.Point(0, 0);
            this.pckContent.Margin = new System.Windows.Forms.Padding(0);
            this.pckContent.MinimumSize = new System.Drawing.Size(2, 22);
            this.pckContent.Name = "pckContent";
            this.pckContent.Size = new System.Drawing.Size(244, 23);
            this.pckContent.TabIndex = 0;
            this.pckContent.Text = "Black";
            this.pckContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pckContent.UnknownColorAsHex = true;
            // 
            // ColorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ColorEditor";
            this.PreferHeight = 22;
            this.Size = new System.Drawing.Size(300, 23);
            this.pnlWorkingArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Helpers.ColorPicker pckContent;
    }
}
