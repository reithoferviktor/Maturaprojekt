using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSkript
{
    public class WIEDERHOLE : Expression
    {
        Programm pg;
        int mal = 0;
        public override void Parse(List<Token> tokens)
        {
            if(tokens[0].Type == Token.TokenType.Wiederhole)
            {
                tokens.RemoveAt(0);
                if (tokens[0].Type == Token.TokenType.Number)
                {
                    mal = int.Parse(tokens[0].Value);
                    tokens.RemoveAt(0);
                    if (tokens[0].Type == Token.TokenType.Mal)
                    {
                        tokens.RemoveAt(0);
                        if (tokens[0].Type == Token.TokenType.Lkl)
                        {
                            tokens.RemoveAt(0);
                            pg = new Programm();
                            pg.Parse(tokens);

                        }

                    }

                }

            }
        }

        public override void Run(Zeichner z)
        {
            for (int i = 0; i < mal; i++)
            {
                pg.Run(z);
            }
        }
    }
}
