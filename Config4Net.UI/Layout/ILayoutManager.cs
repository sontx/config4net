using System.Collections.Generic;
using System.Drawing;

namespace Config4Net.UI.Layout
{
    public interface ILayoutManager
    {
        LayoutOptions LayoutOptions { get; set; }

        IReadOnlyCollection<IComponent> RegistedComponents { get; }

        void Register(IComponent component);

        Size ComputeWholeRegion();
    }
}