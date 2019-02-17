using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EifelMono.Fluent.IO
{

    public class ValuePath
    {
        public ValuePath() { }

        public ValuePath(string value) : this() { Value = value; }

        public string Value { get; protected set; }

        public static implicit operator string(ValuePath path)
            => path.Value;

        public List<string> Segements()
            => Value.Split(Path.PathSeparator).ToList();

        public override string ToString()
            => "{Value}";
    }
}
