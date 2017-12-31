using Config4Net.UI.Containers;
using Config4Net.Utils;

namespace Config4Net.UI
{
    /// <summary>
    /// An utility that helps to lookup a child <see cref="IComponent"/> from a giving <see cref="IContainer"/>.
    /// </summary>
    public static class LookupUtils
    {
        /// <summary>
        /// Lookups child component from property name.
        /// </summary>
        /// <param name="container">An <see cref="IContainer"/> that contains the lookup component.</param>
        /// <param name="propertyName">Property name that equals to component name. Lookups by this name.</param>
        /// <typeparam name="T">Type of the result component.</typeparam>
        /// <returns></returns>
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