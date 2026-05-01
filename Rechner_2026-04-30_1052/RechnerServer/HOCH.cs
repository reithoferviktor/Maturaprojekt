using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RechnerServer
{
    public class HOCH : AbstractExpr
    {
        public override List<AbstractExpr> Anweisungslist { get; set; } = new List<AbstractExpr>();

        public double? value;

        public override void Parse(List<Token> tokens)
        {

            while (tokens.Count > 0)
            {
                if (tokens[0].Type == Tokentype.H)
                {
                    tokens.RemoveAt(0);
                    HOCH H = new HOCH();
                    H.Parse(tokens);
                    Anweisungslist.Add(H);
                }
                else if(tokens[0].Type == Tokentype.Z)
                {
                    value = float.Parse(tokens[0].value);
                    tokens.RemoveAt(0);
                }
                else if (tokens[0].Type == Tokentype.LKL)
                {
                    tokens.RemoveAt(0);
                    PLUSMINUS pm = new PLUSMINUS();
                    pm.Parse(tokens);
                    value = pm.Run();
                }
                else
                {
                    return;
                }
            }
        }

        public override double Run()
        {
            if (Anweisungslist.Count == 0)
            {

                return (double)value;

            }
            return Math.Pow((double)value, Anweisungslist[0].Run());
        }
    }
}
