using Config4Net.UI.Layout;

namespace Config4Net.UI.WinForms.Layout
{
    internal sealed class LayoutManagerFactory : ILayoutManagerFactory
    {
        public ILayoutManager Create()
        {
            return new FlowLayoutManager();
        }
    }
}