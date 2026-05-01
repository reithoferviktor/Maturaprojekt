using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PA2_5A_2026;
using Painter;
namespace PA2_Reithofer_Viktor
{
    public class stmt : Expression
    {
        List<Expression> expressions = new List<Expression>();

        public stmt() { }
        public override void Parse(List<Token> tokenlist)
        {
            if (tokenlist[0].Type == Token.TokenType.colr)
            {
                colr cl = new colr();
                cl.Parse(tokenlist);
                expressions.Add(cl);
            }
            else if (tokenlist[0].Type == Token.TokenType.fr)
            {
                fr frr = new fr();
                frr.Parse(tokenlist);
                expressions.Add(frr);
            }
            else if (tokenlist[0].Type == Token.TokenType.drw)
            {
                drw draww = new drw();
                draww.Parse(tokenlist);
                expressions.Add(draww);
            }
            else if (tokenlist[0].Type == Token.TokenType.trn)
            {
                trn turner = new trn();
                turner.Parse(tokenlist);
                expressions.Add(turner);
            }
            else if (tokenlist[0].Type == Token.TokenType.lkl)
            {
                trn turner = new trn();
                turner.Parse(tokenlist);
                expressions.Add(turner);
            }





        }
        public override void Run(PainterControl painter)
        {
            foreach (Expression e in expressions)
            {
                e.Run(painter);
            }
        }
    }
}
