namespace Server;

public class PermutationsTourSolver
{
    private GraphData graph;
    private DijkstraSolver dijkstra;

    public PermutationsTourSolver(GraphData g)
    {
        graph = g;
        dijkstra = new DijkstraSolver(g);
    }

    public (List<int> order, double total) Solve(List<int> selectedIds)
    {
        throw new NotImplementedException("Permutationen: alle Reihenfolgen mit fixiertem Startpunkt durchprobieren und beste Tour zurueckgeben.");
    }

    public IEnumerable<List<int>> Permute(List<int> rest)
    {
        throw new NotImplementedException("Eigener Permutationsgenerator (rekursiv oder iterativ).");
    }
}
