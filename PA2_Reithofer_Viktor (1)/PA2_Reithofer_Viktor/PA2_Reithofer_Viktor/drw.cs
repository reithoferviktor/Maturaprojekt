using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PA2_5A_2026;
using Painter;
namespace PA2_Reithofer_Viktor
{
    public class drw : Expression
    {
        public drw() { }
        int lnght = 0;
        public override void Parse(List<Token> tokenlist)
        {
            if (tokenlist.Count > 1)
            {
                if (tokenlist[0].Type == Token.TokenType.drw)
                {
                    tokenlist.RemoveAt(0);
                    if (tokenlist[0].Type == Token.TokenType.number)
                    {
                        lnght = int.Parse(tokenlist[0].Value);
                        tokenlist.RemoveAt(0);

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
           painter.Draw(lnght);
        }
    }
}
