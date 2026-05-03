using System.Text.RegularExpressions;
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
        Regex RKL = new Regex(@"\}");
        Regex LKL = new Regex(@"\{");
        Regex Position = new Regex(@"POSITION");
        Regex Number = new Regex(@"\-?\d+");
        Regex Color = new Regex(@"Rot|Blau|Gruen");
        Regex Dicke = new Regex(@"DICKE");
        Regex Farbe = new Regex(@"FARBE");
        Regex Wiederhole = new Regex(@"WIEDERHOLE");
        Regex Mal = new Regex(@"MAL");
        Regex Wenn = new Regex(@"WENN");
        Regex posx = new Regex(@"POSX");
        Regex posy = new Regex(@"POSY");
        Regex kl = new Regex(@"\<");
        Regex gr = new Regex(@"\>");
        Regex sm = new Regex(@"\=");
        Regex Dann = new Regex(@"DANN");
        Regex MOVE = new Regex(@"BEWEGE");
        Regex GlobalRegex = new Regex(@"BEWEGE|DANN|\=|\>|\<|POSY|POSX|WENN|MAL|WIEDERHOLE|FARBE|DICKE|Rot|Blau|Gruen|\-?\d+|POSITION|\{|\}|.");
        MatchCollection mc  = GlobalRegex.Matches(code);
        List<Token> tokens = new List<Token>();
        foreach (Match m in mc)
        {
            if (m.Success)
            {
                Token token = new Token();
                if (LKL.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Lkl;
                    token.Value = m.Value;
                }
                else if (RKL.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Rkl;
                    token.Value = m.Value;
                }
                else if (Position.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Position;
                    token.Value = m.Value;
                }
                else if (Number.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Number;
                    token.Value = m.Value;
                }
                else if (Color.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Color;
                    token.Value = m.Value;
                }
                else if (Dicke.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Dicke;
                    token.Value = m.Value;
                }
                else if (Farbe.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Farbe;
                    token.Value = m.Value;
                }
                else if (Wiederhole.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Wiederhole;
                    token.Value = m.Value;
                }
                else if (Mal.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Mal;
                    token.Value = m.Value;
                }
                else if (Wenn.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Wenn;
                    token.Value = m.Value;
                }
                else if (posx.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.PosX;
                    token.Value = m.Value;
                }
                else if (posy.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.PosY;
                    token.Value = m.Value;
                }
                else if (kl.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Less;
                    token.Value = m.Value;
                }
                else if (gr.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Greater;
                    token.Value = m.Value;
                }
                else if (sm.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Equal;
                    token.Value = m.Value;
                }
                else if (Dann.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Dann;
                    token.Value = m.Value;
                }
                else if (MOVE.IsMatch(m.Value))
                {
                    token.Type = Token.TokenType.Bewege;
                    token.Value = m.Value;
                }
                else {
                    if (m.Value != " " && m.Value != "\n")
                    {
                        token.Type = Token.TokenType.Error;
                        token.Value = m.Value;
                    }
                }
                if (token.Value != "")
                {
                    tokens.Add(token);
                }
            }
        }

        return tokens;


    }
}
