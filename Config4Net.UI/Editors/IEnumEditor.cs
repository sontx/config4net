using System;

namespace Config4Net.UI.Editors
{
    public interface IEnumEditor : IEditor<Enum>
    {
        DisplayMode DisplayMode { get; set; }
    }
}