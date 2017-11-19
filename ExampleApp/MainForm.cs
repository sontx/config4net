using Config4Net;
using System;
using System.Windows.Forms;
using Config4Net.Core;

namespace ExampleApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupConfig();
        }

        private void SetupConfig()
        {
            ConfigPool.RegisterFactory(new AppConfigFactory());
            ConfigPool.RegisterFactory(new CustomConfigFactory());
        }

        private void btnOpenAppSetting_Click(object sender, EventArgs e)
        {
            new AppConfigForm().Show(this);
        }

        private void btnOpenCustomSetting1_Click(object sender, EventArgs e)
        {
            new CustomConfigForm().Show(this);
        }
    }
}