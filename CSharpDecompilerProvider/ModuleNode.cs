using System.Collections.Generic;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;

namespace CSharpDecompilerProvider
{
    public class ModuleNode : BaseNode
    {
        public ModuleNode(string name, IEntity entity, CSharpDecompiler decompiler)
            : base(name, entity, decompiler)
        {
        }

        public override bool IsContainer { get => true; }

        public IEnumerable<NamespaceNode> Namespaces { get; set; }

        public override string Decompiled => decompiler.DecompileModuleAndAssemblyAttributesToString();
    }
}
