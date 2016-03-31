using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
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
    }
}