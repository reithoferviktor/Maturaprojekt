namespace AStarCanvas;

/// <summary>
/// Enthält den vordefinierten Graphen (Stadtplan-Szenario).
/// Knoten = Stadtteile / Sehenswürdigkeiten, Kanten = Straßen.
///
/// Koordinaten sind auf eine Canvas-Größe von 800×560 ausgelegt.
/// </summary>
public static class GraphData
{
    public static (List<Node> nodes, List<Edge> edges) Create()
    {
        var nodes = new List<Node>
        {
            new() { Id =  0, Name = "Hauptbahnhof",    X =  90,  Y = 280 },
            new() { Id =  1, Name = "Westpark",        X = 180,  Y = 120 },
            new() { Id =  2, Name = "Marktplatz",      X = 240,  Y = 280 },
            new() { Id =  3, Name = "Universität",     X = 200,  Y = 420 },
            new() { Id =  4, Name = "Nordviertel",     X = 300,  Y =  80 },
            new() { Id =  5, Name = "Stadtmitte",      X = 380,  Y = 220 },
            new() { Id =  6, Name = "Altstadttor",     X = 350,  Y = 360 },
            new() { Id =  7, Name = "Sportplatz",      X = 150,  Y = 490 },
            new() { Id =  8, Name = "Ostbahnhof",      X = 520,  Y = 140 },
            new() { Id =  9, Name = "Technopark",      X = 480,  Y = 320 },
            new() { Id = 10, Name = "Krankenhaus",     X = 440,  Y = 460 },
            new() { Id = 11, Name = "Flughafen",       X = 650,  Y =  80 },
            new() { Id = 12, Name = "Messegelände",    X = 640,  Y = 240 },
            new() { Id = 13, Name = "Südring",         X = 600,  Y = 400 },
            new() { Id = 14, Name = "Hafenviertel",    X = 560,  Y = 500 },
            new() { Id = 15, Name = "Einkaufszentrum", X = 720,  Y = 180 },
            new() { Id = 16, Name = "Botanik",         X = 720,  Y = 340 },
            new() { Id = 17, Name = "Stadtrand Nord",  X = 400,  Y =  40 },
            new() { Id = 18, Name = "Industriezone",   X = 280,  Y = 500 },
            new() { Id = 19, Name = "Endstation",      X = 760,  Y = 460 },
        };

        // Kanten (ungerichtet, Gewicht = Euklidische Distanz)
        var pairs = new (int, int)[]
        {
            ( 0,  1), ( 0,  2), ( 0,  3),
            ( 1,  2), ( 1,  4), ( 1,  5),
            ( 2,  3), ( 2,  5), ( 2,  6),
            ( 3,  6), ( 3,  7), ( 3, 18),
            ( 4,  5), ( 4, 17), ( 4,  8),
            ( 5,  6), ( 5,  8), ( 5,  9),
            ( 6,  9), ( 6, 10),
            ( 7, 18),
            ( 8, 11), ( 8, 12), ( 8,  9),
            ( 9, 10), ( 9, 12),
            (10, 13), (10, 14), (10, 18),
            (11, 15), (11, 17),
            (12, 13), (12, 15),
            (13, 14), (13, 16),
            (14, 18), (14, 19),
            (15, 16),
            (16, 19),
        };

        var nodeById = nodes.ToDictionary(n => n.Id);
        var edges = pairs
            .Select(p => Edge.Euclidean(nodeById[p.Item1], nodeById[p.Item2]))
            .ToList();

        return (nodes, edges);
    }
}
