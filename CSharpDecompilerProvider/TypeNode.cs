using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using System.Collections.Generic;

namespace CSharpDecompilerProvider
{
    public class TypeNode : BaseNode
    {
        public TypeNode(string name, IEntity entity, CSharpDecompiler decompiler)
            : base(name, entity, decompiler)
        {

        }

        public override bool IsContainer { get => true; }

        public IEnumerable<TypeNode> NestedTypes { get; set; }
        public IEnumerable<MemberNode> Members { get; set; }

        public override string Decompiled
        {
            get
            {
                var cecilType = decompiler.TypeSystem.GetCecil(entity as ITypeDefinition);
                return decompiler.DecompileTypesAsString(new[] { cecilType });
            }
        }
    }
}