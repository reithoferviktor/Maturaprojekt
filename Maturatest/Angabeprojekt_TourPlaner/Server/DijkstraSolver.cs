namespace Server;

public class DijkstraSolver
{
    private GraphData graph;

    public DijkstraSolver(GraphData g) { graph = g; }

    public (List<int> path, double total) Solve(int fromId, int toId)
    {
        throw new NotImplementedException("Dijkstra: kuerzesten Pfad von fromId nach toId berechnen.");
    }
}
