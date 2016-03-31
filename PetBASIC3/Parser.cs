using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetBASIC3.AST;

namespace PetBASIC3
{
    class Parser
    {
        private List<Token> _tokens;
        private int _pos;
        private readonly Stack<int> _tryStack = new Stack<int>(); 

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        private Token Next() => _pos < _tokens.Count ? _tokens[_pos++] : null;
        private Token Peek() => _pos < _tokens.Count ? _tokens[_pos] : null;

        private bool Attempt()
        {
            _tryStack.Push(_pos);
            return true;
        }
        private bool Succeed()
        {
            _tryStack.Pop();
            return true;
        }
        private bool Fail()
        {
            _pos = _tryStack.Pop();
            return false;
        }

        public bool ParseProgram(out List<AstNode> node)
        {
            List<AstNode> nodes = new List<AstNode>();

            while (true)
            {
                while (ParseLiteral("\n")) { }
                AstNode n, stmt;
                if (!ParseNum(out n)) break;
                ParseStatement(out stmt);
                nodes.Add(new LineNode(stmt, n.As<NumNode>().Num));
            }

            node = nodes;
            return true;
        }

        public bool ParseStatement(out AstNode node)
        {
            if (ParseLiteral("print"))
            {
                List<AstNode> nodes;
                ParseExprList(out nodes);
                node = new PrintNode(nodes);
                return true;
            }

            if (ParseLiteral("if"))
            {
                AstNode lhs, rhs, stmt;
                string rel;
                ParseExpr(out lhs);
                ParseRelop(out rel);
                ParseExpr(out rhs);
                ParseLiteral("then");
                ParseStatement(out stmt);
                node = new IfNode(lhs, rhs, rel, stmt);
                return true;
            }

            if (ParseLiteral("goto"))
            {
                AstNode n;
                ParseNum(out n);
                node = new GoToNode(n.As<NumNode>().Num);
                return true;
            }
            if (ParseLiteral("input"))
            {
                List<AstNode> nodes;
                ParseVarList(out nodes);
                node = new InputNode(nodes.Select(x => x.As<VarNode>().Name).ToList());
                return true;
            }
            if (ParseLiteral("gosub"))
            {
                AstNode n;
                ParseNum(out n);
                node = new GoSubNode(n.As<NumNode>().Num);
                return true;
            }
            if (ParseLiteral("return"))
            {
                node = new ReturnNode();
                return true;
            }
            if (ParseLiteral("let"))
            {
                string s;
                AstNode n;
                if (ParseLiteral("["))
                {
                    AstNode n2;
                    ParseExpr(out n2);
                    ParseLiteral("]");
                    ParseLiteral("=");
                    ParseExpr(out n);
                    node = new PokeNode(n2, n);
                    return true;
                }
                ParseWord(out s);
                ParseLiteral("=");
                ParseExpr(out n);
                node = new LetNode(s, n);
                return true;
            }
            if (ParseLiteral("poke"))
            {
                AstNode n, m;
                ParseExpr(out n);
                ParseLiteral(",");
                ParseExpr(out m);
                node = new PokeNode(n, m);
                return true;
            }
            if (ParseLiteral("halt"))
            {
                node = new HaltNode();
                return true;
            }
            if (ParseLiteral("point"))
            {
                AstNode n, m;
                ParseExpr(out n);
                ParseLiteral(",");
                ParseExpr(out m);
                node = new PointNode("set", n, m);
                return true;
            }
            throw new Exception("Expected stmt at " + Peek()?.Position + " found " + Peek()?.Value);
        }

        public bool ParseExprList(out List<AstNode> node)
        {
            node = new List<AstNode>();
            do
            {
                AstNode n;
                if (ParseString(out n))
                    node.Add(n);
                else if (Attempt() && ParseExpr(out n) && Succeed() || Fail())
                    node.Add(n);
                else
                    throw new Exception("Expected string or expr at" + Peek()?.Position);
            } while (ParseLiteral(","));
            return true;
        }

        public bool ParseVarList(out List<AstNode> node)
        {
            node = new List<AstNode>();
            do
            {
                string n;
                if (ParseWord(out n))
                    node.Add(new VarNode(n));
                else
                    throw new Exception("Expected string or expr at" + Peek()?.Position + " found " + Peek()?.Value);
            } while (ParseLiteral(","));
            return true;
        }

        private bool ParseFactor(out AstNode node)
        {
            string s = "";
            List<AstNode> nodes = null;
            if (ParseLiteral("["))
            {
                ParseExpr(out node);
                ParseLiteral("]");
                node = new FunNode("peek", new List<AstNode> {node});
                return true;
            }
            if ((Attempt() && ParseWord(out s) && ParseLiteral("(") && ParseExprList(out nodes) && ParseLiteral(")") &&
                Succeed()) || Fail())
            {
                node = new FunNode(s, nodes);
                return true;
            }
            if (ParseWord(out s))
            {
                node = new VarNode(s);
                return true;
            }
            if (ParseNum(out node))
                return true;
            if ((Attempt() && ParseLiteral("(") && ParseExpr(out node)
                 && ParseLiteral(")") && Succeed()) || Fail())
                return true;
            node = null;
            return false;
        }

        private bool ParseTerm(out AstNode node)
        {
            var nodes = new List<AstNode>();
            var ops = new List<string>();
            
            AstNode n;

            if (!ParseFactor(out n))
            {
                throw new Exception("Expected term at " + Peek()?.Position + " found " + Peek()?.Value);
            }

            nodes.Add(n);

            while (true)
            {
                if (ParseLiteral("*")) ops.Add("*");
                else if (ParseLiteral("/")) ops.Add("/");
                else break;
                if (!ParseFactor(out n))
                    throw new Exception("Expected term at " + Peek()?.Position + " found " + Peek()?.Value);
                nodes.Add(n);
            }

            n = nodes.First();
            Debug.Assert(nodes.Count - 1 == ops.Count);
            for (int i = 1; i < nodes.Count; i++)
            {
                n = new ExprNode(n, nodes[i], ops[i-1][0]);
            }
            node = n;
            return true;
        }

        public bool ParseExpr(out AstNode node)
        {
            var nodes = new List<AstNode>();
            var ops = new List<string>();
            if (ParseLiteral("-"))
            {
                nodes.Add(new NumNode(0));
                ops.Add("-");
            }
            else
                ParseLiteral("+");

            AstNode n;

            if (!ParseTerm(out n))
            {
                throw new Exception("Expected term at " + Peek()?.Position + " found " + Peek()?.Value);
            }

            nodes.Add(n);

            while (true)
            {
                if (ParseLiteral("+")) ops.Add("+");
                else if (ParseLiteral("-")) ops.Add("-");
                else break;
                if (!ParseTerm(out n))
                    throw new Exception("Expected term at " + Peek()?.Position + " found " + Peek()?.Value);
                nodes.Add(n);
            }

            n = nodes.First();
            Debug.Assert(nodes.Count - 1 == ops.Count);
            for (int i = 1; i < nodes.Count; i++)
            {
                n = new ExprNode(n, nodes[i], ops[i-1][0]);
            }

            node = n;
            return true;
        }

        private bool ParseLiteral(string lit)
        {
            if (Peek()?.Value.ToLower() != lit.ToLower())
                return false;
            Next();
            return true;
        }
        private bool ParseNum(out AstNode node)
        {
            if (Peek()?.Type == TokenType.Number)
            {
                node = new NumNode(int.Parse(Next().Value ?? ""));
                return true;
            }
            node = null;
            return false;
        }

        private bool ParseRelop(out string node)
        {
            if (Peek()?.Type == TokenType.RelOp)
            {
                node = Next().Value;
                return true;
            }
            node = null;
            return false;
        }

        private bool ParseWord(out string node)
        {
            if (Peek()?.Type == TokenType.Word)
            {
                node = Next().Value;
                return true;
            }
            node = null;
            return false;
        }

        private bool ParseString(out AstNode node)
        {
            if (Peek()?.Type == TokenType.String)
            {
                node = new StringNode(Next().Value);
                return true;
            }
            node = null;
            return false;
        }

    }
}
