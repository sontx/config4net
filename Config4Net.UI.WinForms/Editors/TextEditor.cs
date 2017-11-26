using Config4Net.UI.Editors;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class TextEditor : DefaultEditor, ITextEditor
    {
        private readonly EditorHelper<string> _editorHelper;

        private string _value;
        private bool _readOnly;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public string Value
        {
            get => _value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () => { txtContent.Text = value; _value = value; },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                txtContent.ReadOnly = value;
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

        public TextEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<string>(this);
        }
    }
}