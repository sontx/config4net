using System;

namespace Config4Net.UI.Editors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FolderPickerAttribute : Attribute
    {
        /// <summary>
        /// Just display file name instead of full file path or not.
        /// </summary>
        public bool ShowFolderName { get; set; }

        /// <summary>
        /// Enable user edit text or not, this force user must pick a file
        /// instead of typing full path in the text box.
        /// </summary>
        public bool TextEditable { get; set; }
    }
}