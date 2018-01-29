using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class CheckboxEditor : DefaultEditor, ICheckboxEditor
    {
        private readonly EditorHelper<bool> _editorHelper;

        private bool _readOnly;
        private string _description;
        private bool _value;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public override string Description
        {
            get => _description;
            set
            {
                _description = value;
                chkContent.Text = value;
            }
        }

        public bool Value
        {
            get => _value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () =>
                    {
                        _value = value;
                        chkContent.Checked = value;
                    },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                chkContent.Enabled = !value;
                _readOnly = value;
            }
        }

        public void SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            _editorHelper.SetReferenceInfo(referenceInfo);
        }

        public void Bind()
        {
            _editorHelper.Bind();
        }

        public void Reset()
        {
            _editorHelper.Reset();
        }

        public override int PreferHeight
        {
            get => chkContent.Height;
            set => base.PreferHeight = value;
        }

        public CheckboxEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<bool>(this);
        }

        private void chkContent_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_value != chkContent.Checked)
                Value = chkContent.Checked;
        }
    }
}