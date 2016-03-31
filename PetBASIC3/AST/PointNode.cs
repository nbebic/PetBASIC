using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
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
    }
}