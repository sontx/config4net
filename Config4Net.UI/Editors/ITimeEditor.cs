using System;

namespace Config4Net.UI.Editors
{
    public interface ITimeEditor : IEditor<DateTime>
    {
        string DateFormat { get; set; }
    }
}