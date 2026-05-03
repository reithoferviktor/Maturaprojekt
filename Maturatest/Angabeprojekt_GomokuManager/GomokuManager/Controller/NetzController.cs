using System.Net.Sockets;
using System.Windows;
using GomokuManager.Models;
using NetworkLib;

namespace GomokuManager.Controller;

// ============================================================
// TODO 3 — Netzwerk NetzController
//
// Aufgabe: Implementiere den Netzwerk-Controller.
//   1. Konstruktor: TcpClient auf host:port erstellen,
//      Transfer instanziieren und Start() aufrufen.
//   2. Clicked(): eigenen Stein (Black) setzen +
//      MSG { Type=Move, X=cell.X, Y=cell.Y } senden.
//   3. ReceiveMessage(): gegnerischen Stein (White) setzen.
//      Achtung: Dispatcher.Invoke() weil fremder Thread!
//   4. TransferDisconnected(): Statusmeldung anzeigen.
// ============================================================
public class NetzController : AbstractController, Receiver
{
    private Field board = new Field(15);
    private Transfer? transfer;

    public override Field Gameboard => board;

    public NetzController(string host, int port)
    {
        // TODO: TcpClient + Transfer + Start()
        throw new NotImplementedException();
    }

    public override void Clicked(Cell cell)
    {
        // TODO: Stein setzen + MSG senden
        throw new NotImplementedException();
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        // TODO: Dispatcher.Invoke → gegnerischen Stein setzen
        throw new NotImplementedException();
    }

    public void TransferDisconnected(Transfer t)
    {
        Application.Current.Dispatcher.Invoke(() =>
            MessageBox.Show("Verbindung getrennt."));
    }

    public override bool CheckWin(CellState color) => false;
}
