using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;

namespace CSharpDecompilerProvider
{
    public class MemberNode : BaseNode
    {
        public MemberNode(string name, IEntity entity, CSharpDecompiler decompiler)
            : base(name, entity, decompiler)
        {

        }
        public override bool IsContainer { get => false; }

        public override string Decompile
        {
            get
            {
                var cecilMember = decompiler.TypeSystem.GetCecil(entity as IMember).Resolve();
                return decompiler.DecompileAsString(cecilMember);
            }
        }
    }
}