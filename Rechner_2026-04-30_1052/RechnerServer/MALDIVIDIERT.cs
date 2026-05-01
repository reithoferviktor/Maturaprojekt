using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RechnerServer
{
    public class MALDIVIDIERT : AbstractExpr
    {
        public override List<AbstractExpr> Anweisungslist { get; set; } = new List<AbstractExpr>();

        public List<string> opp = new();
        public override void Parse(List<Token> tokens)
        {
            HOCH h = new HOCH();
            Anweisungslist.Add(h);
            h.Parse(tokens);
            while (tokens.Count > 0)
            {
                if (tokens[0].Type == Tokentype.MD)
                {
                    opp.Add(tokens[0].value);
                    tokens.RemoveAt(0);
                    HOCH H = new HOCH();
                    H.Parse(tokens);
                    Anweisungslist.Add(H);
                }
                else
                {
                    return;
                }
            }
        }

        public override double Run()
        {
            double start = Anweisungslist[0].Run();
            Anweisungslist.RemoveAt(0);
            while (Anweisungslist.Count > 0 )
            {
                if (opp[0] == "*")
                {
                    opp.RemoveAt(0);
                    start = start * Anweisungslist[0].Run();
                    Anweisungslist.RemoveAt(0);


                }
                else if (opp[0] == "/")
                {
                    opp.RemoveAt(0);
                    start = start / Anweisungslist[0].Run();
                    Anweisungslist.RemoveAt(0);

                }
            }
            return start;
        }
    }
}
