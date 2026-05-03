using NetworkLib;

namespace Server;

public class DijkstraSolver
{
    private GraphData graph;

    public DijkstraSolver(GraphData g) { graph = g; }

    public (List<int> path, double total) Solve(int vonId, int nachId)
    {
        graph = new GraphData();
        Dictionary<int, double> Dist = new();
        Dictionary<int, int?> Prev = new();
        PriorityQueue<int, double> queue = new PriorityQueue<int, double>();
        foreach (var item in graph.Stationen)
        {
            if (item.Id == vonId)
            {
                Dist[item.Id] = 0;
                queue.Enqueue(item.Id, 0);
            }
            else
            {
                Dist[item.Id] = double.PositiveInfinity;

            }
            Prev[item.Id] = null;
        }
        while (queue.Count > 0)
        {
            int q = queue.Dequeue();
            foreach (var item in graph.Adjazenz[q])
            {
                double alternativ = item.Dist + Dist[q];
                if (alternativ < Dist[item.Nach])
                {
                    Dist[item.Nach] = alternativ;
                    Prev[item.Nach] = q;
                    queue.Enqueue(item.Nach, alternativ);
                }
            }
        }
        List<int> path = new List<int>();
        double total = Dist[nachId];
        int curr = nachId;
        path.Add(curr);
        while (Prev[curr] != null)
        {
            curr = (int)Prev[curr];

            path.Add(curr);
        }
        path.Reverse();
        return (path, total);


    }
}
