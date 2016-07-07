using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public class FunNode : AstNode
    {
        private readonly Dictionary<string[], string> BitTable = new Dictionary<string[], string>
        {
            {new[] {"SH", "A", "Q", "1", "0", "P", "EN", "SP"}, "%00000001"},
            {new[] {"Z", "S", "W", "2", "9", "O", "L", "SY"}, "%00000010"},
            {new[] {"X", "D", "E", "3", "8", "I", "K", "M"}, "%00000100"},
            {new[] {"C", "F", "R", "4", "7", "U", "J", "N"}, "%00001000"},
            {new[] {"V", "G", "T", "5", "6", "Y", "H", "B"}, "%00010000"}
        };

        private readonly Dictionary<string[], string> PortTable = new Dictionary<string[], string>
        {
            {new[] {"SH", "Z", "X", "C", "V"}, "$FEFE"},
            {new[] {"A", "S", "D", "F", "G"}, "$FDFE"},
            {new[] {"Q", "W", "E", "R", "T"}, "$FBFE"},
            {new[] {"1", "2", "3", "4", "5"}, "$F7FE"},
            {new[] {"0", "9", "8", "7", "6"}, "$EFFE"},
            {new[] {"P", "O", "I", "U", "Y"}, "$DFFE"},
            {new[] {"EN", "L", "K", "J", "H"}, "$BFFE"},
            {new[] {"SP", "SY", "M", "N", "B"}, "$7FFE"}
        };
        private string _name;
        private List<AstNode> _args;
        public FunNode(string name, List<AstNode> args)
        {
            _name = name;
            _args = args;
        } 
        public override void CodeGen(CodeGenerator cg)
        {
            if (_name.ToLower() == "inkey")
            {
                if (_args.Count != 1)
                    throw new Exception("HOW?");
                string a;
                if (_args[0] is VarNode)
                    a = _args[0].As<VarNode>().Name.ToUpper();
                else if (_args[0] is NumNode)
                    a = _args[0].As<NumNode>().Num.ToString();
                else throw new Exception("HOW?");

                var port = PortTable.FirstOrDefault(x => x.Key.Contains(a)).Value ?? "";
                var bit = BitTable.FirstOrDefault(x => x.Key.Contains(a)).Value ?? "";
                if (port == "" || bit == "")
                    throw new Exception(string.Format("Invalid key '{0}'", a));
                cg.Emit("ld", "bc", port);
                cg.Emit("in", "a", "(c)");
                cg.Emit("and", bit);
                cg.Emit("ld", "c", "a");
                cg.Emit("ld", "b", "0");
                cg.Emit("push", "bc");
                return;
            }
            foreach (var node in _args)
                node.CodeGen(cg);
            switch (_name.ToLower())
            {
                case "rnd":
                    if (_args.Count != 1)
                        throw new Exception("RND expects 1 argument");
                    cg.Emit("pop", "hl");
                    cg.Emit("ld", "hl", "0");
                    cg.Emit("push", "hl");
                    break;
                case "usr":
                    switch (_args.Count) {
                        case 1:
                            cg.Emit("pop", "hl");
                            cg.Emit("call", "(hl)");
                            break;
                        case 2:
                            cg.Emit("pop", "bc");
                            cg.Emit("pop", "hl");
                            cg.Emit("call", "(hl)");
                            break;
                        default:
                            throw new Exception("USR expects 1 or 2 arguments");
                    }
                    break;
                case "peek":
                    if (_args.Count != 1)
                        throw new Exception("PEEK expects 1 argument");
                    cg.Emit("pop", "hl");
                    cg.Emit("ld", "b", "0");
                    cg.Emit("ld", "c", "(hl)");
                    cg.Emit("push", "bc");
                    break;
            }
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
            throw new NotImplementedException();
        }
    }
}