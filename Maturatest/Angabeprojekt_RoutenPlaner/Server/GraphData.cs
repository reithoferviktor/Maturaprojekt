using NetworkLib;

namespace Server;

public class Edge
{
    public int Von;
    public int Nach;
    public double Distanz;
}

public class GraphData
{
    public List<StationDto> Stationen { get; } = new();
    public List<Edge> Verbindungen { get; } = new();
    public Dictionary<int, List<(int Nach, double Dist)>> Adjazenz { get; } = new();

    public GraphData()
    {
        Seed();
        BuildAdjacency();
    }

    private void Seed()
    {
        AddS(1, "Hauptbahnhof", 540, 380);
        AddS(2, "Stadtmitte",   460, 320);
        AddS(3, "Westpark",     320, 260);
        AddS(4, "Nordmarkt",    420, 140);
        AddS(5, "Ostend",       640, 200);
        AddS(6, "Suedhafen",    520, 540);
        AddS(7, "Universitaet", 280, 420);
        AddS(8, "Flughafen",    700, 480);
        AddS(9, "Messezentrum", 380, 480);
        AddS(10, "Botanik",     220, 180);
        AddS(11, "Altstadt",    480, 240);
        AddS(12, "Stadion",     620, 360);

        AddE(1, 2, 80);
        AddE(2, 11, 80);
        AddE(11, 4, 110);
        AddE(2, 3, 150);
        AddE(3, 10, 120);
        AddE(3, 7, 170);
        AddE(7, 9, 110);
        AddE(9, 6, 160);
        AddE(6, 8, 200);
        AddE(8, 12, 140);
        AddE(12, 5, 170);
        AddE(5, 4, 230);
        AddE(4, 11, 110);
        AddE(11, 5, 240);
        AddE(1, 12, 90);
        AddE(1, 6, 170);
        AddE(2, 4, 210);
    }

    private void AddS(int id, string name, double x, double y)
        => Stationen.Add(new StationDto { Id = id, Name = name, X = x, Y = y });

    private void AddE(int v, int n, double d)
        => Verbindungen.Add(new Edge { Von = v, Nach = n, Distanz = d });

    private void BuildAdjacency()
    {
        foreach (var s in Stationen)
            Adjazenz[s.Id] = new();

        foreach (var e in Verbindungen)
        {
            Adjazenz[e.Von].Add((e.Nach, e.Distanz));
            Adjazenz[e.Nach].Add((e.Von, e.Distanz));
        }
    }
}
