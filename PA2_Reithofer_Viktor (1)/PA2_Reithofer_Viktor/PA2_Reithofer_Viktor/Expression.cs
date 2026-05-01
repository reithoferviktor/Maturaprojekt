using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PA2_Reithofer_Viktor;

namespace PA2_5A_2026
{
    public abstract class Expression
    {
        public static List<String> Errors { get; set; } = new List<String>(); 
        public abstract void Parse(List<Token> tokenlist);
        public abstract void Run(PainterControl painter);
    }
}
