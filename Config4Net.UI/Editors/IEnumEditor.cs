using System;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// The enum editor that looks like <see cref="ISelectEditor"/> but for <see cref="Enum"/>.
    /// </summary>
    public interface IEnumEditor : IEditor<Enum>
    {
        /// <summary>
        /// Gets or sets <see cref="DisplayMode"/>.
        /// </summary>
        DisplayMode DisplayMode { get; set; }
    }
}