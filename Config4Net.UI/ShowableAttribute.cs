using System;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.UI
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ShowableAttribute : Attribute
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public string Condition { get; set; }
        public Type ComponentType { get; set; }

        public ShowableAttribute(string label = null)
        {
            Label = label;
        }
    }

    public class ShowableInfo
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public string Condition { get; set; }
        public Type ComponentType { get; set; }

        public static ShowableInfo From(MemberInfo memberInfo)
        {
            var showableAttribute = memberInfo.GetCustomAttribute<ShowableAttribute>();
            if (showableAttribute == null) return null;
            return new ShowableInfo
            {
                Name = showableAttribute.Name,
                Label = string.IsNullOrEmpty(showableAttribute.Label)
                    ? StringUtils.ToFriendlyString(memberInfo.Name)
                    : showableAttribute.Label,
                Description = showableAttribute.Description,
                Required = showableAttribute.Required,
                Condition = showableAttribute.Condition,
                ComponentType = showableAttribute.ComponentType
            };
        }
    }
}