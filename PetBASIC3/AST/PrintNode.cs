using System;
using System.Collections.Generic;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class PrintNode : AstNode
    {
        private List<AstNode> _exprs;
        public PrintNode(List<AstNode> exprs)
        {
            _exprs = exprs;
        }
        
        public override void CodeGen(CodeGenerator cg)
        {
            foreach (var node in _exprs)
            {
                if (node is ExprNode || node is NumNode || node is VarNode || node is FunNode)
                {
                    node.CodeGen(cg);
                    cg.Emit("pop", "bc");
                    cg.Emit("call", "print_num");
                }
                else if (node is StringNode)
                {
                    cg.Emit("ld", "hl", "str" + node.As<StringNode>().Index);
                    cg.Emit("call", "print_str");
                }
                else
                {
                    throw new Exception("Use print with expression or string");
                }
            }
        }
    }
}