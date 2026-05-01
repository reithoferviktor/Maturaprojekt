namespace Server;

public class PermutationenSolver
{
    private GraphData graph;
    private DijkstraSolver dijkstra;

    public PermutationenSolver(GraphData g)
    {
        graph = g;
        dijkstra = new DijkstraSolver(g);
    }

    public (List<int> ordnung, double total) Solve(List<int> ausgewaehlteIds)
    {
        throw new NotImplementedException("Permutationen: alle Reihenfolgen mit fixiertem Startpunkt durchprobieren und beste Tour zurueckgeben.");
    }

    public IEnumerable<List<int>> Permutiere(List<int> rest)
    {
        throw new NotImplementedException("Eigener Permutationsgenerator (rekursiv oder iterativ).");
    }
}
