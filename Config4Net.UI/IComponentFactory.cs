namespace Config4Net.UI
{
    public interface IComponentFactory<out T> where T : IComponent
    {
        T Create();
    }
}