using Config4Net.UI.Editors;

namespace Config4Net.UI.WinForms.Editors.Factories
{
    internal class ColorEditorFactory : IEditorFactory<IColorEditor>
    {
        public IColorEditor Create()
        {
            return new ColorEditor();
        }
    }
}