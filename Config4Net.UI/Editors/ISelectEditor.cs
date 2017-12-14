using Config4Net.Types;

namespace Config4Net.UI.Editors
{
    public interface ISelectEditor : IEditor<object>
    {
        DisplayMode DisplayMode { get; set; }
    }
}