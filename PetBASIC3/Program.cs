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
10 let i = 16384
20 let [i]w = -1
30 let i = i + 2
40 if i <= 22527 then goto 20
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
