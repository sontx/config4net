using System;

namespace Config4Net.UI.Editors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MemoAttribute : Attribute
    {
        public bool WrapText { get; set; }
        public int Height { get; set; }
        public bool VerticalScroll { get; set; }
        public bool HorizontalScroll { get; set; }
    }
}