using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class ListEditorFactory : IEditorFactory<IListEditor>
    {
        public IListEditor Create()
        {
            return new ListEditor();
        }
    }
}