using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class EnumEditorFactory : IEditorFactory<IEnumEditor>
    {
        public IEnumEditor Create()
        {
            return new EnumEditor();
        }
    }
}