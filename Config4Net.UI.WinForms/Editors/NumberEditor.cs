using Config4Net.UI.Editors;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class NumberEditor : DefaultEditor, INumberEditor
    {
        private readonly EditorHelper<decimal> _editorHelper;
        
        private bool _readOnly;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public decimal Value
        {
            get => numContent.Value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () => { numContent.Value = value; },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                numContent.ReadOnly = value;
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

        public NumberEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<decimal>(this);
        }
    }
}