using System;

namespace Config4Net.UI
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ShowableAttribute : Attribute
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public string Condition { get; set; }
        public Type ComponentType { get; set; }

        public ShowableAttribute(string label = null)
        {
            this.Label = label;
        }
    }
}