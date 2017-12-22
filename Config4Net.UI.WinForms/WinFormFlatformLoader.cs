using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.WinForms.Containers.Factories;
using Config4Net.UI.WinForms.Editors.Factories;
using Config4Net.UI.WinForms.Layout;

namespace Config4Net.UI.WinForms
{
    public sealed class WinFormFlatformLoader : FlatformLoader
    {
        public override void Load(UiManager uiManager)
        {
            base.Load(uiManager);
            
            uiManager.LayoutManagerFactory = new LayoutManagerFactory();
            uiManager.RegisterComponentFactory(typeof(IGroupContainer), new GroupContainerFactory());
            uiManager.RegisterComponentFactory(typeof(IWindowContainer), new WindowContainerFactory());
            uiManager.RegisterComponentFactory(typeof(INumberEditor), new NumberEditorFactory());
            uiManager.RegisterComponentFactory(typeof(ITextEditor), new TextEditorFactory());
            uiManager.RegisterComponentFactory(typeof(ICheckboxEditor), new CheckboxEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IColorEditor), new ColorEditorFactory());
            uiManager.RegisterComponentFactory(typeof(ISelectEditor), new SelectEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IEnumEditor), new EnumEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IDateEditor), new DateEditorFactory());
            uiManager.RegisterComponentFactory(typeof(ITimeEditor), new TimeEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IDateTimeEditor), new DateTimeEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IFilePickerEditor), new FilePickerEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IFolderPickerEditor), new FolderPickerEditorFactory());
            uiManager.RegisterComponentFactory(typeof(IListEditor), new ListEditorFactory());
        }
    }
}