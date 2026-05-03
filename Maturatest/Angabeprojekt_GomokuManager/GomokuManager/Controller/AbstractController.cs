using GomokuManager.Models;

namespace GomokuManager.Controller;

public abstract class AbstractController
{
    public abstract Field Gameboard { get; }
    public abstract void Clicked(Cell cell);
    public abstract bool CheckWin(CellState color);
}
