namespace Config4Net.UI.WinForms.Editors
{
    partial class EnumEditor
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
            this.cmbContent = new System.Windows.Forms.ComboBox();
            this.pnlWorkingArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWorkingArea
            // 
            this.pnlWorkingArea.Controls.Add(this.cmbContent);
            this.pnlWorkingArea.Size = new System.Drawing.Size(244, 23);
            // 
            // cmbContent
            // 
            this.cmbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbContent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContent.FormattingEnabled = true;
            this.cmbContent.Location = new System.Drawing.Point(0, 0);
            this.cmbContent.Name = "cmbContent";
            this.cmbContent.Size = new System.Drawing.Size(244, 24);
            this.cmbContent.TabIndex = 0;
            this.cmbContent.SelectedIndexChanged += new System.EventHandler(this.cmbContent_SelectedIndexChanged);
            // 
            // EnumEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EnumEditor";
            this.PreferHeight = 22;
            this.Size = new System.Drawing.Size(300, 23);
            this.pnlWorkingArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbContent;
    }
}
