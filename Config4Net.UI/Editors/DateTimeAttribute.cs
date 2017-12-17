using System;

namespace Config4Net.UI.Editors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeAttribute : Attribute
    {
        /// <summary>
        /// Default datetime value, it should be correct with <see cref="Format"/>
        /// or <see cref="DateTimeOptions"/>.
        /// </summary>
        public string DefaultDateTime { get; set; }

        /// <summary>
        /// Minimun datetime that will be shown in the editor.
        /// It should be correct with <see cref="Format"/> or <see cref="DateTimeOptions"/>.
        /// </summary>
        public string MinDateTime { get; set; }

        /// <summary>
        /// Maximun datetime that will be shown in the editor.
        /// It should be correct with <see cref="Format"/> or <see cref="DateTimeOptions"/>.
        /// </summary>
        public string MaxDateTime { get; set; }

        /// <summary>
        /// The custom datetime format that will override <see cref="DateTimeOptions"/>.
        /// </summary>
        public string Format { get; set; }
    }
}