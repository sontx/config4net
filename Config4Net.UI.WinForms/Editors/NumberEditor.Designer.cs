namespace Config4Net.UI.WinForms.Editors
{
    partial class NumberEditor
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
            this.numContent = new System.Windows.Forms.NumericUpDown();
            this.pnlWorkingArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numContent)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.numContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 33);
            // 
            // numContent
            // 
            this.numContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numContent.Location = new System.Drawing.Point(0, 0);
            this.numContent.Name = "numContent";
            this.numContent.Size = new System.Drawing.Size(244, 22);
            this.numContent.TabIndex = 0;
            this.numContent.ValueChanged += new System.EventHandler(this.numContent_ValueChanged);
            // 
            // NumberEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NumberEditor";
            this.PreferHeight = 23;
            this.Size = new System.Drawing.Size(300, 33);
            this.pnlWorkingArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numContent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numContent;
    }
}
