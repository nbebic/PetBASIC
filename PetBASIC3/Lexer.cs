using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PetBASIC3
{
    public interface ILexerInput
    {
        char Next();
        char Peek();
        Tuple<int, int> Position();
    }

    public class StringInput : ILexerInput
    {
        private readonly string _input;
        private int _pos;
        public StringInput(string input)
        {
            _input = input.Replace('\'', '"') + " ";
        }

        public char Next()
        {
            return _pos < _input.Length ? _input[_pos++] : '\0';
        }

        public char Peek()
        {
            return _pos < _input.Length ? _input[_pos] : '\0';
        }

        public Tuple<int, int> Position()
        {
            return Tuple.Create(_pos, 0);
        }
    }

    public enum TokenType
    {
        None,
        String,
        Number,
        Word,
        RelOp
    }

    public class Token
    {
        public readonly string Value;
        public readonly Tuple<int, int> Position;
        public readonly TokenType Type;

        public Token(string value, Tuple<int, int> position, TokenType type)
        {
            Value = value;
            Position = position;
            Type = type;
        }

        public override string ToString() => Value;
    }

    public class Lexer
    {
        public static List<Token> Lex(ILexerInput input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var tokens = new List<Token>();
            var curType = TokenType.None;
            var curToken = new StringBuilder();
            Action<TokenType> finalize = delegate(TokenType t)
            {
                if (curToken.ToString() != "")
                    tokens.Add(new Token(curToken.ToString(), input.Position(), curType));
                curToken.Clear();
                curType = t;
            };
            while (input.Peek() != '\0')
            {
                var c = input.Next();
                if (curType == TokenType.String && c != '"')
                {
                    curToken.Append(c);
                }
                else if (char.IsDigit(c))
                {
                    if (curType != TokenType.Number)
                        finalize(TokenType.Number);
                    curToken.Append(c);
                }
                else if (char.IsWhiteSpace(c) && c != '\n')
                {
                    if (curType != TokenType.None)
                        finalize(TokenType.None);
                }
                else if (char.IsLetter(c))
                {
                    if (curType != TokenType.Word)
                        finalize(TokenType.Word);
                    curToken.Append(c);
                }
                else if ("<>=".Contains(c))
                {
                    if (curType != TokenType.RelOp)
                        finalize(TokenType.RelOp);
                    curToken.Append(c);
                }
                else if (c == '"')
                {
                    if (curType == TokenType.String)
                    {
                        curToken.Insert(0, '"');
                        curToken.Append('"');
                        finalize(TokenType.None);
                    }
                    else
                        finalize(TokenType.String);
                }
                else
                {
                    finalize(TokenType.None);
                    curToken.Append(c);
                    finalize(TokenType.None);
                }
            }
            return tokens;
        }  
    }
}
