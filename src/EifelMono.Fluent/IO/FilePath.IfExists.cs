using System.IO;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public partial class FilePath
    {
        protected IfExistsClass _IfExistsClass;
        public IfExistsClass IfExists { get => _IfExistsClass ?? (_IfExistsClass = new IfExistsClass(this)); }

        public class IfExistsClass : FluentParentCondition<FilePath>
        {
            public IfExistsClass(FilePath parentThis) : base(parentThis, ()=> parentThis.Exists)
            {
            }

            public FilePath Delete()
            {
                if (Condition)
                    ParentThis.Delete();
                return ParentThis;
            }

            public FilePath ClearAttributes()
            {
                if (Condition)
                    return ParentThis.ClearAttributes();
                return ParentThis;
            }

            public FilePath SetAttributes(FileAttributes fileAttributes)
            {
                if (Condition)
                    return ParentThis.SetAttributes(fileAttributes);
                return ParentThis;
            }
            public FilePath RemoveAttributes(FileAttributes fileAttributes)
            {
                if (Condition)
                    return ParentThis.RemoveAttributes(fileAttributes);
                return ParentThis;
            }

            public FilePath RemoveAttributes()
            {
                if (Condition)
                    fluent.Enum.Values<FileAttributes>().ForEach((a) => ParentThis.RemoveAttributes(a));
                return ParentThis;
            }

            public FilePath ChangeAttributes(FileAttributes fileAttributes, bool on)
            {
                if (Condition)
                    return ParentThis.ChangeAttributes(fileAttributes, on);
                return ParentThis;
            }
        }
    }
}
