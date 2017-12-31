using System.Collections.Generic;
using Config4Net.UI.Layout;

namespace Config4Net.UI.Containers
{
    /// <summary>
    /// A container that can contain other <see cref="IComponent"/>.
    /// </summary>
    public interface IContainer : IComponent
    {
        /// <summary>
        /// Gets children <see cref="IComponent"/>.
        /// </summary>
        IReadOnlyCollection<IComponent> Children { get; }
        
        /// <summary>
        /// Gets or sets <see cref="ILayoutManager"/>.
        /// </summary>
        ILayoutManager LayoutManager { set; get; }

        /// <summary>
        /// Gets or sets the <see cref="ContainerAppearance"/>.
        /// </summary>
        ContainerAppearance Appearance { get; set; }
        
        /// <summary>
        /// Adds child <see cref="IComponent"/>.
        /// </summary>
        void AddChild(IComponent component);
    }
}