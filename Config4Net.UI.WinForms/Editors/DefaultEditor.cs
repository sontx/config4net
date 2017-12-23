using Config4Net.UI.Editors;
using Config4Net.UI.WinForms.Editors.Helpers;
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
        private SizeMode _sizeMode;
        private string _description;

        public EditorAppearance Appearance
        {
            get => _appearance;
            set
            {
                _appearance = value;
                ComputeSize();
            }
        }

        public virtual int PreferHeight
        {
            get => _preferHeight;
            set
            {
                _preferHeight = value;
                ComputeSize();
            }
        }

        public virtual Type DefinationType { get; set; }

        public virtual string Description
        {
            get => _description;
            set
            {
                _description = value;
                if (pnlWorkingArea.Controls.Count == 0) return;
                {
                    if (string.IsNullOrEmpty(value))
                        toolTip.RemoveAll();
                    else
                        toolTip.SetToolTip(pnlWorkingArea.Controls[0], value);
                }
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

        public SizeMode SizeMode
        {
            get => _sizeMode;
            set
            {
                _sizeMode = value;
                ComputeSize();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel WorkingArea => this.pnlWorkingArea;

        public DefaultEditor()
        {
            InitializeComponent();
#if DEBUG_COLORS
            BackColor = Color.Red;
#endif
        }

        protected void ComputeSize()
        {
            if (Appearance == null) return;

            switch (_sizeMode)
            {
                case SizeMode.Default:
                    ComputeSizeAbsolute();
                    break;

                case SizeMode.Absolute:
                    ComputeSizeAbsolute();
                    break;

                case SizeMode.MatchParent:
                    ComputeSizeMatchParent();
                    break;

                case SizeMode.WrapContent:
                    ComputeSizeWrapContent();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ComputeSizeWrapContent()
        {
            ComputeSizeAbsolute();
        }

        private void ComputeSizeMatchParent()
        {
            if (Parent == null) return;
            ComputeSize(Parent.Size.Width - Parent.Padding.Left - Parent.Padding.Right - Margin.Left - Margin.Right);
        }

        private void ComputeSizeAbsolute()
        {
            ComputeSize(_appearance.Width);
        }

        private void ComputeSize(int allowWidth)
        {
            var labelSize = TextRenderer.MeasureText(labLabel.Text, labLabel.Font);
            var numberOfLines = labelSize.Width / Appearance.LabelWidth + 1;
            Height = Math.Max(PreferHeight, numberOfLines * labelSize.Height) + Padding.Top + Padding.Bottom;
            Width = allowWidth;
            labLabel.Width = (int)(allowWidth * (_appearance.LabelWidth / (float)_appearance.Width));
        }

        protected override void OnResize(EventArgs e)
        {
            ComputeSize();
            base.OnResize(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            ComputeSize();
        }
    }
}