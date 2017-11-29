using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors
{
    internal class CheckboxEditorFactory : IEditorFactory<ICheckboxEditor>
    {
        public ICheckboxEditor Create()
        {
            return new CheckboxEditor();
        }
    }
}