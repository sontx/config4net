using System;

namespace Config4Net.UI.Editors
{
    public interface IEditor<T>: IComponent
    {
        event ValueChangedEventHandler ValueChanged;

        event ValueChangingEventHandler ValueChanging;

        T Value { get; set; }

        bool ReadOnly { get; set; }

        void SetReferenceInfo(object source, string propertyPath);

        void Bind();
    }

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public class ValueChangedEventArgs : EventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }

        public ValueChangedEventArgs(object oldValue, object newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    public delegate void ValueChangingEventHandler(object sender, ValueChangedEventArgs e);

    public class ValueChangingEventArgs : EventArgs
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public bool Passed { get; set; }

        public ValueChangingEventArgs(object oldValue, object newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.Passed = true;
        }
    }
}