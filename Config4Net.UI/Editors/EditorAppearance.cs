namespace Config4Net.UI.Editors
{
    /// <summary>
    /// The appearance of <see cref="IEditor{T}"/>.
    /// </summary>
    public class EditorAppearance : AppearanceBase
    {
        /// <summary>
        /// Gets or sets label width.
        /// </summary>
        public int LabelWidth { get; set; }
    }

    /// <summary>
    /// Create <see cref="EditorAppearance"/> instance.
    /// </summary>
    public interface IEditorAppearanceFactory
    {
        /// <summary>
        /// Create <see cref="EditorAppearance"/> instance.
        /// </summary>
        EditorAppearance Create();
    }

    /// <inheritdoc />
    public class DefaultEditorAppearanceFactory : IEditorAppearanceFactory
    {
        /// <inheritdoc />
        public EditorAppearance Create()
        {
            return new EditorAppearance
            {
                Width = 350,
                LabelWidth = 150
            };
        }
    }
}