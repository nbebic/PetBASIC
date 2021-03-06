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
                    cg.Emit("ld", "a", "h");
                    cg.Emit("ld", "c", "l");
                    cg.Emit("call", "div_ac_de");
                    cg.Emit("ld", "h", "a");
                    cg.Emit("ld", "l", "c");
                    break;
            }
            cg.Emit("push", "hl");
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
            _lhs.CodeGenBasicalCalculate(cg);
            _rhs.CodeGenBasicalCalculate(cg);
            switch (_op)
            {
                case '+':
                    cg.EmitByte(0x0f);
                    break;
                case '-':
                    cg.EmitByte(0x01);
                    cg.EmitByte(0x03);
                    break;
                case '*':
                    cg.EmitByte(0x04);
                    break;
                case '/':
                    cg.EmitByte(0x05);
                    cg.EmitByte(0x27);
                    break;
            }
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            throw new System.NotImplementedException();
        }

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            _lhs.CodeGenBasicalPre(cg);
            _rhs.CodeGenBasicalPre(cg);
        }
    }
}