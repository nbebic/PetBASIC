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
5 let r = 0
10 for i = -100 to 100
20 for j = -100 to 100
30 if i*i + j*j <= 10000 then let r = r + 1
40 next j
50 next i
60 print r
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
                //node.CodeGen(cg);
                node.CodeGenBasicalDo(cg);
            }
            cg.End();
            Console.Write(cg);
            File.WriteAllText(@"C:\Users\Nikola\Desktop\output.asm", cg.ToString());
            Console.ReadLine();
        }
    }
}
