using Config4Net.UI.Containers;
using Config4Net.UI.Editors;

namespace Config4Net.UI
{
    /// <summary>
    /// Binds the underlying and settings data to the UI.
    /// </summary>
    public interface IUiBinder
    {
        /// <summary>
        /// Binds to an <see cref="IEditor{T}"/>.
        /// </summary>
        void BindEditor(IComponent component, EditorBindInfo bindInfo);

        /// <summary>
        /// Binds to an <see cref="IContainer"/>.
        /// </summary>
        void BindContainer(IContainer container, ContainerBindInfo bindInfo);
    }
}