using Config4Net.UI.Containers;

namespace Config4Net.UI.WinForms.Containers
{
    public class WindowContainerFactory : IContainerFactory<IWindowContainer>
    {
        public IWindowContainer Create()
        {
            return new WindowContainer();
        }
    }
}