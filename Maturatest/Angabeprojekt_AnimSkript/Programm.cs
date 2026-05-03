namespace AnimSkript;

public class Programm : Expression
{
    private List<Expression> stmts = new();

    public override void Parse(List<Token> tokens)
    {
        while (tokens.Count > 0)
        {
            if (tokens[0].Type == Token.TokenType.Rkl)
            {
                tokens.RemoveAt(0);
                return;
            }
            STMT statement = new STMT();
            statement.Parse(tokens);
            stmts.Add(statement);
        }
    }

    public override void Run(Zeichner z)
    {
        foreach (var s in stmts)
            s.Run(z);
    }
}
