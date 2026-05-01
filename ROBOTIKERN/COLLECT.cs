using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBOTIKERN
{
    public class COLLECT : AbstractExpression
    {
        public override void parse(List<Token> Tokenlist)
        {
            
        }

        public override void Run(RobotField rf)
        {
            rf.Collect();
        }
    }
}
