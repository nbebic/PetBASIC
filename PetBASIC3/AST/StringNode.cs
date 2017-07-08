using System.Collections.Generic;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class StringNode : AstNode
    {
        public static readonly List<string> constants = new List<string>();
        public static int StrIndex = 0;

        public readonly string Value;
        public readonly int Index;

        public StringNode(string value)
        {
            Value = value;
            Index = StrIndex++;
            constants.Add(value);
        }
        public override void CodeGen(CodeGenerator cg) { }
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
            throw new System.NotImplementedException();
        }
    }
}