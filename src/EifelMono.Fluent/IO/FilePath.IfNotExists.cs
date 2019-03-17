using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.IO
{
    public partial class FilePath
    {
        protected IfNotExistsClass _IfNotExistsClass;
        public IfNotExistsClass IfNotExists { get => _IfNotExistsClass ?? (_IfNotExistsClass = new IfNotExistsClass(this)); }

        public class IfNotExistsClass : FluentParentCondition<FilePath>
        {
            public IfNotExistsClass(FilePath parentThis) : base(parentThis, () => ! parentThis.Exists)
            {
            }
        }
    }
}
