namespace Server;
using MoreLinq;

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
        int start = ausgewaehlteIds[0];
        ausgewaehlteIds.RemoveAt(0);
        double best = double.PositiveInfinity;
        List<int> bestfind = new List<int>();
        foreach (var item in ausgewaehlteIds.Permutations())
        {
           List<int> ints = new List<int>();
            ints.Add(start);
            foreach (var item1 in item)
            {
                ints.Add(item1);
            }
            double curr = 0;
            for (int i = 0; i < ints.Count-1; i++)
            {
                List<int> path;
                double curry;
                (path, curry) = dijkstra.Solve(ints[i], ints[i+1]);
                curr += curry;
            }
            if (best > curr)
            {
                bestfind = ints;
                best = curr;
            }
        }
        return (bestfind, best);
    }

    public IEnumerable<List<int>> Permutiere(List<int> rest)
    {
        throw new NotImplementedException("Eigener Permutationsgenerator (rekursiv oder iterativ).");
    }
}
