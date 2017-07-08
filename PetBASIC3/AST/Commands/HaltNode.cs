using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    public class HaltNode : AstNode
    {
        public override void CodeGen(CodeGenerator cg)
        {
            cg.Emit("halt");
        }

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            throw new System.NotImplementedException();
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
            throw new System.NotImplementedException();
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            cg.Emit("halt");
        }
    }
}