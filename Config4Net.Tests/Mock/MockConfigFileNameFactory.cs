using Config4Net.Core.Adapter;

namespace Config4Net.Tests.Mock
{
    internal class MockConfigFileNameFactory : IConfigFileNameFactory
    {
        private readonly string _configFileName;

        public MockConfigFileNameFactory(string configFileName)
        {
            _configFileName = configFileName;
        }

        public string Create(IConfigFile configFile, string extension)
        {
            return _configFileName;
        }
    }
}