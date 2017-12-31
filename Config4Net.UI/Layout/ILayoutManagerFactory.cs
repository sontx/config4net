namespace Config4Net.UI.Layout
{
    /// <summary>
    /// Creates new <see cref="ILayoutManager"/>.
    /// </summary>
    public interface ILayoutManagerFactory
    {
        /// <summary>
        /// Creates new <see cref="ILayoutManager"/>.
        /// </summary>
        ILayoutManager Create();
    }
}