using System.Collections.Generic;
using Config4Net.Utils;

namespace Config4Net.UI
{
    /// <summary>
    /// Settings for <see cref="IComponent"/>.
    /// </summary>
    public class Settings
    {
        private readonly Dictionary<string, object> _map = new Dictionary<string, object>();

        /// <summary>
        /// Get setting value. If the name does not exist, defaultValue will be returned.
        /// </summary>
        public T Get<T>(string name, T defaultValue)
        {
            return (T)(_map.ContainsKey(name) ? _map[name] : defaultValue);
        }

        /// <summary>
        /// Get setting value. If the name does not exist, default(T) will be returned.
        /// </summary>
        public T Get<T>(string name)
        {
            return Get(name, default(T));
        }

        /// <summary>
        /// Get setting value. If the name does not exist, default(T) will be returned.
        /// </summary>
        public T Get<T>()
        {
            var type = typeof(T);
            var name = type.Name;
            if (type.IsInterface && name.StartsWith("I"))
                name = name.Substring(1);
            name = StringUtils.ToVariableName(name);
            return Get<T>(name);
        }

        /// <summary>
        /// Store a setting value to this setting bundle. If the setting already exists, it will be replaced.
        /// </summary>
        public void Put(string name, object value)
        {
            if (_map.ContainsKey(name))
                _map.Remove(name);
            _map.Add(name, value);
        }
    }
}