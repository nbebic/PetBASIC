using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class VarNode : AstNode
    {
        public readonly string Name;
        public int Address => (Name.ToUpper()[0] - 'A')*2;
        public VarNode(string name)
        {
            Name = name;
        }
        public override void CodeGen(CodeGenerator cg)
        {
            cg.Emit("ld", "bc", "(vars+" + Address + ")");
            cg.Emit("push", "bc");
        }
    }
}