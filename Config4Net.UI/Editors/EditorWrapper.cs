using Config4Net.Utils;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// Helper class that helps to work with <see cref="IEditor{T}"/>.
    /// </summary>
    public sealed class EditorWrapper
    {
        private readonly object _genericEditor;
        private bool _readOnly;
        private EditorAppearance _appearance;
        private object _value;

        /// <summary>
        /// Creates an <see cref="EditorWrapper"/> from an <see cref="IEditorFactory{T}"/>.
        /// </summary>
        public static EditorWrapper From(object genericEditor)
        {
            return new EditorWrapper(genericEditor);
        }

        private EditorWrapper(object genericEditor)
        {
            Precondition.ArgumentNotNull(genericEditor, nameof(genericEditor));
            _genericEditor = genericEditor;
        }

        /// <summary>
        /// <see cref="IEditor{T}.Value"/>.
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                ObjectUtils.SetProperty(_genericEditor, nameof(Value), value);
            }
        }

        /// <summary>
        /// <see cref="IEditor{T}.Appearance"/>.
        /// </summary>
        public EditorAppearance Appearance
        {
            get => _appearance;
            set
            {
                _appearance = value;
                ObjectUtils.SetProperty(_genericEditor, nameof(Appearance), value);
            }
        }

        /// <summary>
        /// <see cref="IEditor{T}.ReadOnly"/>.
        /// </summary>
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                ObjectUtils.SetProperty(_genericEditor, nameof(ReadOnly), value);
            }
        }

        /// <summary>
        /// See <see cref="IEditor{T}.SetReferenceInfo"/>
        /// </summary>
        public void SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            ObjectUtils.ExecuteMethod(_genericEditor, nameof(SetReferenceInfo), referenceInfo);
        }
    }
}