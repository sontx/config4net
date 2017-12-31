namespace Config4Net.UI.Editors
{
    /// <summary>
    /// The editor factory.
    /// </summary>
    public interface IEditorFactory<out T>: IComponentFactory<T> where T : IComponent
    {
    }
}