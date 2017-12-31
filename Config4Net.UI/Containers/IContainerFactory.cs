namespace Config4Net.UI.Containers
{
    /// <summary>
    /// Create an <see cref="IContainer"/> object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainerFactory<out T>: IComponentFactory<T> where T : IContainer
    {
    }
}