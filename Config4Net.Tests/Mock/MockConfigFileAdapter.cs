using Config4Net.Core.Adapter;
using System;

namespace Config4Net.Tests.Mock
{
    internal class MockConfigFileAdapter : IConfigFileAdapter
    {
        private readonly string _configKey;
        private readonly string _fileContent;
        private readonly object _configData;

        public MockConfigFileAdapter(string configKey, string fileContent, object configData)
        {
            _configKey = configKey;
            _fileContent = fileContent;
            _configData = configData;
        }

        public string ToString(IConfigFile configFile)
        {
            return _fileContent;
        }

        public IConfigFile ToConfigFile(string configFileAsString)
        {
            if (configFileAsString == _fileContent)
            {
                return new ConfigFile
                {
                    Metadata = new Metadata
                    {
                        Key = _configKey,
                        TypeId = "someId",
                        Modified = DateTime.Now
                    },
                    ConfigData = _configData
                };
            }
            return null;
        }
    }
}