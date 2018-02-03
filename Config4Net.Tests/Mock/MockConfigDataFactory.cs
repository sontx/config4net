using Config4Net.Core.Manager;

namespace Config4Net.Tests.Mock
{
    internal class MockConfigDataFactory : IConfigDataFactory
    {
        private readonly object _configData;

        public MockConfigDataFactory(object configData)
        {
            _configData = configData;
        }

        public object Create()
        {
            return _configData;
        }
    }
}