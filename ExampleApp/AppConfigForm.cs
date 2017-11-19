using Config4Net;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExampleApp
{
    public partial class AppConfigForm : Form
    {
        private readonly AppConfig _appConfig;

        public AppConfigForm()
        {
            InitializeComponent();

            _appConfig = ConfigPool.App<AppConfig>();

            textBox1.Text = _appConfig.Config1;
            numericUpDown1.Value = _appConfig.Config2;
            dateTimePicker1.Value = _appConfig.Config3.Config1;
            textBox2.Text = _appConfig.Config3.Config2.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _appConfig.Config1 = textBox1.Text;
            _appConfig.Config2 = (int)numericUpDown1.Value;
            _appConfig.Config3.Config1 = dateTimePicker1.Value;
            _appConfig.Config3.Config2 = Color.FromName(textBox2.Text);

            MessageBox.Show(this, @"Saved!");

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}