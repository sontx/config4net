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
                Margin = new Padding(2)
            };
        }

        public virtual EditorAppearance CreatEditorAppearance()
        {
            return new EditorAppearance
            {
                Width = 300,
                LabelWidth = 200
            };
        }

        public virtual ContainerAppearance CreateContainerAppearance()
        {
            return new ContainerAppearance
            {
            };
        }
    }
}