using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSkript
{
    public class BEWEGE : Expression
    {
        double x;
        double y;
        public override void Parse(List<Token> tokens)
        {
            if (tokens[0].Type == Token.TokenType.Bewege)
            {
                tokens.RemoveAt(0);
                if (tokens[0].Type == Token.TokenType.Number)
                {
                    x = double.Parse(tokens[0].Value);
                    tokens.RemoveAt(0);
                    if (tokens[0].Type == Token.TokenType.Number)
                    {
                        y = double.Parse(tokens[0].Value);
                        tokens.RemoveAt(0);
                    }
                }
            }

        }

        public override void Run(Zeichner z)
        {
            z.Bewege(x, y);
        }
    }
}
