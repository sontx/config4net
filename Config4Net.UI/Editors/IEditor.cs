using Config4Net.UI.Editors.Definations;
using System;

namespace Config4Net.UI.Editors
{
    /// <summary>
    /// An editor that interacts with a config property. An editor can be a textbox,
    /// a time picker or a combobox...
    /// </summary>
    /// <typeparam name="T">The data type that is manages by this editor.</typeparam>
    public interface IEditor<T> : IComponent
    {
        /// <summary>
        /// Trigger when <see cref="Value"/> changed.
        /// <seealso cref="ValueChangedEventArgs"/>
        /// </summary>
        event ValueChangedEventHandler ValueChanged;

        /// <summary>
        /// Trigger when <see cref="Value"/> is changing. The changing can be canceled by
        /// set <see cref="ValueChangingEventArgs.Cancel"/> property to true.
        /// <seealso cref="ValueChangingEventArgs"/>
        /// </summary>
        event ValueChangingEventHandler ValueChanging;

        /// <summary>
        /// The value of the editor that is synchronized with config property.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Settings for this editor.
        /// </summary>
        EditorAppearance Appearance { get; set; }

        /// <summary>
        /// The editor is in read-only mode or not.
        /// </summary>
        bool ReadOnly { get; set; }

        /// <summary>
        /// Sets the config property that will be bound to this editor.
        /// </summary>
        void SetReferenceInfo(ReferenceInfo referenceInfo);

        /// <summary>
        /// Reset the editor's <see cref="Value"/> to origin value that is retrived from config property.
        /// </summary>
        void Reset();
    }

    /// <summary>
    /// The editor's value changed event handler.
    /// <seealso cref="ValueChangedEventArgs"/>
    /// </summary>
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    /// <summary>
    /// The editor's value changed event args.
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// The new value.
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// Create new <see cref="ValueChangedEventArgs"/> instance.
        /// </summary>
        /// <param name="oldValue"><see cref="OldValue"/></param>
        /// <param name="newValue"><see cref="NewValue"/></param>
        public ValueChangedEventArgs(object oldValue, object newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    /// <summary>
    /// The editor's value changing event handler.
    /// <seealso cref="ValueChangingEventArgs"/>
    /// </summary>
    public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);

    /// <summary>
    /// The editor's value changing event args.
    /// </summary>
    public class ValueChangingEventArgs : EventArgs
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// The new value.
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// Set this flag to true to cancel the changing process.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Create new <see cref="ValueChangingEventArgs"/> instance.
        /// </summary>
        /// <param name="oldValue"><see cref="OldValue"/></param>
        /// <param name="newValue"><see cref="NewValue"/></param>
        public ValueChangingEventArgs(object oldValue, object newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}