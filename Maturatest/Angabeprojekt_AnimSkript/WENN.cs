using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSkript
{
    public class WENN : Expression
    {
        Programm pgr;
        Token vgl;
        Token opp;
        double number;
        public override void Parse(List<Token> tokens)
        {
            if (tokens[0].Type == Token.TokenType.Wenn)
            {
                tokens.RemoveAt(0);
                if (tokens[0].Type == Token.TokenType.PosX || tokens[0].Type == Token.TokenType.PosY || tokens[0].Type == Token.TokenType.Schritt)
                {
                    vgl = tokens[0];
                    tokens.RemoveAt(0);
                    if (tokens[0].Type == Token.TokenType.Less || tokens[0].Type == Token.TokenType.Greater || tokens[0].Type == Token.TokenType.Equal)
                    {
                        opp = tokens[0];
                        tokens.RemoveAt(0);
                        if ((tokens[0].Type == Token.TokenType.Number))
                        {
                            number = double.Parse(tokens[0].Value);
                            tokens.RemoveAt(0);

                            if (tokens[0].Type == Token.TokenType.Dann)
                            {
                                tokens.RemoveAt(0);
                                if (tokens[0].Type == Token.TokenType.Lkl)
                                {
                                    tokens.RemoveAt(0);
                                    Programm pg = new Programm();
                                    pg.Parse(tokens);
                                    pgr = pg;
                                }
                            }
                        }
                    }

                }
                
                
            }
        }

        public override void Run(Zeichner z)
        {
            double val = 0;
            if (vgl.Type == Token.TokenType.PosX)
            {
                val = z.Stift.X;
            }
            if (vgl.Type == Token.TokenType.PosY)
            {
                val = z.Stift.Y;
            }
            if (vgl.Type == Token.TokenType.Schritt)
            {
                val = z.Stift.Schritt;
            }
            if (vgl.Value == "<")
            {
                if (val < number)
                {
                    pgr.Run(z);
                }
            }
            else if (vgl.Value == ">")
            {
                if (val > number)
                {
                    pgr.Run(z);
                }
            }
            else if (vgl.Value == "=")
            {
                if (val == number)
                {
                    pgr.Run(z);
                }
            }
        }
    }
}
