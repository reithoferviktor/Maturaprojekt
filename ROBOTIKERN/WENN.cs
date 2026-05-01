using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBOTIKERN
{
    public class WENN : AbstractExpression
    {
        List<AbstractExpression> Anweisungslist = new List<AbstractExpression>();

        public string letter = "";
        public string direction = "";

        public override void parse(List<Token> Tokenlist)
        {
            if (Tokenlist[0].tokentype == tokentype.DIRECTION)
            {
                direction = Tokenlist[0].value;
                Tokenlist.RemoveAt(0);
            if (Tokenlist[0].tokentype == tokentype.ISA)
            {
                Tokenlist.RemoveAt(0);
                if (Tokenlist[0].tokentype == tokentype.LETTER)
                {
                    letter  = Tokenlist[0].value;
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
            }

        }

        public override void Run(RobotField rf)
        {
            RobotField.Direction dir = RobotField.Direction.Right;
            if (direction == "LEFT")
            {
                dir = RobotField.Direction.Left;

            }
            if (direction == "DOWN")
            {
                dir = RobotField.Direction.Down;

            }
            if (direction == "UP")
            {
                dir = RobotField.Direction.Up;

            }
            if (direction == "RIGHT")
            {
                dir = RobotField.Direction.Right;

            }
            if (rf.IsLetter(letter, dir))
            {
                Anweisungslist[0].Run(rf);
            }
        }
    }
}
