using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System.Reflection;

namespace Config4Net.UI
{
    internal class SettingBinder : ISettingBinder
    {
        public void BindEditor(IComponent component, MemberInfo configMemberInfo, Settings settings)
        {
            var editor = EditorWrapper.From(component);
            editor.Appearance = settings.Get<IEditorAppearanceFactory>().Create();
            editor.SetReferenceInfo(settings.Get<ReferenceInfo>());
            BindComponent(component, configMemberInfo, settings);
        }

        public void BindContainer(IContainer container, MemberInfo configMemberInfo, Settings settings)
        {
            container.LayoutManager = GetLayoutManager(settings);
            container.Appearance = settings.Get<IContainerAppearanceFactory>().Create();
            BindComponent(container, configMemberInfo, settings);
        }

        private static void BindComponent(IComponent component, MemberInfo configMemberInfo, Settings settings)
        {
            var name = settings.Get<string>("name");
            var text = settings.Get<string>("text");

            if (string.IsNullOrWhiteSpace(name))
                name = configMemberInfo.Name;
            if (string.IsNullOrWhiteSpace(text))
                text = StringUtils.ToFriendlyString(configMemberInfo.Name);

            component.Text = text;
            component.Name = name;
            component.Description = settings.Get("description", "");
            component.SizeMode = GetSizeMode(component, settings.Get<ISizeOptionsFactory>());

            component.SetSettings(settings);
        }

        private static ILayoutManager GetLayoutManager(Settings settings)
        {
            var layoutOptions = settings.Get<ILayoutOptionsFactory>().Create();
            var layoutManager = settings.Get<ILayoutManagerFactory>().Create();
            layoutManager.LayoutOptions = layoutOptions;
            return layoutManager;
        }

        private static SizeMode GetSizeMode(IComponent component, ISizeOptionsFactory sizeOptionsFactory)
        {
            var sizeOptions = sizeOptionsFactory.Create();

            if (component is IWindowContainer)
                return sizeOptions.WindowContainerSizeMode;
            if (component is IGroupContainer)
                return sizeOptions.GroupContainerSizeMode;
            return sizeOptions.EditorSizeMode;
        }
    }
}