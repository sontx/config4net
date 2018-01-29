using Config4Net.UI.Containers;
using Config4Net.UI.Editors;

namespace Config4Net.UI
{
    /// <summary>
    /// Size options.
    /// </summary>
    public class SizeOptions
    {
        /// <summary>
        /// Size options for <see cref="IEditor{T}"/>
        /// </summary>
        public SizeMode EditorSizeMode { get; set; }

        /// <summary>
        /// Size options for <see cref="IGroupContainer"/>
        /// </summary>
        public SizeMode GroupContainerSizeMode { get; set; }

        /// <summary>
        /// Size options for <see cref="IWindowContainer"/>
        /// </summary>
        public SizeMode WindowContainerSizeMode { get; set; }
    }

    /// <summary>
    /// Create <see cref="SizeOptions"/> instance.
    /// </summary>
    public interface ISizeOptionsFactory
    {
        /// <summary>
        /// Create <see cref="SizeOptions"/> instance.
        /// </summary>
        SizeOptions Create();
    }

    /// <inheritdoc />
    public class DefaultSizeOptionsFactory : ISizeOptionsFactory
    {
        /// <inheritdoc />
        public SizeOptions Create()
        {
            return new SizeOptions
            {
                EditorSizeMode = SizeMode.Default,
                WindowContainerSizeMode = SizeMode.Default,
                GroupContainerSizeMode = SizeMode.Default
            };
        }
    }
}