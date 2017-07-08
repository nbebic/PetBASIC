using System;
using System.Collections.Generic;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST.Commands
{
    class ForNode : AstNode
    {
        public static readonly Dictionary<string, Action<CodeGenerator, int>> ForActs =
            new Dictionary<string, Action<CodeGenerator, int>>();
        private static int _curFor;
        private VarNode _v;
        private AstNode _start;
        private AstNode _end;

        public ForNode(string v, AstNode start, AstNode end)
        {
            _v = new VarNode(v);
            _start = start;
            _end = end;
        }
        public override void CodeGen(CodeGenerator cg)
        {
            _start.CodeGen(cg);
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(vars+" + _v.Address + ")", "hl");
            var cf = _curFor++;
            cg.Label("for" + cf);
            ForActs.Add(_v.Name, (gen, i) =>
            {
                _end.CodeGen(gen);
                _v.CodeGen(gen);
                // bc = end
                // hl = v
                gen.Emit("pop", "hl");
                gen.Emit("pop", "bc");
                gen.Emit("or", "a");
                // while (v <= end) <=> while !(end < v)
                // end - v; jp m, next <=> if (end < v) break;
                gen.Emit("sbc", "hl", "bc");
                gen.Emit("jp", "p", "forn" + i);
                gen.Emit("ld", "hl", "(vars+" + _v.Address + ")");
                gen.Emit("inc", "hl");
                gen.Emit("ld", "(vars+" + _v.Address + ")", "hl");
                gen.Emit("jp", "for" + cf);
                gen.Label("forn" + i);
            });
        }

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            throw new NotImplementedException();
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
            throw new NotImplementedException();
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            _start.CodeGen(cg);
            cg.Emit("pop", "hl");
            cg.Emit("ld", "(vars+" + _v.Address + ")", "hl");
            var cf = _curFor++;
            cg.Label("for" + cf);
            ForActs.Add(_v.Name, (gen, i) =>
            {
                _end.CodeGen(gen);
                _v.CodeGen(gen);
                // bc = end
                // hl = v
                gen.Emit("pop", "hl");
                gen.Emit("pop", "bc");
                gen.Emit("or", "a");
                // while (v <= end) <=> while !(end < v)
                // end - v; jp m, next <=> if (end < v) break;
                gen.Emit("sbc", "hl", "bc");
                gen.Emit("jp", "p", "forn" + i);
                gen.Emit("ld", "hl", "(vars+" + _v.Address + ")");
                gen.Emit("inc", "hl");
                gen.Emit("ld", "(vars+" + _v.Address + ")", "hl");
                gen.Emit("jp", "for" + cf);
                gen.Label("forn" + i);
            });
        }
    }


    public class NextNode : AstNode
    {
        private static int _i;
        private readonly string _v;
        public NextNode(string v)
        {
            _v = v;
        }

        public override void CodeGen(CodeGenerator cg)
        {
            if (!ForNode.ForActs.ContainsKey(_v))
                throw new Exception("NEXT " + _v + " before corresponding FOR");
            ForNode.ForActs[_v](cg, _i++);
        }

        public override void CodeGenBasicalPre(CodeGenerator cg)
        {
            throw new NotImplementedException();
        }

        public override void CodeGenBasicalCalculate(CodeGenerator cg)
        {
            throw new NotImplementedException();
        }

        public override void CodeGenBasicalDo(CodeGenerator cg)
        {
            if (!ForNode.ForActs.ContainsKey(_v))
                throw new Exception("NEXT " + _v + " before corresponding FOR");
            ForNode.ForActs[_v](cg, _i++);
        }
    }
}
