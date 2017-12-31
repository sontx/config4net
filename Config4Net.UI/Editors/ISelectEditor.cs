using Config4Net.Types;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// The selection editor that represents a combobox in <see cref="UI.DisplayMode.Collapse"/> mode.
    /// Each item can be an complex object.
    /// </summary>
    public interface ISelectEditor : IEditor<object>
    {
        /// <summary>
        /// Gets or sets the <see cref="ISelectEditor"/> display mode.
        /// </summary>
        DisplayMode DisplayMode { get; set; }
    }
}