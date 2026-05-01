using Kakuro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Matura_2023_A2
{
    public enum celltype {kaka, normal, gesperrrrt };
    internal class Cell
    {
        public celltype type;
        public KakuroControl kak;
        public TextBox TextBox = new TextBox { Text = "", Tag=""};
        public TextBlock TextBlock = new TextBlock { Background = Brushes.Black };
        public int x;
        public int y;

    }
}
