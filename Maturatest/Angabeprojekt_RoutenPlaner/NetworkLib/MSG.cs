namespace NetworkLib;

public enum MsgType
{
    None,
    InitRequest,
    InitAnswer,
    PathRequest,
    PathAnswer,
    TourRequest,
    TourAnswer,
    Error
}

public class StationDto
{
    public int Id;
    public string Name = "";
    public double X;
    public double Y;
}

public class EdgeDto
{
    public int Von;
    public int Nach;
    public double Distanz;
}

public class MSG
{
    public MsgType Type = MsgType.None;

    public int FromId;
    public int ToId;
    public List<int>? SelectedIds;

    public List<StationDto>? Stationen;
    public List<EdgeDto>? Verbindungen;
    public List<int>? PathIds;
    public double Distanz;

    public string? Error;
}
