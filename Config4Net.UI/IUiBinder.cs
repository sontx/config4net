using Config4Net.UI.Containers;

namespace Config4Net.UI
{
    public interface IUiBinder
    {
        void BindEditor(IComponent component, EditorBindInfo bindInfo);

        void BindContainer(IContainer container, ContainerBindInfo bindInfo);
    }
}