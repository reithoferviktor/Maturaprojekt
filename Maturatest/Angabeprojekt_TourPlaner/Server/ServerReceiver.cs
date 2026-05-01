using NetworkLib;

namespace Server;

public class ServerReceiver : Receiver
{
    private GraphData graph;
    private DijkstraSolver dijkstra;
    private KMeansSolver kmeans;
    private PermutationsTourSolver tour;

    public ServerReceiver(GraphData g)
    {
        graph = g;
        dijkstra = new DijkstraSolver(g);
        kmeans = new KMeansSolver(g);
        tour = new PermutationsTourSolver(g);
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        Console.WriteLine($"[{t.GetIP()}] empfangen: {m.Type}");

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
                case MsgType.ClusterRequest:
                    HandleCluster(m, t);
                    break;
                case MsgType.TourRequest:
                    HandleTour(m, t);
                    break;
                default:
                    SendError(t, $"Unbekannter Anfragetyp: {m.Type}");
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
        var edges = graph.Edges.Select(e => new EdgeInfo
        {
            From = e.From,
            To = e.To,
            Distance = e.Distance
        }).ToList();

        t.Send(new MSG
        {
            Type = MsgType.InitAnswer,
            Pois = graph.Pois.ToList(),
            Edges = edges
        });
    }

    private void HandlePath(MSG m, Transfer t)
    {
        var (path, total) = dijkstra.Solve(m.FromId, m.ToId);
        t.Send(new MSG
        {
            Type = MsgType.PathAnswer,
            PathIds = path,
            Distance = total
        });
    }

    private void HandleCluster(MSG m, Transfer t)
    {
        var clusters = kmeans.Solve(m.K);
        t.Send(new MSG
        {
            Type = MsgType.ClusterAnswer,
            Clusters = clusters
        });
    }

    private void HandleTour(MSG m, Transfer t)
    {
        if (m.SelectedIds == null || m.SelectedIds.Count < 2)
        {
            SendError(t, "Mindestens zwei POIs auswaehlen.");
            return;
        }
        var (order, total) = tour.Solve(m.SelectedIds);
        t.Send(new MSG
        {
            Type = MsgType.TourAnswer,
            OrderedIds = order,
            Distance = total
        });
    }

    private void SendError(Transfer t, string msg)
    {
        Console.WriteLine("  -> Fehler: " + msg);
        t.Send(new MSG { Type = MsgType.Error, Error = msg });
    }

    public void TransferDisconnected(Transfer t)
    {
        Console.WriteLine($"[{t.GetIP()}] getrennt");
    }
}
