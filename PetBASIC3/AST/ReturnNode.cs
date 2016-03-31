using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class ReturnNode : AstNode
    {
        public override void CodeGen(CodeGenerator cg)
        {
            cg.Emit("ret");
        }
    }
}