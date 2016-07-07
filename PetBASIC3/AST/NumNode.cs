using System;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class NumNode : AstNode
    {
        public readonly int Num;
        public NumNode(int num)
        {
            Num = num;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            cg.Emit("ld", "hl", Num.ToString());
            cg.Emit("push", "hl");
        }

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            cg.Emit("ld", "bc", Num.ToString());
            cg.Emit("call", "$2d2b");
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg) {}

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            throw new InvalidOperationException();
        }
    }
}