using Config4Net.UI.Containers;

namespace Config4Net.UI.WinForms.Containers
{
    internal class GroupContainerFactory : IContainerFactory<IGroupContainer>
    {
        public IGroupContainer Create()
        {
            return new GroupContainer();
        }
    }
}