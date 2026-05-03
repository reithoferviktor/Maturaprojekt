using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GomokuManager.Models;

// ============================================================
// TODO 1 — INotifyPropertyChanged
// Implementiere das Interface INotifyPropertyChanged sodass
// sich die UI automatisch aktualisiert wenn State gesetzt wird.
//
// Schritte:
//   1. : INotifyPropertyChanged ergaenzen
//   2. event PropertyChangedEventHandler? PropertyChanged; deklarieren
//   3. OnPropertyChanged() Hilfsmethode anlegen (CallerMemberName!)
//   4. State-Property so umbauen:
//        get => _state;
//        set { _state = value; OnPropertyChanged(); }
// ============================================================
public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }

    private CellState _state = CellState.Empty;
    public CellState State
    {
        get => _state;
        set => _state = value;   // TODO: OnPropertyChanged() aufrufen
    }

    // TODO: PropertyChanged-Event + OnPropertyChanged-Methode hierher
}
