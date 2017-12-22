using Config4Net.UI.Containers;
using Config4Net.Utils;

namespace Config4Net.UI
{
    public static class LookupUtils
    {
        public static T LookupByName<T>(this IContainer container, string propertyName) where T : IComponent
        {
            Precondition.ArgumentNotNull(container, nameof(container));
            Precondition.ArgumentNotNull(propertyName, nameof(propertyName));

            foreach (var child in container.Children)
            {
                if (child.Name == propertyName) return (T)child;

                if (child is IContainer childContainer)
                {
                    return LookupByName<T>(childContainer, propertyName);
                }
            }

            return default(T);
        }
    }
}