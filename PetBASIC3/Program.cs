using System;
using System.Collections.Generic;
using System.IO;
using PetBASIC3.AST;
using PetBASIC3.CodeGen;

namespace PetBASIC3
{
    class Program
    {
        public const string s =
@"
10 for i = 16384 to 22527
20 poke i, 255
30 next i
";
        static void Main(string[] args)
        {
            var tokens = Lexer.Lex(new StringInput(s));
            var p = new Parser(tokens);
            List<AstNode> n;
            p.ParseProgram(out n);
            CodeGenerator cg = new CodeGenerator();
            foreach (var node in n)
            {
                node.CodeGen(cg);
            }
            cg.End();
            Console.Write(cg);
            File.WriteAllText(@"C:\Users\Nikola\Desktop\output.asm", cg.ToString());
            Console.ReadLine();
        }
    }
}
