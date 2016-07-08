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
5 let n = 29303
10 for i = 2 to n-1
20 if n = (n/i)*i then goto 200
30 next i
40 print ""prime""
50 return
200 print i, "" not prime""
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
