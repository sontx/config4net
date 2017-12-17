using Config4Net.UI.Layout;
using System;
using System.Collections.Generic;

namespace Config4Net.UI.Editors
{
    public interface IListEditor : IEditor<IList<object>>
    {
        void SetItemFactory(Func<IComponent> itemFactory);

        void SetLayoutManagerFactory(Func<ILayoutManager> layoutManagerFactory);

        void SetUiBinder(IUiBinder binder);
    }
}