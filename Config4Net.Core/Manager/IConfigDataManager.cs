namespace Config4Net.Core.Manager
{
    internal interface IConfigDataManager
    {
        ConfigDataManagerSettings Settings { get; set; }

        void Add(string configKey, object configData);

        bool Has(string configKey);

        object Get(string configKey);

        void Remove(string configKey);

        void Save();

        void Load();
    }
}