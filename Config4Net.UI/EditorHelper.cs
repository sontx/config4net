using System;
using System.Reflection;
using Config4Net.UI.Editors;
using Config4Net.Utils;

namespace Config4Net.UI
{
    /// <summary>
    /// Helps to synchnorize between the editor and underlying data.
    /// </summary>
    /// <typeparam name="T">Type of value that depends on the editor.</typeparam>
    public sealed class EditorHelper<T>
    {
        private PropertyBinder<T> _binder;
        private readonly IEditor<T> _editor;

        /// <summary>
        /// Creates new editor helper.
        /// </summary>
        /// <param name="editor">The editor that will be synchronized.</param>
        public EditorHelper(IEditor<T> editor)
        {
            this._editor = editor;
        }

        /// <summary>
        /// Sets underlying data that will be synchronized with the editor.
        /// </summary>
        /// <param name="source">The owner of the giving property info.</param>
        /// <param name="propertyInfo">The property info that will be synchronized with the editor.</param>
        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            _binder = new PropertyBinder<T>(source, propertyInfo);
            _editor.Value = _binder.Value;
        }

        /// <summary>
        /// Sets the editor value from the UI to underlying data.
        /// </summary>
        public void Bind()
        {
            _binder.Value = _editor.Value;
        }

        /// <summary>
        /// Resets the underlying data to origin.
        /// </summary>
        public void Reset()
        {
            _binder.Reset();
        }

        /// <summary>
        /// Changes the UI value.
        /// </summary>
        /// <param name="newValue">New value.</param>
        /// <param name="doChaningValue">Do changing the UI value.</param>
        /// <param name="onValueChanging">Trigger value changing event.</param>
        /// <param name="onValueChanged">Trigger value changed event.</param>
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