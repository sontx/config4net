using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using System.Reflection;

namespace Config4Net.UI
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

    /// <summary>
    /// A bundle of settings that will be bound to the component.
    /// </summary>
    public abstract class BindInfo
    {
        /// <summary>
        /// Size options.<seealso cref="UI.SizeOptions"/>
        /// </summary>
        public SizeOptions SizeOptions { get; set; }

        /// <summary>
        /// Settings that was extracted from <see cref="ShowableAttribute"/>.
        /// </summary>
        public ShowableInfo ShowableInfo { get; set; }
    }

    /// <summary>
    /// Binding info of <see cref="IContainer"/>.
    /// </summary>
    public class ContainerBindInfo : BindInfo
    {
    }

    /// <summary>
    /// Binding info of <see cref="IEditor{T}"/>
    /// </summary>
    public class EditorBindInfo : BindInfo
    {
        /// <summary>
        /// Defination info.
        /// </summary>
        public DefinationInfo DefinationInfo { get; set; }

        /// <summary>
        /// Reference info that will be used to synchronize with the UI and underlying data.
        /// </summary>
        public ReferenceInfo ReferenceInfo { get; set; }
    }
}