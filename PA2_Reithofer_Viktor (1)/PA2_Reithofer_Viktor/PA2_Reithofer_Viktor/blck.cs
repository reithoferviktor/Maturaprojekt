using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PA2_5A_2026;
using Painter;
namespace PA2_Reithofer_Viktor
{
    public class blck : Expression
    {
        public blck() { }
        List<Expression> expressions = new List<Expression>();

        public override void Parse(List<Token> tokenlist)
        {
            if (tokenlist[0].Type == Token.TokenType.lkl)
            {
                tokenlist.RemoveAt(0);
            }
            else
            {


                System.Windows.MessageBox.Show("Like klammer erwartet. Folgendes erhalten: " + tokenlist[0].Type + " " +  tokenlist[0].Value);
                throw new Exception("Fehler beim Parsen");

            }


            while (tokenlist[0].Type != Token.TokenType.rkl)
                {
                    stmt st = new stmt();
                    st.Parse(tokenlist);
                    expressions.Add(st);
                }
            if (tokenlist[0].Type == Token.TokenType.rkl)
            {
                tokenlist.RemoveAt(0);

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
