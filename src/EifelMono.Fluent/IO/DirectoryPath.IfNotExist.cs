using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public partial class DirectoryPath : ValuePath, IFluentExists
    {
        protected IfNotExistsClass _IfNotExistsClass;
        public IfNotExistsClass IfNotExists { get => _IfNotExistsClass ?? (_IfNotExistsClass = new IfNotExistsClass(this)); }

        public class IfNotExistsClass : FluentParentCondition<DirectoryPath>
        {
            public IfNotExistsClass(DirectoryPath parentThis) : base(parentThis, () => ! parentThis.Exists)
            {
            }
        }
    }
}
