namespace NetworkLib;

public enum MsgType { None, Move, GameOver }

public class MSG
{
    public MsgType Type = MsgType.None;
    public int X;
    public int Y;
    public string? Gewinner;
}
