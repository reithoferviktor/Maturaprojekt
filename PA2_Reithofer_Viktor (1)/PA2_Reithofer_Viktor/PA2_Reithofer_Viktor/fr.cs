using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PA2_5A_2026;
using Painter;
namespace PA2_Reithofer_Viktor
{
    public class fr : Expression
    {
        List<Expression> expressions = new List<Expression>();
        int amnt = 0;
        public fr() { }
        public override void Parse(List<Token> tokenlist)
        {
            if (tokenlist.Count > 4)
            {
                if (tokenlist[0].Type == Token.TokenType.fr)
                {
                    tokenlist.RemoveAt(0);
                    if (tokenlist[0].Type == Token.TokenType.number)
                    {
                        amnt = int.Parse(tokenlist[0].Value);
                        tokenlist.RemoveAt(0);
                        if (tokenlist[0].Type == Token.TokenType.lkl)
                        {
                            blck block = new blck();
                            block.Parse(tokenlist);
                            expressions.Add(block);
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Nummer erwartet. Folgendes erhalten: " + tokenlist[0].Type + " " + tokenlist[0].Value);
                        throw new Exception("Fehler beim Parsen");


                    }

                }
            }
        }
        public override void Run(PainterControl painter)
        {
            for (int i = 0; i < amnt; i++)
            {
                foreach (Expression e in expressions)
                {
                    e.Run(painter);
                }
            }
        }
    }
}
