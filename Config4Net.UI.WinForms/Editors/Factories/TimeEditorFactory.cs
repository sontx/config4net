using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class TimeEditorFactory : IEditorFactory<ITimeEditor>
    {
        public ITimeEditor Create()
        {
            return new TimeEditor();
        }
    }
}