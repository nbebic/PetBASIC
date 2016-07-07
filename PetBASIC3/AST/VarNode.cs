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

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            cg.Emit("ld", "bc", "(vars+" + Address + ")");
            cg.Emit("call", "$2d2b");
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            throw new System.NotImplementedException();
        }
    }
}