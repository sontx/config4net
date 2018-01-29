using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors
{
    public class DateEditor : DateTimeEditor, IDateEditor
    {
        protected override DateTimeOptions DateTimeOptions
        {
            get => base.DateTimeOptions;
            set
            {
                base.DateTimeOptions = value;
                DefaultDateTimeFormat = value.DefaultDateFormat;
            }
        }

        public DateEditor()
        {
            DefaultDateTimeFormat = "dd/MM/yyyy";
        }
    }
}