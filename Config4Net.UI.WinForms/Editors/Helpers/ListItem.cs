using System;
using System.Windows.Forms;
using Config4Net.Utils;

namespace Config4Net.UI.WinForms.Editors.Helpers
{
    public partial class ListItem : UserControl
    {
        private string _text;
        private DefaultEditor _editor;
        public event EventHandler Removed;

        public object Value => ObjectUtils.GetProperty(_editor, "Value");

        public override string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (_editor != null)
                    _editor.Text = value;
            }
        }

        public void SetEditor(DefaultEditor editor)
        {
            _editor = editor;
            Controls.Add(editor);
            editor.Dock = DockStyle.Fill;
            editor.BringToFront();
            editor.SizeChanged += (s, e) => { Width = btnRemove.Width + editor.Width; };
            Height = editor.PreferHeight;
        }

        public ListItem()
        {
            InitializeComponent();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                Width = Parent.Width;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Removed?.Invoke(this, EventArgs.Empty);
        }
    }
}