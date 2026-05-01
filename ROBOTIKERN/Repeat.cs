using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBOTIKERN
{
    public class Repeat : AbstractExpression
    {
        List<AbstractExpression> Anweisungslist = new();
        int amount = 0;

        public override void parse(List<Token> Tokenlist)
        {
            
            if (Tokenlist[0].tokentype == tokentype.NUMBER)
            {
                amount = int.Parse(Tokenlist[0].value);
                Tokenlist.RemoveAt(0);
                if (Tokenlist[0].tokentype == tokentype.LKL)
                {
                    Tokenlist.RemoveAt(0);
                    Programm progr = new Programm();
                    progr.parse(Tokenlist);
                    Anweisungslist.Add(progr);
                }
            }
        }

        public override void Run(RobotField rf)
        {
            for (int i = 0; i < amount; i++)
            {
                Anweisungslist[0].Run(rf);
            }
        }
    }
}
