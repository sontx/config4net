using Config4Net.UI.Editors;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class NumberEditor : DefaultEditor, INumberEditor
    {
        private readonly EditorHelper<decimal> _editorHelper;
        
        private bool _readOnly;
        private decimal _value;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public decimal Value
        {
            get => _value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () =>
                    {
                        _value = value;
                        numContent.Value = value;
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
                numContent.ReadOnly = value;
                _readOnly = value;
            }
        }

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            var numberAttribute = propertyInfo.GetCustomAttribute<NumberAttribute>();
            if (numberAttribute != null)
            {
                numContent.Maximum = (decimal) numberAttribute.Max;
                numContent.Minimum = (decimal) numberAttribute.Min;
                numContent.DecimalPlaces = numberAttribute.DecimalPlaces;
            }

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
            get => numContent.Height;
            set => base.PreferHeight = value;
        }

        public NumberEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<decimal>(this);
        }

        private void numContent_ValueChanged(object sender, System.EventArgs e)
        {
            if (_value != numContent.Value)
                Value = numContent.Value;
        }
    }
}