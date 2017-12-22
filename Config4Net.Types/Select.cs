using System.Collections.Generic;

namespace Config4Net.Types
{
    public class Select
    {
        public IList<Option> Options { get; set; }

        public class Option
        {
            public string DisplayText { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        public class Builder<T>
        {
            private readonly Select _select;

            public Builder()
            {
                _select = new Select { Options = new List<Option>() };
            }

            public Builder<T> AddOption(string displayText, T value)
            {
                _select.Options.Add(new Option { DisplayText = displayText, Value = value });
                return this;
            }

            public Builder<T> AddOption(T value)
            {
                _select.Options.Add(new Option { DisplayText = value.ToString(), Value = value });
                return this;
            }

            public Select Build()
            {
                return _select;
            }
        }
    }
}