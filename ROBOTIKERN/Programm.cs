using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBOTIKERN
{
    public class Programm : AbstractExpression
    {
        List<AbstractExpression> Anweisungslist = new ();

        public override void parse(List<Token> Tokenlist)
        {
            while (Tokenlist.Count > 0)
            {
                if (Tokenlist[0].tokentype == tokentype.RKL)
                {
                    Tokenlist.RemoveAt(0);
                    return;
                }
                STMT stmt = new STMT();
                    stmt.parse(Tokenlist);
                    Anweisungslist.Add(stmt);
                    
            }
        }

        public override void Run(RobotField rf)
        {
            foreach (var item in Anweisungslist)
            {
                item.Run(rf);
            }
        }
    }
}
