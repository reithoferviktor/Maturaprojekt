using System.Collections.ObjectModel;

namespace GomokuManager.Models;

public class Field
{
    public int Size { get; }
    public ObservableCollection<Cell> Cells { get; } = new();

    public Field(int size)
    {
        Size = size;
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                Cells.Add(new Cell { X = x, Y = y });
    }

    public Cell Get(int x, int y) => Cells.First(c => c.X == x && c.Y == y);

    public void Reset()
    {
        foreach (var c in Cells) c.State = CellState.Empty;
    }
}
