using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RechnerServer
{
    public abstract class AbstractExpr
    {
        public abstract List<AbstractExpr> Anweisungslist { get; set;}
        public abstract void Parse(List<Token> tokens);
        public abstract double Run();
    }
}
