using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class DateTimeEditorFactory : IEditorFactory<IDateTimeEditor>
    {
        public IDateTimeEditor Create()
        {
            return new DateTimeEditor();
        }
    }
}