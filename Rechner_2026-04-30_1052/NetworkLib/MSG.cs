using System.Runtime.InteropServices.JavaScript;

public enum Type { Request, Answer, Error};
public class Err
{
    public int index;
    public string error;
    public override string ToString()
    {
        return "Invalid charakter: " + error + " at charakter: " + index.ToString();
    }
}
public class MSG
{
    public double? ergebnis;
    public Type Type = Type.Error;
    public List<Err>? Errors;
    public string? expression;

}