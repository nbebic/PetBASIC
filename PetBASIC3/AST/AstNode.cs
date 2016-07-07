using System.CodeDom;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetBASIC3.CodeGen;

namespace PetBASIC3.AST
{
    public abstract class AstNode
    {
        public abstract void CodeGen(CodeGenerator cg);

        public abstract void CodeGenBasicalPre(CodeGenerator cg);
        public abstract void CodeGenBasicalCalculate(CodeGenerator cg);
        public abstract void CodeGenBasicalDo(CodeGenerator cg);

    }
}
