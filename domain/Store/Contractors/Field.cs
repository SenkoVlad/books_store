using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Contractors
{
    public abstract class Field
    {
        public string Label { get; }
        public string Name { get; }
        public string Value { get; }

        public Field(string label, string name, string value)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public class SelectedField : Field
    {
        public IReadOnlyDictionary<string, string> Items { get; }
        public SelectedField(string label, string name, string value, IReadOnlyDictionary<string, string> items)
            : base(label, name, value)
        {
            Items = items;
        }
    }


}
