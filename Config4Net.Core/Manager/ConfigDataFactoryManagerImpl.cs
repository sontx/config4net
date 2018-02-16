using System;
using System.Collections.Generic;

namespace Config4Net.Core.Manager
{
    internal class ConfigDataFactoryManagerImpl : IConfigDataFactoryManager
    {
        private readonly Dictionary<Type, IConfigDataFactory> _factoryMap = new Dictionary<Type, IConfigDataFactory>();
        
        public void Register(Type configType, IConfigDataFactory factory)
        {
            lock (this)
            {
                _factoryMap.Remove(configType);
                _factoryMap.Add(configType, factory);
            }
        }

        public void Unregister(Type configType)
        {
            lock (this)
            {
                _factoryMap.Remove(configType);
            }
        }

        public IConfigDataFactory Get(Type configType, bool preventNullReference)
        {
            lock (this)
            {
                return _factoryMap.ContainsKey(configType)
                    ? _factoryMap[configType]
                    : new ConfigDataFactoryImpl(configType, new ConfigDataCheckerImpl(preventNullReference));
            }
        }
    }
}