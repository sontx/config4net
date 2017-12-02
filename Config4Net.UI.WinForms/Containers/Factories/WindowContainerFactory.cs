using Config4Net.UI.Containers;

namespace Config4Net.UI.WinForms.Containers.Factories
{
    public class WindowContainerFactory : IContainerFactory<IWindowContainer>
    {
        public IWindowContainer Create()
        {
            return new WindowContainer();
        }
    }
}