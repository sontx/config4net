using System;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Containers
{
    public partial class ChooserForm : Form
    {
        protected Panel WorkspacePanel => panel1;
        
        protected void AdjustButtons(Padding padding, Padding margin)
        {
            panel2.Margin = Compatibility.ToWinFormPadding(margin);
            panel2.Padding = Compatibility.ToWinFormPadding(padding);
        }

        public ChooserForm()
        {
            InitializeComponent();
        }

        protected virtual void OnCancel()
        {
        }

        protected virtual void OnAccept()
        {
        }

        protected virtual bool OnValidate()
        {
            return true;
        }

        protected virtual void OnAcceptButtonClicked()
        {
            if (!OnValidate()) return;

            OnAccept();
            DialogResult = DialogResult.OK;

            Close();
        }

        protected virtual void OnCancelButtonClicked()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OnAcceptButtonClicked();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DialogResult != DialogResult.OK)
                OnCancel();
            base.OnClosed(e);
        }
    }
}