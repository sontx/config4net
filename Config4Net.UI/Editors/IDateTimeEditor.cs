using System;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// The date time editor.
    /// </summary>
    public interface IDateTimeEditor : IEditor<DateTime>
    {
        /// <summary>
        /// Gets or sets the <see cref="UI.DateTimeOptions"/>
        /// </summary>
        DateTimeOptions DateTimeOptions { get; set; }
    }
}