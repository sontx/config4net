using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class DateEditorFactory : IEditorFactory<IDateEditor>
    {
        public IDateEditor Create()
        {
            return new DateEditor();
        }
    }
}