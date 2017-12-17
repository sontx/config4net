using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Layout;

namespace Config4Net.UI
{
    public interface ISettingFactory
    {
        LayoutOptions CreateLayoutOptions();

        EditorAppearance CreatEditorAppearance();

        ContainerAppearance CreateContainerAppearance();

        SizeOptions CreateSizeOptions();

        DateTimeOptions CreateDateTimeOptions();
    }
}