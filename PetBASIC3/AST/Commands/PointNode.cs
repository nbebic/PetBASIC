using System;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    public class PointNode : AstNode
    {
        private string _mode;
        private AstNode _x;
        private AstNode _y;
        public PointNode(string mode, AstNode x, AstNode y)
        {
            _mode = mode;
            _x = x;
            _y = y;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _x.CodeGen(cg);
            _y.CodeGen(cg);
            cg.Emit("pop", "bc");
            cg.Emit("pop", "de");
            cg.Emit("ld", "b", "e");
            cg.Emit("call", "plot_xy");
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
            _y.CodeGenBasicalPre(cg);
            _x.CodeGenBasicalPre(cg);
            cg.StartCalc();
            _x.CodeGenBasicalCalculate(cg);
            _y.CodeGenBasicalCalculate(cg);
            cg.EndCalc();
            cg.Emit("call", "$2da2");
            cg.Emit("ld", "b", "e");
            cg.Emit("call", "$2da2");
            cg.Emit("ld", "b", "d");
            cg.Emit("call", "plot_xy");
        }
    }
}