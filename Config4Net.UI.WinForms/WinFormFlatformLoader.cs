using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.WinForms.Containers.Factories;
using Config4Net.UI.WinForms.Editors.Factories;
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
            UiManager.Default.RegisterFactory(typeof(IColorEditor), new ColorEditorFactory());
            UiManager.Default.RegisterFactory(typeof(ISelectEditor), new SelectEditorFactory());
            UiManager.Default.RegisterFactory(typeof(IEnumEditor), new EnumEditorFactory());
            UiManager.Default.RegisterFactory(typeof(IDateEditor), new DateEditorFactory());
            UiManager.Default.RegisterFactory(typeof(ITimeEditor), new TimeEditorFactory());
            UiManager.Default.RegisterFactory(typeof(IDateTimeEditor), new DateTimeEditorFactory());
            UiManager.Default.RegisterFactory(typeof(IFilePickerEditor), new FilePickerEditorFactory());
        }
    }
}