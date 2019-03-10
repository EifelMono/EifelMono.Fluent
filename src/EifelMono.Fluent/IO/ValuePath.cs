using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    [DataContract]
    public class ValuePath
    {
        public ValuePath() { }

        public ValuePath(string value) : this() { Value = value; }

        private string _value;
        [DataMember]
        public string Value
        {
            get => _value; internal set
            {
                if (value != _value)
                {
                    _value = value;
                    SplitValues = NormalizeValue.SplitPath().ToList();
                    IsValueOk = CheckValue();
                }
            }
        }

        public bool IsValueOk { get; set; } = true;

        protected virtual bool CheckValue()
            => true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<string> SplitValues { get; private set; } = new List<string>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string SplitValuesLast { get => SplitValues.Last(); }

        public override string ToString()
            => $"{Value}";

        public static implicit operator string(ValuePath path)
            => path.FullPath;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string FullPath
            => Path.GetFullPath(Value);

        public static char PathSeparatorChar => Path.DirectorySeparatorChar;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string NormalizeValue { get => Value.NormalizePath(); }
    }
}
