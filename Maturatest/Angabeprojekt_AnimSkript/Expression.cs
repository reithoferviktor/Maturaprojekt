namespace AnimSkript;

public abstract class Expression
{
    public static List<string> Errors { get; set; } = new();
    public abstract void Parse(List<Token> tokens);
    public abstract void Run(Zeichner z);
}
