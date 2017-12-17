using Config4Net.UI.Editors;
using Config4Net.Utils;

namespace Config4Net.UI.WinForms.Editors
{
    public class DateEditor : DateTimeEditor, IDateEditor
    {
        private DateTimeOptions _dateTimeOptions;

        public override DateTimeOptions DateTimeOptions
        {
            get => _dateTimeOptions;
            set
            {
                Precondition.ArgumentNotNull(value, nameof(DateTimeOptions));
                _dateTimeOptions = value;
                DefaultDateTimeFormat = value.DefaultDateFormat;
            }
        }

        public DateEditor()
        {
            DefaultDateTimeFormat = "dd/MM/yyyy";
        }
    }
}