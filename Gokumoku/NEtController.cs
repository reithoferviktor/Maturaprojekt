using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gokumoku
{
    public class NEtController : AbstractController, Receiver
    {
        Transfer t;
        public override Field gameboard { get; set; }
        public override int gamesize { get; set; }
        bool dubistdran = false;

        public NEtController(int gs)
        {
            t = new Transfer(new System.Net.Sockets.TcpClient("localhost", 12345), this);
            t.Start();
            gamesize = gs;
            gameboard = new Field(gamesize);
        }
        public override void Clicked(Cell zelle)
        {
            if (zelle.Type == celltype.nonefield)
            {
                if (dubistdran)
                {
                    zelle.Type = celltype.ownfield;
                    t.Send(new MSG { type = msgtype.ZUG, x = zelle.x, y = zelle.y });
                    dubistdran = false;
                }
                else
                {
                    MessageBox.Show("Du bist nicht dran!");

                }
            }
            else
            {
                MessageBox.Show("Nur leere felder sind auswählbar");
            }
        }
        public void ReceiveMessage(MSG m, Transfer t)
        {
            if (m.type == msgtype.ZUG)
            {
                var curry = gameboard.cells.First(c => c.x == m.x && c.y == m.y);
                gameboard.cells[gameboard.cells.IndexOf(curry)].Type = celltype.oppfield;
                if (ISLost())
                {
                    MessageBox.Show("Du hast verloren!");

                }
                dubistdran = true;
                
            }
            else if (m.type == msgtype.DUBISTSTART)
            {
                dubistdran = true;
                MessageBox.Show("Du bist dran!");
            }
        }

        public void TransferDisconnected(Transfer t)
        {
        }

        public void AddDebugInfo(Transfer t, string m, bool sent)
        {
        }

        public override bool ISLost()
        {
            int x = 0;
            int counter = 0;
            foreach (var cell in gameboard.cells)
            {
                if (cell.x > x)
                {
                    counter = 0;
                }
                if (cell.Type != celltype.oppfield)
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                    if (counter == 5)
                    {
                        return true;
                    }
                }
            }
            for (int i = 0; i < gamesize; i++)
            {
                counter = 0;
                var curry = gameboard.cells.Where(t => t.y == i);
                foreach (var item in curry)
                {
                    if (item.Type != celltype.oppfield)
                    {
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                        if (counter == 5)
                        {
                            return true;
                        }
                    }
                }
            }
            for (int i = 0; i < gamesize*gamesize; i++)
            {
                int d = i;
                counter = 0;

                while (d < gamesize * gamesize)
                {
                    Cell cell = gameboard.cells[d];
                    if (cell.Type != celltype.oppfield)
                    {
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                        if (counter == 5)
                        {
                            return true;
                        }
                    }
                    d += gamesize + 1;
                }
            }
            for (int i = 0; i < gamesize * gamesize; i++)
            {
                int d = i;
                counter = 0;

                while (d > 0)
                {
                    Cell cell = gameboard.cells[d];
                    if (cell.Type != celltype.oppfield)
                    {
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                        if (counter == 5)
                        {
                            return true;
                        }
                    }
                    d -= (gamesize - 1);
                }
            }


            return false;

        }
    }
}
