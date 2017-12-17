using Config4Net.Utils;

namespace Config4Net.UI
{
    public sealed class ComponentFactoryWrapper
    {
        private readonly object _genericFactory;

        public static ComponentFactoryWrapper From(object genericFactory)
        {
            return new ComponentFactoryWrapper(genericFactory);
        }

        private ComponentFactoryWrapper(object genericFactory)
        {
            _genericFactory = genericFactory;
        }

        public IComponent Create()
        {
            return (IComponent) ObjectUtils.ExecuteMethod(_genericFactory, "Create");
        }
    }
}