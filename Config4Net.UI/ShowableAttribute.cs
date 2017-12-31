using Config4Net.Utils;
using System;
using System.Reflection;

namespace Config4Net.UI
{
    /// <summary>
    /// Annotates a property is a showable element in the UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ShowableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the component label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the component description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the component is mandatory.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the component condition that helps to validate the value
        /// before it changes.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the component type that presents this property in UI.
        /// </summary>
        public Type ComponentType { get; set; }

        /// <summary>
        /// Create <see cref="ShowableAttribute"/>.
        /// </summary>
        public ShowableAttribute(string label = null)
        {
            Label = label;
        }
    }

    /// <summary>
    /// Contains extracted info from <see cref="ShowableAttribute"/>.
    /// </summary>
    public class ShowableInfo
    {
        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the component label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the component description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the component is mandatory.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the component condition that helps to validate the value
        /// before it changes.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the component type that presents this property in UI.
        /// </summary>
        public Type ComponentType { get; set; }

        /// <summary>
        /// Creates <see cref="ShowableInfo"/> from a giving property.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
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