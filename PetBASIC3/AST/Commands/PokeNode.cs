using System;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    public class PokeNode : AstNode
    {
        private readonly AstNode _dest;
        private readonly AstNode _expr;
        private readonly bool _word;
        public PokeNode(AstNode dest, AstNode expr, bool word)
        {
            _dest = dest;
            _expr = expr;
            _word = word;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _dest.CodeGen(cg);
            _expr.CodeGen(cg);
            cg.Emit("pop", "de");
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(hl)", "e");
            if (_word)
            {
                cg.Emit("inc", "hl");
                cg.Emit("ld", "(hl)", "d");
            }
        }

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            throw new InvalidOperationException();
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
            throw new InvalidOperationException();
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            _expr.CodeGenBasicalPre(cg);
            _dest.CodeGenBasicalPre(cg);
            cg.StartCalc();
            _dest.CodeGenBasicalCalculate(cg);
            _expr.CodeGenBasicalCalculate(cg);
            cg.EndCalc();
            cg.Emit("call", "$2da2");
            cg.Emit("push", "bc");
            cg.Emit("call", "$2da2");
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(hl)", "c");
            if (_word)
            {
                cg.Emit("inc", "hl");
                cg.Emit("ld", "(hl)", "b");
            }
        }
    }
}