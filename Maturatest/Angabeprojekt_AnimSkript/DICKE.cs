using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSkript
{
    public class DICKE : Expression
    {
        double d;
        public override void Parse(List<Token> tokens)
        {
            if (tokens[0].Type == Token.TokenType.Dicke)
            {
                tokens.RemoveAt(0);
                if (tokens[0].Type == Token.TokenType.Number)
                {
                    d = double.Parse(tokens[0].Value);
                    tokens.RemoveAt(0);
 
                }
            }
        }

        public override void Run(Zeichner z)
        {
            z.Stift.Dicke = d;
        }
    }
}
