using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbcRobotCore.RobotField;

namespace ROBOTIKERN
{
    public class MOVE : AbstractExpression
    {
        String direction = "";
        public override void parse(List<Token> Tokenlist)
        {
            if (Tokenlist[0].tokentype == tokentype.DIRECTION)
            {
                direction = Tokenlist[0].value;
                Tokenlist.RemoveAt(0);
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
            rf.Move(dir);
            
        }
    }
}
