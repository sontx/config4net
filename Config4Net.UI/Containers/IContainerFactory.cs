namespace Config4Net.UI.Containers
{
    public interface IContainerFactory<out T>: IComponentFactory<T> where T : IContainer
    {
    }
}