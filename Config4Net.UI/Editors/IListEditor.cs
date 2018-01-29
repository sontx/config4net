using Config4Net.UI.Layout;
using System;
using System.Collections.Generic;
using System.Reflection;

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
        /// Sets binder that helper to bind <see cref="Settings"/> to each list editor item.
        /// </summary>
        void SetSettingBinder(ISettingBinder binder);

        /// <summary>
        /// Sets settings factory that will be used to create the <see cref="Settings"/>.
        /// </summary>
        /// <param name="settingFactory"></param>
        void SetSettingFactory(Func<object, PropertyInfo, Settings> settingFactory);
    }
}