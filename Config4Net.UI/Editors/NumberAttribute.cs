using System;

namespace Config4Net.UI.Editors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumberAttribute : Attribute
    {
        /// <summary>
        /// Maximun value.
        /// </summary>
        public double Max { get; set; } = 100;

        /// <summary>
        /// Minimun value.
        /// </summary>
        public double Min { get; set; } = 0;

        /// <summary>
        /// Number of decimal places to display.
        /// </summary>
        public int DecimalPlaces { get; set; } = 0;
    }
}