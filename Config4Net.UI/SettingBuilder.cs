using System.Reflection;
using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Layout;
using Config4Net.Utils;

namespace Config4Net.UI
{
    internal class SettingBuilder
    {
        private readonly Settings _settings = new Settings();

        public SettingBuilder SetConfigMemberInfo(MemberInfo configMemberInfo)
        {
            var showableAttribute = configMemberInfo.GetCustomAttribute<ShowableAttribute>();

            string name;
            string text;

            if (showableAttribute != null)
            {
                name = string.IsNullOrWhiteSpace(showableAttribute.Name) ? configMemberInfo.Name : showableAttribute.Name;
                text = showableAttribute.Label;
            }
            else
            {
                name = configMemberInfo.Name;
                text = StringUtils.ToFriendlyString(configMemberInfo.Name);
            }

            _settings.Put("name", name);
            _settings.Put("text", text);
            _settings.Put("description", showableAttribute?.Description);
            _settings.Put("componentType", showableAttribute?.ComponentType);

            ImportAttributes(configMemberInfo);

            return this;
        }

        private void ImportAttributes(MemberInfo configMemberInfo)
        {
            var customAttributes = configMemberInfo.GetCustomAttributes(false);
            foreach (var customAttribute in customAttributes)
            {
                var name = StringUtils.ToVariableName(customAttribute.GetType().Name);
                _settings.Put(name, customAttribute);
            }
        }

        public SettingBuilder SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            _settings.Put(nameof(referenceInfo), referenceInfo);
            return this;
        }

        public SettingBuilder SetLayoutManagerFactory(ILayoutManagerFactory layoutManagerFactory)
        {
            _settings.Put(nameof(layoutManagerFactory), layoutManagerFactory);
            return this;
        }

        public SettingBuilder SetSizeOptionsFactory(ISizeOptionsFactory sizeOptionsFactory)
        {
            _settings.Put(nameof(sizeOptionsFactory), sizeOptionsFactory);
            return this;
        }

        public SettingBuilder SetDateTimeOptionsFactory(IDateTimeOptionsFactory dateTimeOptionsFactory)
        {
            _settings.Put(nameof(dateTimeOptionsFactory), dateTimeOptionsFactory);
            return this;
        }

        public SettingBuilder SetContainerAppearanceFactory(IContainerAppearanceFactory containerAppearanceFactory)
        {
            _settings.Put(nameof(containerAppearanceFactory), containerAppearanceFactory);
            return this;
        }

        public SettingBuilder SetEditorAppearanceFactory(IEditorAppearanceFactory editorAppearanceFactory)
        {
            _settings.Put(nameof(editorAppearanceFactory), editorAppearanceFactory);
            return this;
        }

        public SettingBuilder SetLayoutOptionsFactory(ILayoutOptionsFactory layoutOptionsFactory)
        {
            _settings.Put(nameof(layoutOptionsFactory), layoutOptionsFactory);
            return this;
        }

        public Settings Build()
        {
            return _settings;
        }
    }
}