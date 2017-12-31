namespace Config4Net.UI
{
    /// <summary>
    /// Options about date and time.
    /// </summary>
    public class DateTimeOptions
    {
        /// <summary>
        /// Default date format.
        /// Example: dd/MM/yyyy
        /// </summary>
        public string DefaultDateFormat { get; set; }

        /// <summary>
        /// Default time format.
        /// Example: hh:mm
        /// </summary>
        public string DefaultTimeFormat { get; set; }

        /// <summary>
        /// Default date and time format.
        /// Example: hh:mm:ss dd/MM/yyyy
        /// </summary>
        public string DefaultDateTimeFormat { get; set; }
    }
}