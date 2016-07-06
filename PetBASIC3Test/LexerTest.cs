using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetBASIC3;

namespace PetBASIC3Test
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void LexerTest1()
        {
            var s = Lexer.Lex(new StringInput("2 3+7 \"ma + ()gija\" (2+3*4) LET S = X"));
            var ex = new[]
            {"2", "3", "+", "7", "\"ma + ()gija\"", "(", "2", "+", "3", "*", "4", ")", "LET", "S", "=", "X"};
            Assert.AreEqual(ex.Length, s.Count);
            Assert.IsTrue(ex.Zip(s, (s1, token) => token.Value == s1).All(x => x));
        }
    }
}
