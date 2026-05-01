using NetworkLib;

namespace Server;

public class KMeansSolver
{
    private GraphData graph;

    public KMeansSolver(GraphData g) { graph = g; }

    public List<ClusterAssignment> Solve(int k)
    {
        throw new NotImplementedException("KMeans: alle POIs anhand X/Y in k Cluster gruppieren.");
    }
}
