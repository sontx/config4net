using System;
using Config4Net.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Config4Net.Core.Adapter
{
    internal class JsonConfigFileAdapter : IConfigFileAdapter
    {
        private readonly JsonSerializer _serializer;

        public JsonConfigFileAdapter(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public string ToString(IConfigFile configFile)
        {
            Precondition.ArgumentNotNull(configFile, nameof(configFile));
            return JObject.FromObject(configFile, _serializer).ToString();
        }

        public IConfigFile ToConfigFile(string configFileAsString)
        {
            Precondition.ArgumentNotNull(configFileAsString, nameof(configFileAsString));
            var jsonObject = JObject.Parse(configFileAsString);
            var metadata = jsonObject.GetValue(nameof(IConfigFile.Metadata)).ToObject<Metadata>(_serializer);
            var configDataType = Type.GetType(metadata.TypeId, true);
            var configData = jsonObject
                .GetValue(nameof(IConfigFile.ConfigData))
                .ToObject(configDataType, _serializer);

            return new ConfigFile
            {
                Metadata = metadata,
                ConfigData = configData
            };
        }
    }
}