namespace AnimSkript;

public class Token
{
    public enum TokenType
    {
        Position, Bewege, Farbe, Dicke,
        Wiederhole, Mal, Wenn, Dann, Color,
        PosX, PosY, Schritt,
        Lkl, Rkl,
        Number,
        Less, Greater, Equal,
        Error
    }

    public TokenType Type;
    public string Value = "";
}
