using Config4Net.UI.Editors;
using System;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class DateTimeEditor : DefaultEditor, IDateTimeEditor
    {
        private readonly EditorHelper<DateTime> _editorHelper;

        private bool _readOnly;
        private string _dateFormat = "HH:mm:ss - dd/MM/yyyy";

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public string DateFormat
        {
            get => _dateFormat;
            set
            {
                _dateFormat = value;
                dtContent.CustomFormat = value;
            }
        }

        public DateTime Value
        {
            get => dtContent.Value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () =>
                    {
                        dtContent.Value = value < dtContent.MinDate || value > dtContent.MaxDate ? DateTime.Now : value;
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
                dtContent.Enabled = value;
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
            get => dtContent.Height;
            set => base.PreferHeight = value;
        }

        public DateTimeEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<DateTime>(this);
        }
    }
}