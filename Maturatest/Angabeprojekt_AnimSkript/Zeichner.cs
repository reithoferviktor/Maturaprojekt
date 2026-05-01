using System.Windows.Controls;
using System.Windows.Shapes;

namespace AnimSkript;

public class Zeichner
{
    private Canvas canvas;
    public Stiftzustand Stift { get; } = new();

    public Zeichner(Canvas c) { canvas = c; }

    public void Reset()
    {
        canvas.Children.Clear();
        Stift.X = 0;
        Stift.Y = 0;
        Stift.Schritt = 0;
        Stift.Dicke = 1;
        Stift.Farbe = System.Windows.Media.Brushes.Black;
    }

    public void Setze(double x, double y)
    {
        Stift.X = x;
        Stift.Y = y;
    }

    public void Bewege(double dx, double dy)
    {
        double x1 = Stift.X;
        double y1 = Stift.Y;
        Stift.X += dx;
        Stift.Y += dy;
        Stift.Schritt++;

        canvas.Children.Add(new Line
        {
            X1 = x1, Y1 = y1,
            X2 = Stift.X, Y2 = Stift.Y,
            Stroke = Stift.Farbe,
            StrokeThickness = Stift.Dicke
        });
    }
}
