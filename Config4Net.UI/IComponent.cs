namespace Config4Net.UI
{
    /// <summary>
    /// Base component that is an element in the UI.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the component text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the component description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the component size mode.
        /// </summary>
        SizeMode SizeMode { get; set; }

        /// <summary>
        /// Binds the component value to underlying value.
        /// </summary>
        void Bind();

        /// <summary>
        /// Set <see cref="Settings"/>.
        /// </summary>
        void SetSettings(Settings settings);
    }
}