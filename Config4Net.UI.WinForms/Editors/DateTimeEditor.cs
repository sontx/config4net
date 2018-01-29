using Config4Net.UI.Editors;
using Config4Net.Utils;
using System;
using System.Globalization;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class DateTimeEditor : DefaultEditor, IDateTimeEditor
    {
        private readonly EditorHelper<DateTime> _editorHelper;

        private bool _readOnly;
        private DateTimeAttribute _dateTimeAttribute;
        private string _defaultDateTimeFormat;
        private DateTimeOptions _dateTimeOptions;
        private DateTime _value;

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

        protected virtual DateTimeOptions DateTimeOptions
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
            get => _value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () =>
                    {
                        if (value < dtContent.MinDate || value > dtContent.MaxDate) return;
                        _value = value;
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

        public void SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            _editorHelper.SetReferenceInfo(referenceInfo);
        }

        public override void SetSettings(Settings settings)
        {
            base.SetSettings(settings);
            _dateTimeAttribute = settings.Get<DateTimeAttribute>();
            if (_dateTimeAttribute != null)
            {
                dtContent.CustomFormat = string.IsNullOrWhiteSpace(_dateTimeAttribute.Format)
                    ? DefaultDateTimeFormat
                    : _dateTimeAttribute.Format;

                dtContent.MaxDate = GetDateTime(_dateTimeAttribute.MaxDateTime, DateTime.MaxValue);
                dtContent.MinDate = GetDateTime(_dateTimeAttribute.MinDateTime, DateTime.MinValue);
                dtContent.Value = GetDateTime(_dateTimeAttribute.DefaultDateTime, DateTime.Now);
            }

            var dateTimeOptionsFactory = settings.Get<IDateTimeOptionsFactory>();
            if (dateTimeOptionsFactory != null)
                DateTimeOptions = dateTimeOptionsFactory.Create();
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

        private void dtContent_ValueChanged(object sender, EventArgs e)
        {
            if (_value != dtContent.Value)
                Value = dtContent.Value;
        }
    }
}