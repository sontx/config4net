using System.Collections.Generic;

namespace Config4Net.Types
{
    public class Select
    {
        public IList<Option> Options { get; set; }
        public int SelectedIndex { get; set; }
    }

    public class Option
    {
        public string DisplayText { get; set; }
        public object Value { get; set; }
    }
}