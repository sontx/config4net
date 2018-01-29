namespace Config4Net.UI.Layout
{
    /// <summary>
    /// Layout options.
    /// </summary>
    public class LayoutOptions
    {
        /// <summary>
        /// Gets or sets the layout orientation.
        /// </summary>
        public Orientation Orientation { get; set; }
        
        /// <summary>
        /// Gets or sets the layout padding.
        /// </summary>
        public Padding Padding { get; set; }

        /// <summary>
        /// Gets or sets the layout margin.
        /// </summary>
        public Padding Margin { get; set; }
    }

    /// <summary>
    /// Create <see cref="LayoutOptions"/> instance.
    /// </summary>
    public interface ILayoutOptionsFactory
    {
        /// <summary>
        /// Create <see cref="LayoutOptions"/> instance.
        /// </summary>
        LayoutOptions Create();
    }

    /// <inheritdoc />
    public class DefaultLayoutOptionsFactory : ILayoutOptionsFactory
    {
        /// <inheritdoc />
        public LayoutOptions Create()
        {
            return new LayoutOptions
            {
                Padding = new Padding(6),
                Margin = new Padding(0)
            };
        }
    }
}