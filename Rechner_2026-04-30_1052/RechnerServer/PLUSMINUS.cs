using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RechnerServer
{
    public class PLUSMINUS : AbstractExpr
    {
        public override List<AbstractExpr> Anweisungslist { get; set; } =  new List<AbstractExpr>();

        public List<string> opp = new();
        public override void Parse(List<Token> tokens)
        {
            MALDIVIDIERT md = new MALDIVIDIERT();
            Anweisungslist.Add(md);
            md.Parse(tokens);
            while (tokens.Count > 0)
            {
                if (tokens[0].Type == Tokentype.PM)
                {
                    opp.Add(tokens[0].value);
                    tokens.RemoveAt(0);
                }
                else if(tokens[0].Type == Tokentype.RKL)
                {
                    tokens.RemoveAt(0);
                    return;
                }
                else
                {
                    MALDIVIDIERT mdv = new MALDIVIDIERT();
                    Anweisungslist.Add(mdv);
                    mdv.Parse(tokens);
                }
            }
        }

        public override double Run()
        {
            double start = Anweisungslist[0].Run();
            Anweisungslist.RemoveAt(0);

            while (Anweisungslist.Count > 0)
            {
                if (opp[0] == "+")
                {
                    opp.RemoveAt(0);
                    start = start + Anweisungslist[0].Run();
                    Anweisungslist.RemoveAt(0);

                }
                else if (opp[0] == "-")
                {
                    opp.RemoveAt(0);
                    start = start - Anweisungslist[0].Run();
                    Anweisungslist.RemoveAt(0);

                }
            }
            return start;
        }
    }
}
