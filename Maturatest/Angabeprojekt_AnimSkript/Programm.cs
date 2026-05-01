namespace AnimSkript;

public class Programm : Expression
{
    private List<Expression> stmts = new();

    public override void Parse(List<Token> tokens)
    {
        throw new NotImplementedException("Programm.Parse(): alle Anweisungen aus tokens lesen.");
    }

    public override void Run(Zeichner z)
    {
        foreach (var s in stmts)
            s.Run(z);
    }
}
