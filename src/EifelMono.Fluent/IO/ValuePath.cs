using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string Value { get; internal set; }

        public override string ToString()
            => $"{Value}";

        public static implicit operator string(ValuePath path)
            => path.Value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string FullPath
            => Path.GetFullPath(Value);

        public string NormalizedValue
            { get => Value.NormalizePath(); }

        public IEnumerable<string> SpiltValue
            { get => Value.NormalizePath().SplitPath(); }
    }
}
