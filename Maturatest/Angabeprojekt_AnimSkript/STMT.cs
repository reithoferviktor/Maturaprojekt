using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSkript
{
    public class STMT : Expression
    {
        Expression Statement;
        public override void Parse(List<Token> tokens)
        {
            if (tokens[0].Type == Token.TokenType.Dicke)
            {
                DICKE d  = new DICKE();
                d.Parse(tokens);
                Statement = d;
            }
            else if (tokens[0].Type == Token.TokenType.Farbe)
            {
                FARBE fb = new FARBE();
                fb.Parse(tokens);
                Statement = fb;
            }
            else if (tokens[0].Type == Token.TokenType.Wenn)
            {
                WENN wn = new WENN();
                wn.Parse(tokens);
                Statement = wn;
            }
            else if (tokens[0].Type == Token.TokenType.Wiederhole)
            {
                WIEDERHOLE ww = new WIEDERHOLE();
                ww.Parse(tokens);
                Statement = ww;
            }
            else if (tokens[0].Type == Token.TokenType.Bewege)
            {
                BEWEGE bg = new BEWEGE();
                bg.Parse(tokens);
                Statement = bg;
            }
            else if (tokens[0].Type == Token.TokenType.Position)
            {
                POSITION bg = new POSITION();
                bg.Parse(tokens);
                Statement = bg;
            }
        }

        public override void Run(Zeichner z)
        {
            Statement.Run(z);
        }
    }
}
