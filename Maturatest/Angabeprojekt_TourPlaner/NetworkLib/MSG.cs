namespace NetworkLib;

public enum MsgType
{
    None,
    InitRequest,
    InitAnswer,
    PathRequest,
    PathAnswer,
    ClusterRequest,
    ClusterAnswer,
    TourRequest,
    TourAnswer,
    Error
}

public class EdgeInfo
{
    public int From;
    public int To;
    public double Distance;
}

public class POI
{
    public int Id;
    public string Name = "";
    public string Kategorie = "";
    public double X;
    public double Y;
}

public class ClusterAssignment
{
    public int PoiId;
    public int Cluster;
}

public class MSG
{
    public MsgType Type = MsgType.None;

    public int FromId;
    public int ToId;
    public int K;
    public List<int>? SelectedIds;

    public List<POI>? Pois;
    public List<EdgeInfo>? Edges;
    public List<int>? PathIds;
    public double Distance;
    public List<ClusterAssignment>? Clusters;
    public List<int>? OrderedIds;

    public string? Error;
}
