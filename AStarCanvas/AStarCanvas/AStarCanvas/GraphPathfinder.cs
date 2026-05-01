namespace AStarCanvas;

/// <summary>
/// A*-Pathfinder für einen Graphen mit Canvas-Koordinaten.
///
/// ╔══════════════════════════════════════════════════════════════╗
/// ║  AUFGABE: Implementiere die drei markierten Methoden.       ║
/// ║  Alle anderen Methoden und Klassen sind bereits fertig.     ║
/// ╚══════════════════════════════════════════════════════════════╝
/// </summary>
public class GraphPathfinder
{
    private readonly List<Node> _nodes;
    private readonly List<Edge> _edges;

    // Nachschlagetabelle: Node-Id → alle adjazenten (Nachbar, Kantengewicht)
    private readonly Dictionary<int, List<(Node neighbor, double cost)>> _adjacency;

    public GraphPathfinder(List<Node> nodes, List<Edge> edges)
    {
        _nodes = nodes;
        _edges = edges;
        _adjacency = BuildAdjacency(nodes, edges);
    }


    private static double Heuristic(Node a, Node b)
    {
        return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
    }





    public List<Node>? FindPath(Node start, Node end)
    {
        Dictionary<Node, double> Dist = new();
        Dictionary<Node, Node> prev = new();
        PriorityQueue<Node, double> Q = new PriorityQueue<Node, double>();
        foreach (var item in _nodes)
        {
            if (item.Id == start.Id)
            {
                Dist[item] = 0;
                prev[item] = null;
                Q.Enqueue(item, 0);
            }
            else
            {
                Dist[item] = double.PositiveInfinity;
                prev[item] = null;
            }
        }
        while (Q.Count > 0)
        {
            Node q = Q.Dequeue();
            foreach (var item in _adjacency[q.Id])
            {
                double alt = Dist[q] + item.cost;
                if (alt < Dist[item.neighbor])
                {
                    Dist[item.neighbor] = alt;
                    prev[item.neighbor] = q;
                    Q.Enqueue(item.neighbor, alt + Heuristic(item.neighbor, end));
                }
            }
        }
        if (double.IsPositiveInfinity(Dist[end]))
            return null;

        List<Node> weg = new List<Node>();
        Node curr = end;

        while (prev[curr] != null)
        {
            weg.Insert(0, curr);

            curr = prev[curr];
        }
        weg.Insert(0, start);
        return weg;
    }

    // =========================================================
    //  AB HIER: BEREITS GEGEBEN — nicht ändern
    // =========================================================

    /// <summary>
    /// Rekonstruiert den Pfad vom Ziel zurück zum Start.
    /// Bereits fertig – einfach in FindPath aufrufen.
    /// </summary>
    protected static List<Node> ReconstructPath(
        Dictionary<int, Node> cameFrom, Node start, Node end)
    {
        var path    = new List<Node>();
        var current = end;

        while (current.Id != start.Id)
        {
            path.Add(current);
            current = cameFrom[current.Id];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Baut aus der Kantenliste eine beidseitige Adjazenzliste auf.
    /// Jede Edge wird in beide Richtungen eingetragen (ungerichteter Graph).
    /// </summary>
    private static Dictionary<int, List<(Node, double)>> BuildAdjacency(
        List<Node> nodes, List<Edge> edges)
    {
        var adj = nodes.ToDictionary(n => n.Id, _ => new List<(Node, double)>());

        foreach (var e in edges)
        {
            adj[e.From.Id].Add((e.To,   e.Weight));
            adj[e.To.Id  ].Add((e.From, e.Weight));
        }

        return adj;
    }
}
