using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1
{
    public abstract class AbstractExpression
    {
        public void ParseFormel(List<char> formel)
        {

        }

        //Variablenliste wird beim Parsen befüllt
        public static List<char> variables = new List<char>();

        public bool Interpret(Dictionary<char, bool> context)
        {
            return false;
        }
    }
}
