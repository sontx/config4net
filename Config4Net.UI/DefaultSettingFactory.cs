using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using LayoutOptions = Config4Net.UI.Layout.LayoutOptions;

namespace Config4Net.UI
{
    /// <summary>
    /// Default implementation for <see cref="ISettingFactory"/>.
    /// </summary>
    public class DefaultSettingFactory : ISettingFactory
    {
        /// <inheritdoc />
        public virtual LayoutOptions CreateLayoutOptions()
        {
            return new LayoutOptions
            {
                Padding = new Padding(6),
                Margin = new Padding(0)
            };
        }

        /// <inheritdoc />
        public virtual EditorAppearance CreatEditorAppearance()
        {
            return new EditorAppearance
            {
                Width = 350,
                LabelWidth = 150
            };
        }

        /// <inheritdoc />
        public virtual ContainerAppearance CreateContainerAppearance()
        {
            return new ContainerAppearance
            {
                Width = 350
            };
        }

        /// <inheritdoc />
        public virtual SizeOptions CreateSizeOptions()
        {
            return new SizeOptions
            {
                EditorSizeMode = SizeMode.Default,
                WindowContainerSizeMode = SizeMode.Default,
                GroupContainerSizeMode = SizeMode.Default
            };
        }

        /// <inheritdoc />
        public virtual DateTimeOptions CreateDateTimeOptions()
        {
            return new DateTimeOptions
            {
                DefaultDateFormat = "dd/MM/yyyy",
                DefaultTimeFormat = "HH:mm:ss",
                DefaultDateTimeFormat = "HH:mm:ss - dd/MM/yyyy"
            };
        }
    }
}