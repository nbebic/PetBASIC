using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
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
            var v = PartOfMagic(cg);
            _stmt.CodeGen(cg);
            cg.Label(v);
        }

        private string PartOfMagic(CodeGenerator cg)
        {
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
            return nextlbl;
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
            _rhs.CodeGenBasicalPre(cg);
            _lhs.CodeGenBasicalPre(cg);
            cg.StartCalc();
            _lhs.CodeGenBasicalCalculate(cg);
            _rhs.CodeGenBasicalCalculate(cg);
            cg.EndCalc();
            cg.Emit("call", "$2da2");
            cg.Emit("push", "bc");
            cg.Emit("call", "$2da2");
            cg.Emit("push", "bc");
            var v = PartOfMagic(cg);
            _stmt.CodeGenBasicalDo(cg);
            cg.Label(v);
        }
    }
}