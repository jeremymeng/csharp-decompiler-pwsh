using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;

namespace CSharpDecompilerProvider
{
    public abstract class BaseNode
    {
        protected readonly IEntity entity;
        protected readonly CSharpDecompiler decompiler;

        public BaseNode(string name, IEntity entity, CSharpDecompiler decompiler)
        {
            Name = name;
            this.entity = entity;
            this.decompiler = decompiler;
        }

        public string Name { get; set; }

        public virtual bool IsContainer { get => false; }

        public abstract string Decompiled { get; }
    }
}
