using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class FilePickerEditorFactory : IEditorFactory<IFilePickerEditor>
    {
        public IFilePickerEditor Create()
        {
            return new FilePickerEditor();
        }
    }
}