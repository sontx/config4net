using Config4Net.UI.Containers;
using Config4Net.UI.Editors.Definations;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Config4Net.UI
{
    public sealed class UiManager
    {
        #region Default Instance

        private static UiManager _defaultInstance;

        public static UiManager Default => _defaultInstance ?? (_defaultInstance = new UiManager());

        #endregion Default Instance

        // componentType - factory
        private readonly Dictionary<Type, object> _registeredComponentFactories;

        public ILayoutManagerFactory LayoutManagerFactory { get; set; }
        public ISettingFactory SettingFactory { get; set; }
        public bool AllowAutoCreateInstanceIfMissing { get; set; } = true;

        public T Build<T>(object config) where T : IContainer
        {
            Precondition.ArgumentNotNull(config, nameof(config));
            Precondition.ArgumentHasAttribute(config, typeof(ShowableAttribute), nameof(config));
            Precondition.PropertyNotNull(SettingFactory, nameof(SettingFactory));

            var factory = GetFactoryByComponentType<T>();
            var sizeOptions = SettingFactory.CreateSizeOptions();

            // create a container
            var container = (IContainer)factory.Create();
            var configType = config.GetType();
            ApplyDescription(container, configType, sizeOptions);

            foreach (var propertyInfo in configType.GetProperties())
            {
                var component = BuildRecursive(config, propertyInfo, sizeOptions);
                if (component == null) continue;

                container.AddChild(component);
            }

            return (T)container;
        }

        public void RegisterFactory(Type componentType, object factory)
        {
            Precondition.ArgumentNotNull(componentType, nameof(componentType));
            Precondition.ArgumentNotNull(factory, nameof(factory));
            Precondition.ArgumentCompatibleType(componentType, typeof(IComponent), nameof(componentType));

            _registeredComponentFactories.Add(componentType, factory);
        }

        private IComponent BuildRecursive(object parentInstance, PropertyInfo propertyInfo, SizeOptions sizeOptions)
        {
            var showableAttribute = propertyInfo.GetCustomAttribute<ShowableAttribute>();
            if (showableAttribute == null) return null;

            var propertyInfos = propertyInfo.PropertyType.GetProperties();
            var hasChildrenComponents = propertyInfos.Any(ipropertyInfo =>
                ipropertyInfo.GetCustomAttribute<ShowableAttribute>() != null);

            // create a container
            if (hasChildrenComponents)
            {
                var factory = GetFactoryByComponentType<IGroupContainer>();
                var groupContainer = factory.Create();
                ApplyDescription(groupContainer, propertyInfo, sizeOptions);
                var currentInstance = propertyInfo.GetValue(parentInstance);
                if (currentInstance == null && AllowAutoCreateInstanceIfMissing)
                {
                    propertyInfo.SetValue(parentInstance,
                        currentInstance = Activator.CreateInstance(propertyInfo.PropertyType));
                }

                foreach (var childPropertyInfo in propertyInfos)
                {
                    var component = BuildRecursive(currentInstance, childPropertyInfo, sizeOptions);
                    if (component == null) continue;

                    groupContainer.AddChild(component);
                }

                return groupContainer;
            }
            // create an editor
            else
            {
                if (showableAttribute.ComponentType == null)
                {
                    throw new CustomAttributeFormatException(
                        $"{nameof(showableAttribute.ComponentType)} must be defined.");
                }

                var factory = _registeredComponentFactories[showableAttribute.ComponentType];

                var component = factory.GetType().GetMethod("Create")?.Invoke(factory, BindingFlags.Instance, null,
                    null, CultureInfo.CurrentCulture);

                component?.GetType().GetMethod("SetReferenceInfo")?.Invoke(
                    component,
                    BindingFlags.Instance,
                    null,
                    new[] { parentInstance, propertyInfo },
                    CultureInfo.CurrentCulture);

                ApplyDescription((IComponent)component, propertyInfo, sizeOptions);
                return (IComponent)component;
            }
        }

        private void ApplyDescription(IComponent component, MemberInfo memberInfo, SizeOptions sizeOptions)
        {
            var showableAttribute = memberInfo.GetCustomAttribute<ShowableAttribute>();

            component.Text = string.IsNullOrEmpty(showableAttribute.Label)
                ? StringUtils.ToFriendlyString(memberInfo.Name)
                : showableAttribute.Label;

            component.Description = showableAttribute.Description;

            // it's a container
            if (component is IContainer container)
            {
                ApplyDescriptionToContainer(container, sizeOptions);
            }
            // it's an editor
            else
            {
                ObjectUtils.SetProperty(component, "Appearance", SettingFactory.CreatEditorAppearance());
                component.SizeMode = sizeOptions.EditorSizeMode;

                var definationAttribute = memberInfo.GetCustomAttribute<DefinationAttribute>();
                if (definationAttribute != null)
                {
                    ObjectUtils.SetProperty(component, "DefinationType", definationAttribute.Value);
                }
            }
        }

        private void ApplyDescriptionToContainer(IContainer container, SizeOptions sizeOptions)
        {
            Precondition.PropertyNotNull(LayoutManagerFactory, nameof(LayoutManagerFactory));
            Precondition.PropertyNotNull(SettingFactory, nameof(SettingFactory));

            var layoutManager = LayoutManagerFactory.Create();
            layoutManager.LayoutOptions = SettingFactory.CreateLayoutOptions();

            container.LayoutManager = layoutManager;
            container.Appearance = SettingFactory.CreateContainerAppearance();

            if (container is IGroupContainer)
            {
                container.SizeMode = sizeOptions.GroupContainerSizeMode;
            }
            else // IWindowContainer
            {
                container.SizeMode = sizeOptions.WindowContainerSizeMode;
            }
        }

        private IComponentFactory<T> GetFactoryByComponentType<T>() where T : IComponent
        {
            var registeredFactory = _registeredComponentFactories[typeof(T)];
            return (IComponentFactory<T>)registeredFactory;
        }

        public UiManager()
        {
            _registeredComponentFactories = new Dictionary<Type, object>();
            SettingFactory = new DefaultSettingFactory();
        }
    }
}