namespace Server;

public class DijkstraSolver
{
    private GraphData graph;

    public DijkstraSolver(GraphData g) { graph = g; }

    public (List<int> path, double total) Solve(int vonId, int nachId)
    {
        throw new NotImplementedException("Dijkstra: kuerzesten Pfad von vonId nach nachId in graph.Adjazenz finden.");
    }
}
