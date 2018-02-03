namespace Config4Net.Core.Adapter
{
    internal class ConfigFile : IConfigFile
    {
        public IMetadata Metadata { get; set; }
        public object ConfigData { get; set; }
    }
}