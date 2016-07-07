using System.Collections.Generic;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    public class InputNode : AstNode
    {
        private List<string> _vars;
        public InputNode(List<string> vars)
        {
            _vars = vars;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            foreach (var v in _vars)
            {
                cg.Emit("call", "input");
                cg.Emit("ld", "(vars+" + new VarNode(v).Address + ")", "bc");
            }
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
            foreach (var v in _vars)
            {
                cg.Emit("call", "input");
                cg.Emit("ld", "(vars+" + new VarNode(v).Address + ")", "bc");
            }
        }
    }
}