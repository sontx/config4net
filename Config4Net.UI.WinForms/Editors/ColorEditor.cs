using Config4Net.UI.Editors;
using System.Drawing;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class ColorEditor : DefaultEditor, IColorEditor
    {
        private readonly EditorHelper<Color> _editorHelper;

        private bool _readOnly;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public Color Value
        {
            get => pckContent.Color;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () => { pckContent.Color = value; },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                pckContent.Enabled = !value;
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
            get => pckContent.Height;
            set => base.PreferHeight = value;
        }

        public ColorEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<Color>(this);
        }
    }
}