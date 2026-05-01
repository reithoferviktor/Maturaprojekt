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
    public class colr: Expression
    {
       public colr() { }
        string farbe = "";
        public override void Parse(List<Token> tokenlist)
        {
            if (tokenlist.Count > 1)
            {
                if (tokenlist[0].Type == Token.TokenType.colr)
                {
                    tokenlist.RemoveAt(0);
                    if (tokenlist[0].Type == Token.TokenType.brsh)
                    {
                        farbe = tokenlist[0].Value;
                        tokenlist.RemoveAt(0);

                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Farbe erwartet. Folgendes erhalten: " + tokenlist[0].Type + " " + tokenlist[0].Value);
                        throw new Exception("Fehler beim Parsen");


                    }

                }
            }
        }
        public override void Run(PainterControl painter)
        {
            painter.ChangeColor(farbe);
        }
    }
}
