using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.WinForms.Containers;
using Config4Net.UI.WinForms.Editors;
using Config4Net.UI.WinForms.Layout;

namespace Config4Net.UI.WinForms
{
    public sealed class WinFormFlatformLoader : IFlatformLoader
    {
        public void Load()
        {
            UiManager.Default.LayoutManagerFactory = new LayoutManagerFactory();
            UiManager.Default.RegisterFactory(typeof(IGroupContainer), new GroupContainerFactory());
            UiManager.Default.RegisterFactory(typeof(IWindowContainer), new WindowContainerFactory());
            UiManager.Default.RegisterFactory(typeof(INumberEditor), new NumberEditorFactory());
            UiManager.Default.RegisterFactory(typeof(ITextEditor), new TextEditorFactory());
            UiManager.Default.RegisterFactory(typeof(ICheckboxEditor), new CheckboxEditorFactory());
        }
    }
}