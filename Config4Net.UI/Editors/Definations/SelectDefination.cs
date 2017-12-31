using Config4Net.Types;

namespace Config4Net.UI.Editors.Definations
{
    /// <summary>
    /// The defination of <see cref="ISelectEditor"/>.
    /// </summary>
    public abstract class SelectDefination : IDefinationType
    {
        /// <summary>
        /// Get <see cref="Select"/> instance.
        /// </summary>
        /// <returns></returns>
        protected abstract Select GetSelect();

        /// <inheritdoc />
        public object GetDefination()
        {
            return GetSelect();
        }
    }
}