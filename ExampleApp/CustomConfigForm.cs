using Config4Net.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExampleApp
{
    public partial class CustomConfigForm : Form
    {
        private readonly CustomConfig _customConfig;

        public CustomConfigForm()
        {
            InitializeComponent();

            _customConfig = ConfigPool.Get<CustomConfig>();

            textBox1.Text = _customConfig.Config1;
            dateTimePicker1.Value = _customConfig.Config2.Config1;
            textBox2.Text = _customConfig.Config2.Config2.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _customConfig.Config1 = textBox1.Text;
            _customConfig.Config2.Config1 = dateTimePicker1.Value;
            _customConfig.Config2.Config2 = Color.FromName(textBox2.Text);

            MessageBox.Show(this, @"Saved!");

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}