using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class FolderPickerEditorFactory : IEditorFactory<IFolderPickerEditor>
    {
        public IFolderPickerEditor Create()
        {
            return new FolderPickerEditor();
        }
    }
}