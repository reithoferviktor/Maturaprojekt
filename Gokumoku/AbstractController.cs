using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gokumoku
{
    public abstract class AbstractController
    {
         public abstract Field gameboard { get; set; }
         public abstract int gamesize {get; set; }
        public abstract void Clicked(Cell Zelle);
        public abstract bool ISLost();
    }   
}
