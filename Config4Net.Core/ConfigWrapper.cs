namespace Config4Net.Core
{
    internal class ConfigWrapper
    {
        /// <summary>
        /// The full qualified type name that uses to create an object
        /// to hold config data, it must match with config data when
        /// the library load them from file.
        /// </summary>
        public string TypeIdentify { get; set; }

        /// <summary>
        /// The real config data.
        /// </summary>
        public object ConfigObject { get; set; }
    }
}