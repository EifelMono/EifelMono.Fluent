using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.IO
{
    public partial class FilePath
    {
        protected IfExistsClass _IfExistsClass;
        public IfExistsClass IfExists { get => _IfExistsClass ?? (_IfExistsClass = new IfExistsClass(this)); }

        public class IfExistsClass : ParentExist<FilePath>
        {
            public IfExistsClass(FilePath parentThis) : base(parentThis) { }

            public FilePath Delete()
            {
                if (ParentThis.Exists)
                    ParentThis.Delete();
                return ParentThis;
            }

            public FilePath ClearAttributes()
            {
                if (ParentThis.Exists)
                    return ParentThis.ClearAttributes();
                return ParentThis;
            }

            public FilePath SetAttributes(FileAttributes fileAttributes)
            {
                if (ParentThis.Exists)
                    return ParentThis.SetAttributes(fileAttributes);
                return ParentThis;
            }
            public FilePath RemoveAttributes(FileAttributes fileAttributes)
            {
                if (ParentThis.Exists)
                    return ParentThis.RemoveAttributes(fileAttributes);
                return ParentThis;
            }

            public FilePath RemoveAttributes()
            {
                if (ParentThis.Exists)
                    fluent.Enum.Values<FileAttributes>().ForEach((a) => ParentThis.RemoveAttributes(a));
                return ParentThis;
            }

            public FilePath ChangeAttributes(FileAttributes fileAttributes, bool on)
            {
                if (ParentThis.Exists)
                    return ParentThis.ChangeAttributes(fileAttributes, on);
                return ParentThis;
            }
        }
    }
}
