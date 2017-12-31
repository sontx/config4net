namespace Config4Net.UI.Containers
{
    /// <summary>
    /// The window container that can display standalone.
    /// </summary>
    public interface IWindowContainer : IContainer
    {
        /// <summary>
        /// Show the window.
        /// </summary>
        void Show();

        /// <summary>
        /// Show the window.
        /// </summary>
        /// <param name="owner">The window parent.</param>
        void Show(object owner);

        /// <summary>
        /// Show the window as a modal dialog.
        /// </summary>
        /// <param name="owner">The window parent.</param>
        /// <returns>True if the user accepts.</returns>
        bool ShowDialog(object owner);
    }
}