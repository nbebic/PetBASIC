using System.Collections.Generic;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
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
    }
}