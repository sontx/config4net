using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Config4Net.UI
{
    public sealed class UiManager : IUiBinder, ICopyable<UiManager>
    {
        #region Default Instance

        private static UiManager _defaultInstance;

        public static UiManager Default => _defaultInstance ?? (_defaultInstance = Create());

        public static UiManager Create()
        {
            return new UiManager();
        }

        #endregion Default Instance

        private readonly ComponentManager _componentManager;

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
            var container = _componentManager.CreateComponentFromComponentType<IContainer>(typeof(T));
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

            return (T) container;
        }

        public void RegisterComponentFactory(Type componentType, object factory)
        {
            _componentManager.RegisterComponentFactory(componentType, factory);
        }

        public void RegisterDefaultComponentType(Type propertyType, Type componentType)
        {
            _componentManager.RegisterDefaultComponentType(propertyType, componentType);
        }

        public void Copy(UiManager source)
        {
            Precondition.ArgumentNotNull(source, nameof(source));
            LayoutManagerFactory = source.LayoutManagerFactory;
            SettingFactory = source.SettingFactory;
            AllowAutoCreateInstanceIfMissing = source.AllowAutoCreateInstanceIfMissing;
            _componentManager.Copy(source._componentManager);
        }

        private UiManager()
        {
            _componentManager = new ComponentManager();
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

            if (component is IDateTimeEditor dateTimeEditor)
                dateTimeEditor.DateTimeOptions = SettingFactory.CreateDateTimeOptions();
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
            var groupContainer = _componentManager.CreateComponentFromComponentType<IGroupContainer>();
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
            var component = ObjectUtils.IsGenericList(propertyInfo.PropertyType)
                ? CreateListEditor(showableInfo.ComponentType, propertyInfo.PropertyType)
                : CreateNormalEditor(showableInfo.ComponentType, propertyInfo.PropertyType);

            BindEditor(component, new EditorBindInfo
            {
                ShowableInfo = ShowableInfo.From(propertyInfo),
                DefinationInfo = DefinationInfo.From(propertyInfo),
                SizeOptions = sizeOptions,
                ReferenceInfo = new ReferenceInfo
                {
                    PropertyInfo = propertyInfo,
                    Source = parentInstance
                }
            });

            return component;
        }

        private IComponent CreateNormalEditor(Type componentType, Type propertyType)
        {
            return componentType != null
                ? _componentManager.CreateComponentFromComponentType<IComponent>(componentType)
                : _componentManager.CreateComponentFromPropertyType<IComponent>(propertyType);
        }

        private IComponent CreateListEditor(Type componentType, Type propertyType)
        {
            var listEditor = _componentManager.CreateComponentFromComponentType<IListEditor>();
            listEditor.SetUiBinder(this);
            listEditor.SetItemFactory(() => CreateNormalEditor(componentType, propertyType));
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

        #endregion Helper Methods
    }
}