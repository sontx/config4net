using System;

namespace Config4Net.UI.Editors
{
    public interface IDateTimeEditor : IEditor<DateTime>
    {
        DateTimeOptions DateTimeOptions { get; set; }
    }
}