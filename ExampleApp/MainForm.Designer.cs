namespace ExampleApp
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenAppSetting = new System.Windows.Forms.Button();
            this.btnOpenCustomSetting1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOpenAppSetting
            // 
            this.btnOpenAppSetting.Location = new System.Drawing.Point(40, 36);
            this.btnOpenAppSetting.Name = "btnOpenAppSetting";
            this.btnOpenAppSetting.Size = new System.Drawing.Size(139, 42);
            this.btnOpenAppSetting.TabIndex = 0;
            this.btnOpenAppSetting.Text = "Open App Setting...";
            this.btnOpenAppSetting.UseVisualStyleBackColor = true;
            this.btnOpenAppSetting.Click += new System.EventHandler(this.btnOpenAppSetting_Click);
            // 
            // btnOpenCustomSetting1
            // 
            this.btnOpenCustomSetting1.Location = new System.Drawing.Point(185, 36);
            this.btnOpenCustomSetting1.Name = "btnOpenCustomSetting1";
            this.btnOpenCustomSetting1.Size = new System.Drawing.Size(139, 42);
            this.btnOpenCustomSetting1.TabIndex = 0;
            this.btnOpenCustomSetting1.Text = "Open Custom Setting 1...";
            this.btnOpenCustomSetting1.UseVisualStyleBackColor = true;
            this.btnOpenCustomSetting1.Click += new System.EventHandler(this.btnOpenCustomSetting1_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 43);
            this.label1.TabIndex = 1;
            this.label1.Text = "Change some settings and close app then reopen to see your previous settings ;)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 167);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpenCustomSetting1);
            this.Controls.Add(this.btnOpenAppSetting);
            this.Name = "MainForm";
            this.Text = "ExampleApp";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenAppSetting;
        private System.Windows.Forms.Button btnOpenCustomSetting1;
        private System.Windows.Forms.Label label1;
    }
}

