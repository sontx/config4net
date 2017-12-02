using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class TextEditorFactory : IEditorFactory<ITextEditor>
    {
        public ITextEditor Create()
        {
            return new TextEditor();
        }
    }
}