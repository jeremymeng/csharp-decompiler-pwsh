using System.Collections.Generic;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;

namespace CSharpDecompilerProvider
{
    public class NamespaceNode : BaseNode
    {
        public NamespaceNode(string name, IEntity entity, CSharpDecompiler decompiler)
            : base(name, entity, decompiler)
        {
        }

        public override bool IsContainer { get => true; }

        public IEnumerable<TypeNode> Types { get; set; }

        public override string Decompile => $"namespace {Name} {{ }}";
    }
}
