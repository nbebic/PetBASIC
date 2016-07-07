using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    public class GoToNode : AstNode
    {
        private int _dest;
        public GoToNode(int dest)
        {
            _dest = dest;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            cg.Emit("jp", "line" + _dest);
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
            cg.Emit("jp", "line" + _dest);
        }
    }
}