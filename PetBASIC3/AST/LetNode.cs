using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class LetNode : AstNode
    {
        private VarNode _var;
        private AstNode _ast;

        public LetNode(string vvar, AstNode ast)
        {
            _var = new VarNode(vvar);
            _ast = ast;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _ast.CodeGen(cg);
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(vars+" + _var.Address + ")", "hl");
        }
    }
}