using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class GoSubNode : AstNode
    {
        private int _dest;

        public GoSubNode(int dest)
        {
            _dest = dest;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            cg.Emit("call", "line" + _dest);
        }
    }
}