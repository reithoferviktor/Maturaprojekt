using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gokumoku
{
    public  class Field
    {
        public ObservableCollection<Cell> cells { get; set; }

        int gamesize;

        public Field(int gamesize)
        {
            cells = new ObservableCollection<Cell>();
            this.gamesize = gamesize;
            for (int i = 0; i < gamesize; i++)
            {
                for (global::System.Int32 j = 0; j < gamesize; j++)
                {
                    cells.Add(new Cell { x = i, y = j });
                }
            }
        }
    }
}
