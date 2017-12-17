using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using LayoutOptions = Config4Net.UI.Layout.LayoutOptions;

namespace Config4Net.UI
{
    public class DefaultSettingFactory : ISettingFactory
    {
        public virtual LayoutOptions CreateLayoutOptions()
        {
            return new LayoutOptions
            {
                Padding = new Padding(6),
                Margin = new Padding(0)
            };
        }

        public virtual EditorAppearance CreatEditorAppearance()
        {
            return new EditorAppearance
            {
                Width = 350,
                LabelWidth = 150
            };
        }

        public virtual ContainerAppearance CreateContainerAppearance()
        {
            return new ContainerAppearance
            {
                Width = 350
            };
        }

        public virtual SizeOptions CreateSizeOptions()
        {
            return new SizeOptions
            {
                EditorSizeMode = SizeMode.Default,
                WindowContainerSizeMode = SizeMode.Default,
                GroupContainerSizeMode = SizeMode.Default
            };
        }

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