using System.Collections.Generic;
using System.Drawing;

namespace Config4Net.UI.Layout
{
    /// <summary>
    /// Manages how the components is presented.
    /// </summary>
    public interface ILayoutManager
    {
        /// <summary>
        /// Gets or sets the <see cref="Layout.LayoutOptions"/>.
        /// </summary>
        LayoutOptions LayoutOptions { get; set; }

        /// <summary>
        /// Gets the children that are managed by this <see cref="ILayoutManager"/>.
        /// </summary>
        IReadOnlyCollection<IComponent> RegistedComponents { get; }

        /// <summary>
        /// Registers an <see cref="IComponent"/> to this <see cref="ILayoutManager"/>.
        /// </summary>
        void Register(IComponent component);

        /// <summary>
        /// Unregisters an <see cref="IComponent"/> from this <see cref="ILayoutManager"/>.
        /// </summary>
        void Unregister(IComponent component);

        /// <summary>
        /// Computes the region size that needs to show all children.
        /// </summary>
        Size ComputeWholeRegion();
    }
}