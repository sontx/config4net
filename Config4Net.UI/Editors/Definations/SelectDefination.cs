using Config4Net.Types;

namespace Config4Net.UI.Editors.Definations
{
    public abstract class SelectDefination : IDefinationType
    {
        protected abstract Select GetSelect();

        public object GetDefination()
        {
            return GetSelect();
        }
    }
}