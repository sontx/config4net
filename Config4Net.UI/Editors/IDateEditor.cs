using System;

namespace Config4Net.UI.Editors
{
    public interface IDateEditor : IEditor<DateTime>
    {
        string DateFormat { get; set; }
    }
}