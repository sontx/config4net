using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Layout;

namespace Config4Net.UI
{
    /// <summary>
    /// Creates settings.
    /// </summary>
    public interface ISettingFactory
    {
        /// <summary>
        /// Creates <see cref="LayoutOptions"/>.
        /// </summary>
        LayoutOptions CreateLayoutOptions();

        /// <summary>
        /// Creates <see cref="EditorAppearance"/>.
        /// </summary>
        EditorAppearance CreatEditorAppearance();

        /// <summary>
        /// Creates <see cref="ContainerAppearance"/>.
        /// </summary>
        ContainerAppearance CreateContainerAppearance();

        /// <summary>
        /// Creates <see cref="SizeOptions"/>.
        /// </summary>
        SizeOptions CreateSizeOptions();

        /// <summary>
        /// Creates <see cref="DateTimeOptions"/>.
        /// </summary>
        DateTimeOptions CreateDateTimeOptions();
    }
}