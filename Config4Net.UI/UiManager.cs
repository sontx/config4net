using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Config4Net.UI
{
    public sealed class UiManager : IUiBinder
    {
        #region Default Instance

        private static UiManager _defaultInstance;

        public static UiManager Default => _defaultInstance ?? (_defaultInstance = new UiManager());

        #endregion Default Instance

        // componentType - factory
        private readonly Dictionary<Type, object> _registeredComponentFactories;

        #region Properties

        public ILayoutManagerFactory LayoutManagerFactory { get; set; }
        public ISettingFactory SettingFactory { get; set; }
        public bool AllowAutoCreateInstanceIfMissing { get; set; } = true;

        #endregion Properties

        public T Build<T>(object config) where T : IContainer
        {
            Precondition.ArgumentNotNull(config, nameof(config));
            Precondition.ArgumentHasAttribute(config, typeof(ShowableAttribute), nameof(config));
            Precondition.PropertyNotNull(SettingFactory, nameof(SettingFactory));

            var sizeOptions = SettingFactory.CreateSizeOptions();

            // create a container
            var container = (IContainer)GetFactoryByComponentType<T>().Create();
            var configType = config.GetType();

            BindContainer(container, new ContainerBindInfo
            {
                ShowableInfo = ShowableInfo.From(configType),
                SizeOptions = sizeOptions
            });

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

        public UiManager()
        {
            _registeredComponentFactories = new Dictionary<Type, object>();
            SettingFactory = new DefaultSettingFactory();
        }

        #region IUiBinder Implements

        public void BindEditor(IComponent component, EditorBindInfo bindInfo)
        {
            BindComponent(component, bindInfo);

            var editor = EditorWrapper.From(component);
            editor.Appearance = SettingFactory.CreatEditorAppearance();
            editor.DefinationType = bindInfo.DefinationInfo?.Value;
            editor.SetReferenceInfo(bindInfo.ReferenceInfo.Source, bindInfo.ReferenceInfo.PropertyInfo);
        }

        public void BindContainer(IContainer container, ContainerBindInfo bindInfo)
        {
            BindComponent(container, bindInfo);

            Precondition.PropertyNotNull(LayoutManagerFactory, nameof(LayoutManagerFactory));
            Precondition.PropertyNotNull(SettingFactory, nameof(SettingFactory));

            var layoutManager = LayoutManagerFactory.Create();
            layoutManager.LayoutOptions = SettingFactory.CreateLayoutOptions();

            container.LayoutManager = layoutManager;
            container.Appearance = SettingFactory.CreateContainerAppearance();

            if (container is IGroupContainer)
                container.SizeMode = bindInfo.SizeOptions.GroupContainerSizeMode;
            else
                container.SizeMode = bindInfo.SizeOptions.WindowContainerSizeMode;
        }

        #endregion IUiBinder Implements

        #region Helper Methods

        private IComponent BuildRecursive(object parentInstance, PropertyInfo propertyInfo, SizeOptions sizeOptions)
        {
            var showableInfo = ShowableInfo.From(propertyInfo);
            if (showableInfo == null) return null;

            var propertyInfos = propertyInfo.PropertyType.GetProperties();
            var hasChildrenComponents = propertyInfos.Any(ipropertyInfo =>
                ipropertyInfo.GetCustomAttribute<ShowableAttribute>() != null);

            return hasChildrenComponents
                ? BuildContainer(parentInstance, propertyInfo, sizeOptions, propertyInfos)
                : BuildEditor(parentInstance, propertyInfo, sizeOptions, showableInfo);
        }

        private IComponent BuildContainer(
            object parentInstance,
            PropertyInfo propertyInfo,
            SizeOptions sizeOptions,
            PropertyInfo[] propertyInfos)
        {
            var groupContainer = GetFactoryByComponentType<IGroupContainer>().Create();
            BindContainer(groupContainer, new ContainerBindInfo
            {
                ShowableInfo = ShowableInfo.From(propertyInfo),
                SizeOptions = sizeOptions
            });

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

        private IComponent BuildEditor(
            object parentInstance,
            PropertyInfo propertyInfo,
            SizeOptions sizeOptions,
            ShowableInfo showableInfo)
        {
            if (showableInfo.ComponentType == null)
            {
                throw new CustomAttributeFormatException(
                    $"{nameof(showableInfo.ComponentType)} must be defined.");
            }

            var component = ObjectUtils.IsGenericList(propertyInfo.PropertyType)
                ? CreateListEditor(showableInfo.ComponentType)
                : CreateNormalEditor(showableInfo.ComponentType);

            BindEditor(component, new EditorBindInfo
            {
                ShowableInfo = ShowableInfo.From(propertyInfo),
                DefinationInfo = DefinationInfo.From(propertyInfo),
                SizeOptions = sizeOptions,
                ReferenceInfo = new ReferenceInfo { PropertyInfo = propertyInfo, Source = parentInstance }
            });

            return component;
        }

        private IComponent CreateNormalEditor(Type componentType)
        {
            return ComponentFactoryWrapper.From(_registeredComponentFactories[componentType]).Create();
        }

        private IComponent CreateListEditor(Type componentType)
        {
            var listFactory = ComponentFactoryWrapper.From(_registeredComponentFactories[typeof(IListEditor)]);
            var listEditor = (IListEditor)listFactory.Create();
            listEditor.SetUiBinder(this);
            listEditor.SetItemFactory(ComponentFactoryWrapper.From(_registeredComponentFactories[componentType]).Create);
            listEditor.SetLayoutManagerFactory(() =>
            {
                var layoutManager = LayoutManagerFactory.Create();
                layoutManager.LayoutOptions = SettingFactory.CreateLayoutOptions();
                return layoutManager;
            });

            return listEditor;
        }

        private void BindComponent(IComponent component, BindInfo bindInfo)
        {
            component.Text = bindInfo.ShowableInfo.Label;
            component.Description = bindInfo.ShowableInfo.Description;
            component.SizeMode = bindInfo.SizeOptions.EditorSizeMode;
        }

        private IComponentFactory<T> GetFactoryByComponentType<T>() where T : IComponent
        {
            var registeredFactory = _registeredComponentFactories[typeof(T)];
            return (IComponentFactory<T>)registeredFactory;
        }

        #endregion Helper Methods
    }
}