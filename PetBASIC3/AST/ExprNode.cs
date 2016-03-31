using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class ExprNode : AstNode
    {
        private AstNode _lhs;
        private AstNode _rhs;
        private char _op;
        
        public ExprNode(AstNode lhs, AstNode rhs, char op)
        {
            _lhs = lhs;
            _rhs = rhs;
            _op = op;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _lhs.CodeGen(cg);
            _rhs.CodeGen(cg);
            cg.Emit("pop", "de");
            cg.Emit("pop", "hl");
            switch (_op)
            {
                case '+':
                    cg.Emit("add", "hl", "de");
                    break;
                case '-':
                    cg.Emit("or", "a");
                    cg.Emit("sbc", "hl", "de");
                    break;
                case '*':
                    cg.Emit("call", "mult_hl_de");
                    break;
                case '/':
                    cg.Emit("call", "div_bc_de");
                    cg.Emit("ld", "h", "a");
                    cg.Emit("ld", "l", "c");
                    break;
            }
            cg.Emit("push", "hl");
        }
    }
}