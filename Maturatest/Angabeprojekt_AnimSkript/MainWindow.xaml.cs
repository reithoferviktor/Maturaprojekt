using System.Windows;

namespace AnimSkript;

public partial class MainWindow : Window
{
    private Zeichner zeichner;

    public MainWindow()
    {
        InitializeComponent();
        zeichner = new Zeichner(canvas);
        txtCode.Text = "POSITION 100 100\nFARBE Blau\nDICKE 2\n\nWIEDERHOLE 4 MAL {\n    BEWEGE 60 0\n    BEWEGE 0 60\n    BEWEGE -60 0\n    BEWEGE 0 -60\n}\n";
    }

    private void btnRun_Click(object sender, RoutedEventArgs e)
    {
        lbErrors.Items.Clear();
        Expression.Errors.Clear();
        zeichner.Reset();

        try
        {
            List<Token> tokens = Tokenize(txtCode.Text);

            foreach (var t in tokens)
                if (t.Type == Token.TokenType.Error)
                    lbErrors.Items.Add("[Tokenizer] Unbekannt: " + t.Value);

            var prog = new Programm();
            prog.Parse(tokens);
            prog.Run(zeichner);
        }
        catch (Exception ex)
        {
            lbErrors.Items.Add("[Run] " + ex.Message);
        }

        foreach (var err in Expression.Errors)
            lbErrors.Items.Add(err);
    }

    private List<Token> Tokenize(string code)
    {
        // TODO Aufgabe 1.1: code in Tokens zerlegen und zurueckgeben.
        // Bei unbekannten Zeichenketten einen Token mit Type=Error in die Liste eintragen
        // statt eine Exception zu werfen.
        throw new NotImplementedException("Tokenizer noch nicht implementiert.");
    }
}
