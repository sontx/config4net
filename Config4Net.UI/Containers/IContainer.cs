using System.Collections.Generic;
using Config4Net.UI.Layout;

namespace Config4Net.UI.Containers
{
    public interface IContainer : IComponent
    {
        IReadOnlyCollection<IComponent> Children { get; }
        
        ILayoutManager LayoutManager { set; get; }

        ContainerAppearance Appearance { get; set; }
        
        void AddChild(IComponent component);
    }
}