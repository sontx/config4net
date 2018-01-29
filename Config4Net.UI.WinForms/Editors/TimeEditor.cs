using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors
{
    public class TimeEditor : DateTimeEditor, ITimeEditor
    {
        protected override DateTimeOptions DateTimeOptions
        {
            get => base.DateTimeOptions;
            set
            {
                base.DateTimeOptions = value;
                DefaultDateTimeFormat = value.DefaultTimeFormat;
            }
        }

        public TimeEditor()
        {
            DefaultDateTimeFormat = "HH:mm:ss";
        }
    }
}