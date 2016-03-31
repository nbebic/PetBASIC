using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class IfNode : AstNode
    {
        private AstNode _lhs;
        private AstNode _rhs;
        private string _relop;
        private AstNode _stmt;
        private static int _nexts;

        public IfNode(AstNode lhs, AstNode rhs, string relop, AstNode stmt)
        {
            _lhs = lhs;
            _rhs = rhs;
            _relop = relop;
            _stmt = stmt;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _lhs.CodeGen(cg);
            _rhs.CodeGen(cg);
            if (_relop == "<=" || _relop == ">")
            {
                cg.Emit("pop", "hl");
                cg.Emit("pop", "de");
            }
            else
            {
                cg.Emit("pop", "de");
                cg.Emit("pop", "hl");
            }

            cg.Emit("or", "a");
            cg.Emit("sbc", "hl", "de");

            var nextlbl = "next" + _nexts;
            _nexts++;

            switch (_relop)
            {
                case "<":
                case ">":
                    cg.Emit("jp", "p", nextlbl);
                    break;
                case "<=":
                case ">=":
                    cg.Emit("jp", "m", nextlbl);
                    break;
                case "=":
                    cg.Emit("jp", "nz", nextlbl);
                    break;
                case "<>":
                case "><":
                    cg.Emit("jp", "z", nextlbl);
                    break;
            }
            _stmt.CodeGen(cg);
            cg.Label(nextlbl);
        }
    }
}