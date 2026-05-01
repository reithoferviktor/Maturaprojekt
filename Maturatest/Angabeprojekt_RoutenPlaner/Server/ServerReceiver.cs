using NetworkLib;

namespace Server;

public class ServerReceiver : Receiver
{
    private GraphData graph;
    private DijkstraSolver dijkstra;
    private PermutationenSolver permutationen;

    public ServerReceiver(GraphData g)
    {
        graph = g;
        dijkstra = new DijkstraSolver(g);
        permutationen = new PermutationenSolver(g);
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        Console.WriteLine($"[{t.GetIP()}] {m.Type}");

        try
        {
            switch (m.Type)
            {
                case MsgType.InitRequest:
                    HandleInit(t);
                    break;
                case MsgType.PathRequest:
                    HandlePath(m, t);
                    break;
                case MsgType.TourRequest:
                    HandleTour(m, t);
                    break;
                default:
                    SendError(t, "Unbekannter Anfragetyp: " + m.Type);
                    break;
            }
        }
        catch (NotImplementedException nie)
        {
            SendError(t, "TODO: " + nie.Message);
        }
        catch (Exception ex)
        {
            SendError(t, "Fehler: " + ex.Message);
        }
    }

    private void HandleInit(Transfer t)
    {
        var verb = graph.Verbindungen.Select(e => new EdgeDto
        {
            Von = e.Von,
            Nach = e.Nach,
            Distanz = e.Distanz
        }).ToList();

        t.Send(new MSG
        {
            Type = MsgType.InitAnswer,
            Stationen = graph.Stationen.ToList(),
            Verbindungen = verb
        });
    }

    private void HandlePath(MSG m, Transfer t)
    {
        var (path, total) = dijkstra.Solve(m.FromId, m.ToId);
        t.Send(new MSG
        {
            Type = MsgType.PathAnswer,
            PathIds = path,
            Distanz = total
        });
    }

    private void HandleTour(MSG m, Transfer t)
    {
        if (m.SelectedIds == null || m.SelectedIds.Count < 2)
        {
            SendError(t, "Mindestens 2 Stationen waehlen.");
            return;
        }
        var (ordnung, total) = permutationen.Solve(m.SelectedIds);
        t.Send(new MSG
        {
            Type = MsgType.TourAnswer,
            PathIds = ordnung,
            Distanz = total
        });
    }

    private void SendError(Transfer t, string text)
    {
        Console.WriteLine("  Fehler: " + text);
        t.Send(new MSG { Type = MsgType.Error, Error = text });
    }

    public void TransferDisconnected(Transfer t)
    {
        Console.WriteLine($"[{t.GetIP()}] getrennt");
    }
}
