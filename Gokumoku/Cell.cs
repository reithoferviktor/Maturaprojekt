using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gokumoku
{
    public enum celltype {oppfield, ownfield, nonefield }
    public class Cell : INotifyPropertyChanged
    {
        private celltype type = celltype.nonefield;
        public celltype Type
        {
            get { return type; }
            set { type = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type))); }
        }

        public int x;
        public int y;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
