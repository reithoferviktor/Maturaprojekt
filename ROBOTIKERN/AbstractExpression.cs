using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ROBOTIKERN
{
    public abstract class AbstractExpression
    {
        public abstract void parse(List<Token> Tokenlist);
        public abstract void Run(RobotField rf);

    }
}
