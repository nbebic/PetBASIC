using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class PokeNode : AstNode
    {
        private readonly AstNode _dest;
        private readonly AstNode _expr;
        public PokeNode(AstNode dest, AstNode expr)
        {
            _dest = dest;
            _expr = expr;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _dest.CodeGen(cg);
            _expr.CodeGen(cg);
            cg.Emit("pop", "de");
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(hl)", "e");
        }
    }
}