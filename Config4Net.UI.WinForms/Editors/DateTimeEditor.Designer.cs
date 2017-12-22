namespace Config4Net.UI.WinForms.Editors
{
    partial class DateTimeEditor
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
            this.dtContent = new System.Windows.Forms.DateTimePicker();
            this.pnlWorkingArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.dtContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 23);
            // 
            // dtContent
            // 
            this.dtContent.CustomFormat = "HH:mm:ss - dd/MM/yyyy";
            this.dtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtContent.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtContent.Location = new System.Drawing.Point(0, 0);
            this.dtContent.Name = "dtContent";
            this.dtContent.Size = new System.Drawing.Size(244, 22);
            this.dtContent.TabIndex = 0;
            this.dtContent.ValueChanged += new System.EventHandler(this.dtContent_ValueChanged);
            // 
            // DateTimeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DateTimeEditor";
            this.PreferHeight = 22;
            this.Size = new System.Drawing.Size(300, 23);
            this.pnlWorkingArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtContent;
    }
}
