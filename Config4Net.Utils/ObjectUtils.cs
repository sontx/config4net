using Newtonsoft.Json;

namespace Config4Net.Utils
{
    public static class ObjectUtils
    {
        public static void SetProperty(object source, string name, object value)
        {
            source.GetType().GetProperty(name)?.SetValue(source, value);
        }

        public static bool DeepEquals(object obj1, object obj2, bool ignoreCase = false)
        {
            var st1 = JsonConvert.SerializeObject(obj1);
            var st2 = JsonConvert.SerializeObject(obj2);
            return string.Compare(st1, st2, ignoreCase) == 0;
        }

        public static string ToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(string st)
        {
            return JsonConvert.DeserializeObject<T>(st);
        }
    }
}