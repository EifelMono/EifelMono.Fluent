using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.IO
{
    public partial class FilePath 
    {
        protected IfNotExistsClass _IfNotExistsClass;
        public IfNotExistsClass IfNotExists { get => _IfNotExistsClass ?? (_IfNotExistsClass = new IfNotExistsClass(this)); }

        public class IfNotExistsClass : ParentExist<FilePath>
        {
            public IfNotExistsClass(FilePath parentThis) : base(parentThis) { }
        }
    }
}
