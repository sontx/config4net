using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Config4Net.UI
{
    /// <inheritdoc />
    /// <summary>
    /// Builds and manages UI from a giving config data.
    /// </summary>
    public sealed class UiManager : ICopyable<UiManager>
    {
        #region Default Instance

        private static UiManager _defaultInstance;

        /// <summary>
        /// Default instance of <see cref="UiManager"/> with default settings.
        /// </summary>
        public static UiManager Default => _defaultInstance ?? (_defaultInstance = Create());

        /// <summary>
        /// Create new <see cref="UiManager"/> instance.
        /// </summary>
        /// <returns>New <see cref="UiManager"/> instance.</returns>
        public static UiManager Create()
        {
            return new UiManager();
        }

        #endregion Default Instance

        private readonly ComponentManager _componentManager;
        private SettingBinder _settingBinder;

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ISettingBinder"/>.
        /// </summary>
        public ISettingBinder SettingBinder { get; set; }

        #region Factories

        /// <summary>
        /// Gets or sets <see cref="ILayoutManagerFactory"/>.
        /// </summary>
        public ILayoutManagerFactory LayoutManagerFactory { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ILayoutOptionsFactory"/>.
        /// </summary>
        public ILayoutOptionsFactory LayoutOptionsFactory { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ISizeOptionsFactory"/>.
        /// </summary>
        public ISizeOptionsFactory SizeOptionsFactory { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IDateTimeOptionsFactory"/>
        /// </summary>
        public IDateTimeOptionsFactory DateTimeOptionsFactory { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IContainerAppearanceFactory"/>.
        /// </summary>
        public IContainerAppearanceFactory ContainerAppearanceFactory { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IEditorAppearanceFactory"/>.
        /// </summary>
        public IEditorAppearanceFactory EditorAppearanceFactory { get; set; }

        #endregion Factories

        /// <summary>
        /// Gets or sets whether to create new property instance if it's null.
        /// Default is true.
        /// </summary>
        public bool AllowAutoCreateInstanceIfMissing { get; set; } = true;

        #endregion Properties

        private UiManager()
        {
            _componentManager = new ComponentManager();
            _settingBinder = new SettingBinder();

            SettingBinder = new SettingBinder();
            LayoutOptionsFactory = new DefaultLayoutOptionsFactory();
            SizeOptionsFactory = new DefaultSizeOptionsFactory();
            DateTimeOptionsFactory = new DefaultDateTimeOptionsFactory();
            ContainerAppearanceFactory = new DefaultContainerAppearanceFactory();
            EditorAppearanceFactory = new DefaultEditorAppearanceFactory();
        }

        /// <summary>
        /// Builds a UI from config data and binds its properties to this the UI.
        /// </summary>
        /// <param name="config">
        /// The config data that will be used to determine the UI structure and binds
        /// its properties to the UI.
        /// </param>
        /// <typeparam name="T">
        /// Type of the UI which is an <see cref="IContainer"/>.
        /// </typeparam>
        /// <returns>The UI instance that is created from the config data.</returns>
        public T Build<T>(object config) where T : IContainer
        {
            Precondition.ArgumentNotNull(config, nameof(config));
            Precondition.PropertyNotNull(SettingBinder, nameof(SettingBinder));
            CheckFactories();

            var container = _componentManager.CreateComponentFromComponentType<IContainer>(typeof(T));
            var configType = config.GetType();

            var settings = CreateSettingBuilder(configType).Build();
            SettingBinder.BindContainer(container, configType, settings);

            foreach (var propertyInfo in configType.GetProperties())
            {
                var component = BuildRecursive(config, propertyInfo);
                if (component == null) continue;

                container.AddChild(component);
            }

            return (T)container;
        }

        private IComponent BuildRecursive(object parentInstance, PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute<ShowableAttribute>() == null) return null;

            var childrenPropertyInfos = propertyInfo.PropertyType.GetProperties();
            var hasChildrenComponents = childrenPropertyInfos.Any(ipropertyInfo =>
                ipropertyInfo.GetCustomAttribute<ShowableAttribute>() != null);

            return hasChildrenComponents
                ? BuildContainer(parentInstance, propertyInfo, childrenPropertyInfos)
                : BuildEditor(parentInstance, propertyInfo);
        }

        private IComponent BuildContainer(object parentInstance, PropertyInfo propertyInfo, PropertyInfo[] propertyInfos)
        {
            var groupContainer = _componentManager.CreateComponentFromComponentType<IGroupContainer>();

            var settings = CreateSettingBuilder(propertyInfo).Build();
            SettingBinder.BindContainer(groupContainer, propertyInfo, settings);

            var currentInstance = propertyInfo.GetValue(parentInstance);
            if (currentInstance == null && AllowAutoCreateInstanceIfMissing)
            {
                propertyInfo.SetValue(
                    parentInstance,
                    currentInstance = Activator.CreateInstance(propertyInfo.PropertyType));
            }

            foreach (var childPropertyInfo in propertyInfos)
            {
                var component = BuildRecursive(currentInstance, childPropertyInfo);
                if (component == null) continue;

                groupContainer.AddChild(component);
            }

            return groupContainer;
        }

        private IComponent BuildEditor(object parentInstance, PropertyInfo propertyInfo)
        {
            var settings = CreateSettingBuilder(propertyInfo)
                .SetReferenceInfo(new ReferenceInfo
                {
                    PropertyInfo = propertyInfo,
                    Source = parentInstance
                })
                .Build();

            var componentType = settings.Get<Type>("componentType");
            var component = ObjectUtils.IsGenericList(propertyInfo.PropertyType)
                ? CreateListEditor(componentType, propertyInfo.PropertyType)
                : CreateNormalEditor(componentType, propertyInfo.PropertyType);

            SettingBinder.BindEditor(component, propertyInfo, settings);

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
            listEditor.SetSettingBinder(SettingBinder);
            listEditor.SetItemFactory(() => CreateNormalEditor(componentType, propertyType));
            listEditor.SetSettingFactory((parentInstance, propertyInfo) =>
            {
                var settingBuilder = CreateSettingBuilder(propertyInfo)
                    .SetReferenceInfo(new ReferenceInfo
                    {
                        PropertyInfo = propertyInfo,
                        Source = parentInstance
                    });
                return settingBuilder.Build();
            });
            return listEditor;
        }

        private SettingBuilder CreateSettingBuilder(MemberInfo configMemeber)
        {
            return new SettingBuilder()
                .SetConfigMemberInfo(configMemeber)
                .SetContainerAppearanceFactory(ContainerAppearanceFactory)
                .SetEditorAppearanceFactory(EditorAppearanceFactory)
                .SetLayoutManagerFactory(LayoutManagerFactory)
                .SetLayoutOptionsFactory(LayoutOptionsFactory)
                .SetDateTimeOptionsFactory(DateTimeOptionsFactory)
                .SetSizeOptionsFactory(SizeOptionsFactory);
        }

        private void CheckFactories()
        {
            Precondition.PropertyNotNull(LayoutManagerFactory, nameof(LayoutManagerFactory));
            Precondition.PropertyNotNull(SizeOptionsFactory, nameof(SizeOptionsFactory));
            Precondition.PropertyNotNull(DateTimeOptionsFactory, nameof(DateTimeOptionsFactory));
            Precondition.PropertyNotNull(ContainerAppearanceFactory, nameof(ContainerAppearanceFactory));
            Precondition.PropertyNotNull(EditorAppearanceFactory, nameof(EditorAppearanceFactory));
        }

        /// <summary>
        /// Registers an <see cref="IComponentFactory{T}"/>.
        /// </summary>
        /// <param name="componentType">
        /// Type of an <see cref="IComponent"/> that will be used to map with the giving factory.
        /// </param>
        /// <param name="factory">
        /// An <see cref="IComponentFactory{T}"/> that will be used to create an <see cref="IComponent"/>
        /// corresponds to the giving component type.
        /// </param>
        public void RegisterComponentFactory(Type componentType, object factory)
        {
            _componentManager.RegisterComponentFactory(componentType, factory);
        }

        /// <summary>
        /// Registers a default component type that will be used create the UI <see cref="IComponent"/>
        /// if the config property does not supply this info.
        /// Each data type just has one default component.
        /// </summary>
        /// <param name="propertyType">The property type (aka data type) that will use this component type as default.</param>
        /// <param name="componentType">The default component type.</param>
        public void RegisterDefaultComponentType(Type propertyType, Type componentType)
        {
            _componentManager.RegisterDefaultComponentType(propertyType, componentType);
        }

        /// <summary>
        /// Copy whole settings from source to this instance.
        /// </summary>
        public void Copy(UiManager source)
        {
            Precondition.ArgumentNotNull(source, nameof(source));

            _componentManager.Copy(source._componentManager);
            _settingBinder = source._settingBinder;

            SettingBinder = source.SettingBinder;

            LayoutManagerFactory = source.LayoutManagerFactory;
            LayoutOptionsFactory = source.LayoutOptionsFactory;
            SizeOptionsFactory = source.SizeOptionsFactory;
            DateTimeOptionsFactory = source.DateTimeOptionsFactory;
            ContainerAppearanceFactory = source.ContainerAppearanceFactory;
            EditorAppearanceFactory = source.EditorAppearanceFactory;

            AllowAutoCreateInstanceIfMissing = source.AllowAutoCreateInstanceIfMissing;
        }
    }
}