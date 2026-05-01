using PA2_Reithofer_Viktor;
using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2_5A_2026
{
    public class program : Expression
    {
        List<Expression> expressions = new List<Expression>();
        public override void Parse(List<Token> tokenlist)
        {
            while (tokenlist.Count > 0) { 
                stmt st = new stmt();
                st.Parse(tokenlist);
                expressions.Add(st);
            }
        }
        public override void Run(PainterControl painter)
        {
            foreach (Expression e in expressions) {
               e.Run(painter);
            }
        }
    }
}
