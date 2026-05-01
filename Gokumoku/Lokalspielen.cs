using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gokumoku
{
    public class Lokalspielen : AbstractController
    {
        public Lokalspielen(int gs)
        {
            gamesize = gs;
            gameboard = new Field(gamesize);
        }
        public override Field gameboard { get; set; }
        public override int gamesize { get; set; }
        bool dubistdran = false;


        public override void Clicked(Cell zelle)
        {
            if (zelle.Type == celltype.nonefield)
            {
                if (dubistdran)
                {

                    zelle.Type = celltype.ownfield;
                    dubistdran = false;
                    if (ISLost2())
                    {
                        MessageBox.Show("Blau hat verloren");

                    }
                }
                else
                {

                    zelle.Type = celltype.oppfield;
                    dubistdran = true;
                    if (ISLost())
                    {
                        MessageBox.Show("Lila hat verloren");

                    }

                }
            }
            else
            {
                MessageBox.Show("Nur leere felder sind auswählbar");
            }
        }

        public  bool ISLost2()
        {
            int x = 0;
            int counter = 0;
            foreach (var cell in gameboard.cells)
            {
                if (cell.x > x)
                {
                    counter = 0;
                }
                if (cell.Type != celltype.ownfield)
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
                    if (item.Type != celltype.ownfield)
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
            for (int i = 0; i < gamesize * gamesize; i++)
            {
                int d = i;
                counter = 0;

                while (d < gamesize * gamesize)
                {
                    Cell cell = gameboard.cells[d];
                    if (cell.Type != celltype.ownfield)
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
                    if (cell.Type != celltype.ownfield)
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
            for (int i = 0; i < gamesize * gamesize; i++)
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

