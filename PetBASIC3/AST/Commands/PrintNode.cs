using System;
using System.Collections.Generic;
using System.Linq;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
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
                    if (node is VarNode && ((VarNode) node).Name.ToLower() == "nl")
                    {
                        cg.Emit("ld", "a", "13");
                        cg.Emit("rst", "$10");
                    }
                    else
                    {
                        node.CodeGen(cg);
                        cg.Emit("pop", "bc");
                        cg.Emit("call", "print_num");
                    }
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

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            foreach (var node in Enumerable.Reverse(_exprs))
            {
                if (node is ExprNode || node is NumNode || node is VarNode || node is FunNode)
                {
                    if (!(node is VarNode) || ((VarNode) node).Name.ToLower() != "nl")
                        node.CodeGenBasicalPre(cg);
                }
                else if (node is StringNode) {}
                else
                {
                    throw new Exception("Use print with expression or string");
                }
            }

            foreach (var node in _exprs)
            {
                if (node is ExprNode || node is NumNode || node is VarNode || node is FunNode)
                {
                    if (node is VarNode && ((VarNode)node).Name.ToLower() == "nl")
                    {
                        cg.Emit("ld", "a", "13");
                        cg.Emit("rst", "$10");
                    }
                    else
                    {
                        cg.StartCalc();
                        node.CodeGenBasicalCalculate(cg);
                        cg.EndCalc();
                        cg.Emit("call", "$2da2");
                        cg.Emit("call", "print_num");
                    }
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