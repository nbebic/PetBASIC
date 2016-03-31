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
    }
}