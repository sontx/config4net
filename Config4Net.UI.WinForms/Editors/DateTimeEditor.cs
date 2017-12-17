using Config4Net.UI.Editors;
using System;
using System.Globalization;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class DateTimeEditor : DefaultEditor, IDateTimeEditor
    {
        private readonly EditorHelper<DateTime> _editorHelper;

        private bool _readOnly;
        private DateTimeAttribute _dateTimeAttribute;
        private string _defaultDateTimeFormat;
        private DateTimeOptions _dateTimeOptions;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        protected string DefaultDateTimeFormat
        {
            get => _defaultDateTimeFormat;
            set
            {
                _defaultDateTimeFormat = value;
                dtContent.CustomFormat =
                    string.IsNullOrWhiteSpace(_dateTimeAttribute?.Format)
                        ? value
                        : _dateTimeAttribute.Format;
            }
        }

        public virtual DateTimeOptions DateTimeOptions
        {
            get => _dateTimeOptions;
            set
            {
                Precondition.ArgumentNotNull(value, nameof(DateTimeOptions));
                _dateTimeOptions = value;
                DefaultDateTimeFormat = value.DefaultDateTimeFormat;
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
                        if (value >= dtContent.MinDate && value <= dtContent.MaxDate)
                            dtContent.Value = value;
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
            _dateTimeAttribute = propertyInfo.GetCustomAttribute<DateTimeAttribute>();
            if (_dateTimeAttribute != null)
            {
                dtContent.CustomFormat = string.IsNullOrWhiteSpace(_dateTimeAttribute.Format)
                    ? DefaultDateTimeFormat
                    : _dateTimeAttribute.Format;

                dtContent.MaxDate = GetDateTime(_dateTimeAttribute.MaxDateTime, DateTime.MaxValue);
                dtContent.MinDate = GetDateTime(_dateTimeAttribute.MinDateTime, DateTime.MinValue);
                dtContent.Value = GetDateTime(_dateTimeAttribute.DefaultDateTime, DateTime.Now);
            }

            _editorHelper.SetReferenceInfo(source, propertyInfo);
        }

        private DateTime GetDateTime(string dateTimeAsString, DateTime defaultValue)
        {
            return DateTime.TryParseExact(
                dateTimeAsString,
                dtContent.CustomFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTime)
                ? dateTime
                : defaultValue;
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