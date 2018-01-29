using System.Reflection;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// This class contains some data to help to bind a value to
    /// a property inside and object.
    /// </summary>
    public class ReferenceInfo
    {
        /// <summary>
        /// A property info that will be bound.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// The owner instance that contains this property.
        /// </summary>
        public object Source { get; set; }
    }
}