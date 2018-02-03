using System;
using Config4Net.Core.Adapter;

namespace Config4Net.Tests.Mock
{
    internal class MockConfigFileFactory : IConfigFileFactory
    {
        private readonly string _author;

        public MockConfigFileFactory(string author)
        {
            _author = author;
        }

        public IConfigFile Create(string key, object configData)
        {
            return new ConfigFile
            {
                Author = _author,
                Metadata = new Metadata
                {
                    Key = key,
                    TypeId = configData.GetType().AssemblyQualifiedName,
                    Modified = DateTime.Now
                },
                ConfigData = configData
            };
        }
    }

    internal class ConfigFile : IConfigFile
    {
        public string Author { get; set; }
        public IMetadata Metadata { get; set; }
        public object ConfigData { get; set; }
    }
}