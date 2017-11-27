using Config4Net.UI.Editors;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    [Designer(typeof(UserControlDesigner))]
    public partial class DefaultEditor : UserControl
    {
        private int _preferHeight = -1;
        private string _text;
        private EditorAppearance _appearance;

        public EditorAppearance Appearance
        {
            get => _appearance;
            set
            {
                _appearance = value;
                if (value != null)
                {
                    ComputeSize();
                }
            }
        }

        public int PreferHeight
        {
            get => _preferHeight;
            set
            {
                _preferHeight = value;
                ComputeSize();
            }
        }

        public override string Text
        {
            get => _text;
            set
            {
                labLabel.Text = value;
                _text = value;
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel WorkingArea => this.pnlWorkingArea;

        public DefaultEditor()
        {
            InitializeComponent();
        }

        protected void ComputeSize()
        {
            if (Appearance == null) return;

            var labelSize = TextRenderer.MeasureText(labLabel.Text, labLabel.Font);
            var numberOfLines = labelSize.Width / Appearance.LabelWidth + 1;
            labLabel.Width = Appearance.LabelWidth;
            Height = Math.Max(PreferHeight, numberOfLines * labelSize.Height) + Padding.Top + Padding.Bottom;
            Width = Appearance.Width;
        }

        protected override void OnResize(EventArgs e)
        {
            ComputeSize();
            base.OnResize(e);
        }
    }
}