using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2_Reithofer_Viktor
{
    public class Token
    {
        public enum TokenType { fr, lkl, rkl, drw, error, number, colr, trn, direction, brsh };

        public TokenType Type;
        public string Value;


        public Token()
        {
            Type = TokenType.error;
            Value = "Error";
        }

    }
}
