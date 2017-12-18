using Config4Net.Utils;
using System;
using System.Collections.Generic;

namespace Config4Net.UI
{
    internal sealed class ComponentManager : ICloneable
    {
        // component type - factory
        private readonly Dictionary<Type, object> _registeredComponentFactories;

        // property type - component type
        private readonly Dictionary<Type, Type> _defaultComponenTypeForPropertyTypes;

        public void RegisterComponentFactory(Type componentType, object factory)
        {
            Precondition.ArgumentNotNull(componentType, nameof(componentType));
            Precondition.ArgumentNotNull(factory, nameof(factory));
            Precondition.ArgumentCompatibleType(componentType, typeof(IComponent), nameof(componentType));

            _registeredComponentFactories.Add(componentType, factory);
        }

        public void RegisterDefaultComponentType(Type propertyType, Type componentType)
        {
            Precondition.ArgumentNotNull(componentType, nameof(componentType));
            Precondition.ArgumentNotNull(propertyType, nameof(propertyType));
            Precondition.ArgumentCompatibleType(componentType, typeof(IComponent), nameof(componentType));

            if (_defaultComponenTypeForPropertyTypes.ContainsKey(propertyType))
                _defaultComponenTypeForPropertyTypes.Remove(propertyType);
            _defaultComponenTypeForPropertyTypes.Add(propertyType, componentType);
        }

        public T CreateComponentFromComponentType<T>() where T : IComponent
        {
            return CreateComponentFromComponentType<T>(typeof(T));
        }

        public T CreateComponentFromComponentType<T>(Type componentType) where T : IComponent
        {
            Precondition.ArgumentNotNull(componentType, nameof(componentType));
            Precondition.ArgumentCompatibleType(componentType, typeof(IComponent), nameof(componentType));
            return (T)ComponentFactoryWrapper.From(_registeredComponentFactories[componentType]).Create();
        }

        public T CreateComponentFromPropertyType<T>(Type propertyType) where T : IComponent
        {
            Precondition.ArgumentNotNull(propertyType, nameof(propertyType));
            var componentType = LookupComponentType(propertyType);
            if (componentType == null)
                throw new ArgumentException($"Property type {propertyType.Name} did not register yet.");
            return CreateComponentFromComponentType<T>(componentType);
        }

        private Type LookupComponentType(Type propertyType)
        {
            if (_defaultComponenTypeForPropertyTypes.ContainsKey(propertyType))
                return _defaultComponenTypeForPropertyTypes[propertyType];

            foreach (var node in _defaultComponenTypeForPropertyTypes)
            {
                if (node.Key.IsAssignableFrom(propertyType))
                    return node.Value;
            }

            return null;
        }

        public ComponentManager()
        {
            _registeredComponentFactories = new Dictionary<Type, object>();
            _defaultComponenTypeForPropertyTypes = new Dictionary<Type, Type>();
        }

        public object Clone()
        {
            var componentManager = new ComponentManager();
            CollectionUtils.CopyDictionary(_registeredComponentFactories, componentManager._registeredComponentFactories);
            CollectionUtils.CopyDictionary(_defaultComponenTypeForPropertyTypes, componentManager._defaultComponenTypeForPropertyTypes);
            return componentManager;
        }
    }
}