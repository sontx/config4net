namespace Config4Net.UI.Containers
{
    public interface IWindowContainer : IContainer
    {
        void Show();

        void Show(object owner);

        bool ShowDialog(object owner);
    }
}