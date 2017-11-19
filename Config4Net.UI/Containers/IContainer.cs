using System.Collections.Generic;

namespace Config4Net.UI.Containers
{
    public interface IContainer : IComponent
    {
        IReadOnlyList<IComponent> Children { get; set; }

        DisplayDirection DisplayDirection { get; set; }

        void AddChild(IComponent component);
    }
}