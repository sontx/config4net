namespace Config4Net.UI.Editors
{
    public interface IEditorFactory<out T>: IComponentFactory<T> where T : IComponent
    {
    }
}