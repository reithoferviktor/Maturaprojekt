using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AnimSkript
{
    public class FARBE : Expression
    {
        string color = "";
        public override void Parse(List<Token> tokens)
        {
            if (tokens[0].Type == Token.TokenType.Farbe)
            {
                tokens.RemoveAt(0);
                if (tokens[0].Type == Token.TokenType.Color)
                {
                    color = tokens[0].Value;
                    tokens.RemoveAt(0);

                }
            }
        }

        public override void Run(Zeichner z)
        {
            z.Stift.Farbe = z.Stift.ParseFarbe(color);
        }
    }
}
