using System.Globalization;
using NetworkLib;

namespace Server;

public class Edge
{
    public int From;
    public int To;
    public double Distance;
}

public class GraphData
{
    public List<POI> Pois { get; } = new();
    public List<Edge> Edges { get; } = new();

    public Dictionary<int, List<(int To, double Dist)>> Adjacency { get; } = new();

    public void Load(string poisCsv, string edgesCsv)
    {
        Pois.Clear();
        Edges.Clear();
        Adjacency.Clear();

        foreach (var line in File.ReadAllLines(poisCsv).Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var p = line.Split(';');
            Pois.Add(new POI
            {
                Id = int.Parse(p[0]),
                Name = p[1],
                Kategorie = p[2],
                X = double.Parse(p[3], CultureInfo.InvariantCulture),
                Y = double.Parse(p[4], CultureInfo.InvariantCulture)
            });
        }

        foreach (var line in File.ReadAllLines(edgesCsv).Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var p = line.Split(';');
            int a = int.Parse(p[0]);
            int b = int.Parse(p[1]);
            double d = double.Parse(p[2], CultureInfo.InvariantCulture);
            Edges.Add(new Edge { From = a, To = b, Distance = d });
        }

        foreach (var poi in Pois)
            Adjacency[poi.Id] = new();

        foreach (var e in Edges)
        {
            if (!Adjacency.ContainsKey(e.From) || !Adjacency.ContainsKey(e.To)) continue;
            Adjacency[e.From].Add((e.To, e.Distance));
            Adjacency[e.To].Add((e.From, e.Distance));
        }
    }

    public POI? FindPoi(int id) => Pois.FirstOrDefault(p => p.Id == id);
}
