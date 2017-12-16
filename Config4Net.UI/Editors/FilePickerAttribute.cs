using System;

namespace Config4Net.UI.Editors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilePickerAttribute : Attribute
    {
        /// <summary>
        /// The file filters to display in the dialog box. For example: MP3 Files|*.mp3|All Files|*.*
        /// </summary>
        public string FileFilter { get; set; }

        /// <summary>
        /// Just display file name instead of full file path or not.
        /// </summary>
        public bool ShowFileName { get; set; }

        /// <summary>
        /// Enable user edit text or not, this force user must pick a file
        /// instead of typing full path in the text box.
        /// </summary>
        public bool TextEditable { get; set; }
    }
}