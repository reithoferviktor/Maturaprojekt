using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBOTIKERN
{
    public class STMT : AbstractExpression
    {
        List<AbstractExpression> Anweisungslist = new();

        public override void parse(List<Token> Tokenlist)
        {

                if (Tokenlist[0].tokentype == tokentype.REPEAT)
                {
                    Tokenlist.RemoveAt(0);
                    Repeat rp  = new Repeat();
                    rp.parse(Tokenlist);
                    Anweisungslist.Add(rp);
                }
                else if (Tokenlist[0].tokentype == tokentype.UNTIL)
                {
                    Tokenlist.RemoveAt(0);
                    UNTIL ut = new UNTIL();
                    ut.parse(Tokenlist);
                    Anweisungslist.Add(ut);
                 }
                else if (Tokenlist[0].tokentype == tokentype.WENN)
                {
                    Tokenlist.RemoveAt(0);
                    WENN wn = new WENN();
                    wn.parse(Tokenlist);
                    Anweisungslist.Add(wn);
                }
                else if (Tokenlist[0].tokentype == tokentype.MOVE)
                {
                    Tokenlist.RemoveAt(0);
                    MOVE mv = new MOVE();
                    mv.parse(Tokenlist);
                    Anweisungslist.Add(mv);
                }
            else if (Tokenlist[0].tokentype == tokentype.COLLECT)
            {
                Tokenlist.RemoveAt(0);
                COLLECT cl = new COLLECT();
                cl.parse(Tokenlist);
                Anweisungslist.Add(cl);
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
