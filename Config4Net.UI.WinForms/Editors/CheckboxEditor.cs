using Config4Net.UI.Editors;
using System.Reflection;
using System.Text;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class CheckboxEditor : DefaultEditor, ICheckboxEditor
    {
        private readonly EditorHelper<bool> _editorHelper;
        
        private bool _readOnly;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public bool Value
        {
            get => chkContent.Checked;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () => { chkContent.Checked = value; },
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

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            _editorHelper.SetReferenceInfo(source, propertyInfo);
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
    }
}