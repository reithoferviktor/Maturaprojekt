public enum Type { Request, Answer, None };

public class City
{
    public double lat;
    public double lon;
    public string name;
    public override string ToString()
    {
        return name;
    }
}
public class MSG
{

    public Type type = Type.None;
    public List<City>? city;
    public string? search;
}