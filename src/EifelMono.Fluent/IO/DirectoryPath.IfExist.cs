using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Interfaces;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public partial class DirectoryPath : ValuePath, IFluentExists
    {
        protected IfExistsClass _IfExistsClass;
        public IfExistsClass IfExists { get => _IfExistsClass ?? (_IfExistsClass = new IfExistsClass(this)); }

        public class IfExistsClass : FluentParentCondition<DirectoryPath>
        {
            public IfExistsClass(DirectoryPath parentThis) : base(parentThis, () => parentThis.Exists)
            {
            }
        }
    }
}
