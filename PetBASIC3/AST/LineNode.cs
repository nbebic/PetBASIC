using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class LineNode : AstNode
    {
        private AstNode _stmt;
        private int _line;

        public LineNode(AstNode stmt, int line)
        {
            _stmt = stmt;
            _line = line;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            cg.Label("line" + _line);
            _stmt.CodeGen(cg);
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
            cg.Label("line" + _line);
            _stmt.CodeGenBasicalDo(cg);
        }
    }
}