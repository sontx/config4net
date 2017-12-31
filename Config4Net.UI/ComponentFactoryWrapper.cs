using Config4Net.Utils;

namespace Config4Net.UI
{
    /// <summary>
    /// A helper class that helps to work with an <see cref="IComponentFactory{T}"/> object easier.
    /// </summary>
    public sealed class ComponentFactoryWrapper
    {
        private readonly object _genericFactory;

        /// <summary>
        /// Creates <see cref="ComponentFactoryWrapper"/> instance from a <see cref="IComponentFactory{T}"/>.
        /// </summary>
        public static ComponentFactoryWrapper From(object genericFactory)
        {
            return new ComponentFactoryWrapper(genericFactory);
        }

        private ComponentFactoryWrapper(object genericFactory)
        {
            _genericFactory = genericFactory;
        }

        /// <summary>
        /// Call <see cref="IComponentFactory{T}.Create"/>
        /// </summary>
        /// <returns>Result from <see cref="IComponentFactory{T}.Create"/></returns>
        public IComponent Create()
        {
            return (IComponent) ObjectUtils.ExecuteMethod(_genericFactory, "Create");
        }
    }
}