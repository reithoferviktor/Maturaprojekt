using PA2_5A_2026;
using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
namespace PA2_Reithofer_Viktor
{
    public class trn : Expression
    {
        int dgrs = 0;
        String direction = "";
        public trn() { }
        public override void Parse(List<Token> tokenlist)
        {
            if (tokenlist.Count > 2)
            {
                if (tokenlist[0].Type == Token.TokenType.trn)
                {
                    tokenlist.RemoveAt(0);
                    if (tokenlist[0].Type == Token.TokenType.direction)
                    {
                        direction = tokenlist[0].Value;
                        tokenlist.RemoveAt(0);
                        if (tokenlist[0].Type == Token.TokenType.number)
                        {
                            dgrs = int.Parse(tokenlist[0].Value);
                            tokenlist.RemoveAt(0);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Nummer erwartet. Folgendes erhalten: " + tokenlist[0].Type + " " + tokenlist[0].Value);
                            throw new Exception("Fehler beim Parsen");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Direction erwartet. Folgendes erhalten: " + tokenlist[0].Type + " " + tokenlist[0].Value);
                        throw new Exception("Fehler beim Parsen");

                    }
                }

            }
        }
        public override void Run(PainterControl painter)
        {
            /*if (direction == "RIGHT")
            {
                dgrs *= -1;
                painter.Rotate(dgrs);
            }
            else if (direction == "LEFT")
            {
                
                painter.Rotate(dgrs);
            }*/
            painter.Rotate(dgrs);

        }
    }
}
