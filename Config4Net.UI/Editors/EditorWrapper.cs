using System;
using System.Reflection;
using Config4Net.Utils;

namespace Config4Net.UI.Editors
{
    public sealed class EditorWrapper
    {
        private readonly object _genericEditor;
        private bool _readOnly;
        private EditorAppearance _appearance;
        private Type _definationType;
        private object _value;

        public static EditorWrapper From(object genericEditor)
        {
            return new EditorWrapper(genericEditor);
        }

        private EditorWrapper(object genericEditor)
        {
            Precondition.ArgumentNotNull(genericEditor, nameof(genericEditor));
            _genericEditor = genericEditor;
        }

        public Type DefinationType
        {
            get => _definationType;
            set
            {
                _definationType = value; 
                ObjectUtils.SetProperty(_genericEditor, "DefinationType", value);
            }
        }

        public object Value
        {
            get => _value;
            set
            {
                _value = value; 
                ObjectUtils.SetProperty(_genericEditor, "Value", value);
            }
        }

        public EditorAppearance Appearance
        {
            get => _appearance;
            set
            {
                _appearance = value; 
                ObjectUtils.SetProperty(_genericEditor, "Appearance", value);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value; 
                ObjectUtils.SetProperty(_genericEditor, "ReadOnly", value);
            }
        }

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            ObjectUtils.ExecuteMethod(_genericEditor, "SetReferenceInfo", source, propertyInfo);
        }
    }
}