using GomokuManager.Models;

namespace GomokuManager.Controller;

// ============================================================
// TODO 2a — MVC LokalController
// Implementiere Clicked() fuer ein lokales Zwei-Spieler-Spiel.
//
// Logik:
//   - Ist die Zelle schon belegt (State != Empty) → ignorieren
//   - Ansonsten: Stein setzen (abwechselnd Black und White)
//   - isBlack nach jedem Zug umschalten
// ============================================================
public class LokalController : AbstractController
{
    private bool isBlack = true;
    private Field board = new Field(15);

    public override Field Gameboard => board;

    public override void Clicked(Cell cell)
    {
        // TODO: Stein setzen und isBlack umschalten
        throw new NotImplementedException();
    }

    public override bool CheckWin(CellState color)
    {
        // Optionales Extra: 5 in einer Reihe prüfen
        return false;
    }
}
