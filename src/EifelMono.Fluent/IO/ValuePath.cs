using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace EifelMono.Fluent.IO
{

    [DataContract]
    public class ValuePath
    {
        public ValuePath() { }

        public ValuePath(string value) : this() { Value = value; }

        [DataMember]
        public string Value { get; protected set; }

        public static implicit operator string(ValuePath path)
            => path.Value;

        public List<string> Segements()
            => Value.Split(Path.PathSeparator).ToList();

        public override string ToString()
            => $"{Value}";
    }
}
