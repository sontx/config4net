using System;
using System.Drawing;
using Config4Net.UI.Editors;

namespace Config4Net.UI
{
    public abstract class FlatformLoader : IFlatformLoader
    {
        protected UiManager UiManager => UiManager.Default;

        public void Load()
        {
            Load(UiManager.Default);
        }

        public virtual void Load(UiManager uiManager)
        {
            LoadDefaultComponentTypes(uiManager);
        }

        private void LoadDefaultComponentTypes(UiManager uiManager)
        {
            uiManager.RegisterDefaultComponentType(typeof(string), typeof(ITextEditor));
            uiManager.RegisterDefaultComponentType(typeof(int), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(long), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(float), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(double), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(decimal), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(short), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(byte), typeof(INumberEditor));
            uiManager.RegisterDefaultComponentType(typeof(Color), typeof(IColorEditor));
            uiManager.RegisterDefaultComponentType(typeof(DateTime), typeof(IDateTimeEditor));
            uiManager.RegisterDefaultComponentType(typeof(Enum), typeof(IEnumEditor));
            uiManager.RegisterDefaultComponentType(typeof(bool), typeof(ICheckboxEditor));
        }
    }
}