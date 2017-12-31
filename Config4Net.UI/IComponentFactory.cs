namespace Config4Net.UI
{
    /// <summary>
    /// Creates an <see cref="IComponent"/> instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComponentFactory<out T> where T : IComponent
    {
        /// <summary>
        /// Creates an <see cref="IComponent"/> instance.
        /// </summary>
        /// <returns>New component instance.</returns>
        T Create();
    }
}