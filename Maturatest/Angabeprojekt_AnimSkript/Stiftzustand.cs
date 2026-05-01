using System.Windows.Media;

namespace AnimSkript;

public class Stiftzustand
{
    public double X = 0;
    public double Y = 0;
    public Brush Farbe = Brushes.Black;
    public double Dicke = 1;
    public int Schritt = 0;

    public Brush ParseFarbe(string name)
    {
        switch (name)
        {
            case "Schwarz": return Brushes.Black;
            case "Rot":     return Brushes.Red;
            case "Gruen":   return Brushes.Green;
            case "Blau":    return Brushes.Blue;
            case "Gelb":    return Brushes.Gold;
        }
        throw new Exception("Unbekannte Farbe: " + name);
    }
}
