using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class SelectEditorFactory : IEditorFactory<ISelectEditor>
    {
        public ISelectEditor Create()
        {
            return new SelectEditor();
        }
    }
}