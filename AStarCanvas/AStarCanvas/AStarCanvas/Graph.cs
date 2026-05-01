namespace AStarCanvas;

/// <summary>
/// Repräsentiert einen Knoten im Graphen mit einer Position auf dem Canvas.
/// </summary>
public class Node
{
    public int    Id   { get; init; }
    public string Name { get; init; } = "";
    public double X    { get; init; }   // Canvas-Koordinate
    public double Y    { get; init; }   // Canvas-Koordinate

    public override string ToString() => $"{Name} (#{Id})";
}

/// <summary>
/// Repräsentiert eine ungerichtete Kante zwischen zwei Knoten.
/// Das Gewicht wird automatisch als euklidische Distanz berechnet,
/// kann aber auch manuell gesetzt werden.
/// </summary>
public class Edge
{
    public Node   From   { get; init; } = null!;
    public Node   To     { get; init; } = null!;
    public double Weight { get; init; }

    /// <summary>
    /// Erstellt eine Kante – Gewicht = euklidische Distanz der Knoten-Positionen.
    /// </summary>
    public static Edge Euclidean(Node from, Node to)
    {
        double dx = from.X - to.X;
        double dy = from.Y - to.Y;
        return new Edge { From = from, To = to, Weight = Math.Sqrt(dx * dx + dy * dy) };
    }
}
