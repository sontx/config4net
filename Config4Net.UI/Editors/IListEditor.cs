using Config4Net.UI.Layout;
using System;
using System.Collections.Generic;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// The list editor that is a special editor. This editor will be used automatically when
    /// the data type is a <see cref="IList{T}"/>. Each list editor item will be an <see cref="IComponent"/>
    /// that depends on type of T in <see cref="IList{T}"/>.
    /// </summary>
    public interface IListEditor : IEditor<IList<object>>
    {
        /// <summary>
        /// Sets item factory that will be used to create new list editor item on demand.
        /// </summary>
        void SetItemFactory(Func<IComponent> itemFactory);

        /// <summary>
        /// Sets layout factory that will be used to create the layout of <see cref="IListEditor"/>.
        /// </summary>
        void SetLayoutManagerFactory(Func<ILayoutManager> layoutManagerFactory);

        /// <summary>
        /// Sets binder that helper to bind <see cref="BindInfo"/> to each list editor item.
        /// </summary>
        void SetUiBinder(IUiBinder binder);
    }
}