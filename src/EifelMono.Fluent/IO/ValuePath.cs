using System;
namespace EifelMono.Fluent.IO
{

    public class ValuePath
    {
        public ValuePath() { }

        public ValuePath(string value) : this() { Value = value; }

        public string Value { get; protected set; }

        public static implicit operator string(ValuePath path)
            => path.Value;

        public override string ToString()
            => "{Value}";
    }
}
