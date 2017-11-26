using Config4Net.UI.Containers;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Config4Net.UI.Editors;

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

        public T Build<T>(object config) where T : IComponent
        {
            Precondition.ArgumentNotNull(config, nameof(config));
            Precondition.ArgumentHasAttribute(config, typeof(ShowableAttribute), nameof(config));

            var factory = GetFactoryByComponentType<T>();

            // create an editor
            if (!(factory is IContainerFactory<IWindowContainer>) && !(factory is IContainerFactory<IGroupContainer>))
            {
                var component = factory.Create();
                ApplyDescription(component, config.GetType().GetCustomAttribute<ShowableAttribute>());
                return component;
            }

            // create a container
            var container = (IContainer)factory.Create();
            var configType = config.GetType();
            var showableAttribute = configType.GetCustomAttribute<ShowableAttribute>();
            ApplyDescription(container, showableAttribute);

            foreach (var propertyInfo in configType.GetProperties())
            {
                var component = BuildRecursive(propertyInfo);
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

        private IComponent BuildRecursive(PropertyInfo type)
        {
            var showableAttribute = type.GetCustomAttribute<ShowableAttribute>();
            if (showableAttribute == null) return null;

            var propertyInfos = type.PropertyType.GetProperties();
            var hasChildrenComponents = propertyInfos.Any(propertyInfo => propertyInfo.GetCustomAttribute<ShowableAttribute>() != null);

            // create a container
            if (hasChildrenComponents)
            {
                var factory = GetFactoryByComponentType<IGroupContainer>();
                var groupContainer = factory.Create();
                ApplyDescription(groupContainer, showableAttribute);

                foreach (var propertyInfo in propertyInfos)
                {
                    var component = BuildRecursive(propertyInfo);
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
                    throw new CustomAttributeFormatException($"{nameof(showableAttribute.ComponentType)} must be defined.");
                }

                var factory = _registeredComponentFactories[showableAttribute.ComponentType];
                var component = factory.GetType().GetMethod("Create")?.Invoke(factory, BindingFlags.Instance, null, null, CultureInfo.CurrentCulture);
                ApplyDescription((IComponent)component, showableAttribute);
                return (IComponent)component;
            }
        }

        private void ApplyDescriptionToContainer(IContainer container, ShowableAttribute showableAttribute)
        {
            Precondition.PropertyNotNull(LayoutManagerFactory, nameof(LayoutManagerFactory));
            Precondition.PropertyNotNull(SettingFactory, nameof(SettingFactory));

            var layoutManager = LayoutManagerFactory.Create();
            layoutManager.LayoutOptions = SettingFactory.CreateLayoutOptions();

            container.LayoutManager = layoutManager;
            container.Appearance = SettingFactory.CreateContainerAppearance();
        }

        private void ApplyDescription(IComponent component, ShowableAttribute showableAttribute)
        {
            component.Text = showableAttribute.Label;

            // it's a container
            if (component is IContainer container)
            {
                ApplyDescriptionToContainer(container, showableAttribute);
            }
            // it's an editor
            else
            {
                component.GetType().GetProperty("Appearance")?.SetValue(component, SettingFactory.CreatEditorAppearance());
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