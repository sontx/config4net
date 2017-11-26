using System;
using System.Reflection;
using Config4Net.UI.Editors;
using Config4Net.Utils;

namespace Config4Net.UI
{
    public sealed class EditorHelper<T>
    {
        private PropertyBinder<T> _binder;
        private readonly IEditor<T> _editor;

        public EditorHelper(IEditor<T> editor)
        {
            this._editor = editor;
        }

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            _binder = new PropertyBinder<T>(source, propertyInfo);
        }

        public void Bind()
        {
            _binder.Value = _editor.Value;
        }

        public void Reset()
        {
            _binder.Reset();
        }

        public void ChangeValue(
            T newValue, 
            Action doChaningValue, 
            ValueChangingEventHandler onValueChanging,
            ValueChangedEventHandler onValueChanged)
        {
            Precondition.ArgumentNotNull(doChaningValue, nameof(doChaningValue));

            var oldValue = _editor.Value;

            if (onValueChanging != null)
            {
                var args = new ValueChangingEventArgs(oldValue, newValue);
                onValueChanging.Invoke(_editor, args);
                if (args.Cancel) return;
            }

            doChaningValue();

            onValueChanged?.Invoke(_editor, new ValueChangedEventArgs(oldValue, newValue));
        }
    }
}