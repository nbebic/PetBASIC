using System;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    public class LetNode : AstNode
    {
        private VarNode _var;
        private AstNode _ast;

        public LetNode(string vvar, AstNode ast)
        {
            _var = new VarNode(vvar);
            _ast = ast;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            _ast.CodeGen(cg);
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(vars+" + _var.Address + ")", "hl");
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
            _ast.CodeGenBasicalPre(cg);

            cg.StartCalc();
            _ast.CodeGenBasicalCalculate(cg);
            cg.EndCalc();

            cg.Emit("call", "$2da2");
            cg.Emit("ld", "(vars+" + _var.Address + ")", "hl");
        }
    }
}