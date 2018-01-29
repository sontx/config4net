using System.Reflection;
using Config4Net.UI.Containers;
using Config4Net.UI.Editors;

namespace Config4Net.UI
{
    /// <summary>
    /// Binds the underlying and settings data to the UI.
    /// </summary>
    public interface ISettingBinder
    {
        /// <summary>
        /// Binds to an <see cref="IEditor{T}"/>.
        /// </summary>
        void BindEditor(IComponent component, MemberInfo configMemberInfo, Settings settings);

        /// <summary>
        /// Binds to an <see cref="IContainer"/>.
        /// </summary>
        void BindContainer(IContainer container, MemberInfo configMemberInfo, Settings settings);
    }
}