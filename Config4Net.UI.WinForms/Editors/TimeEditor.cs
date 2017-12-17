using Config4Net.UI.Editors;
using Config4Net.Utils;

namespace Config4Net.UI.WinForms.Editors
{
    public class TimeEditor : DateTimeEditor, ITimeEditor
    {
        private DateTimeOptions _dateTimeOptions;

        public override DateTimeOptions DateTimeOptions
        {
            get => _dateTimeOptions;
            set
            {
                Precondition.ArgumentNotNull(value, nameof(DateTimeOptions));
                _dateTimeOptions = value;
                DefaultDateTimeFormat = value.DefaultTimeFormat;
            }
        }

        public TimeEditor()
        {
            DefaultDateTimeFormat = "HH:mm:ss";
        }
    }
}