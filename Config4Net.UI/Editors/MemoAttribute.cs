using System;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// Addition info for <see cref="ITextEditor"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MemoAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets whether <see cref="ITextEditor"/> should wrap text.
        /// </summary>
        public bool WrapText { get; set; }

        /// <summary>
        /// Gets or sets a specific height of the <see cref="ITextEditor"/>.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets whether the vertical scrollbar is visible.
        /// </summary>
        public bool VerticalScroll { get; set; }

        /// <summary>
        /// Gets or sets whether the horizontal scrollbar is visible.
        /// </summary>
        public bool HorizontalScroll { get; set; }
    }
}