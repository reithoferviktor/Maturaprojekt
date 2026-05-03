using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Client.DataModel;
using NetworkLib;

namespace Client;

public partial class MainWindow : Window, Receiver
{
    private const string Host = "127.0.0.1";
    private const int Port = 5050;
    private const string DbDatei = "routen.db";

    private Transfer? transfer;

    public List<StationDto> Stationen { get; set; } = new();
    public List<EdgeDto> Verbindungen { get; set; } = new();

    private List<int> letzterPfad = new();
    private string letzterTyp = "";
    private double letzteDistanz = 0;

    public MainWindow()
    {
        InitializeComponent();
        LadeGespeicherte();
    }

    private string DbPfad => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbDatei);

    public void Redraw()
    {
        canvas.Children.Clear();

        foreach (var v in Verbindungen)
        {
            var a = Stationen.FirstOrDefault(s => s.Id == v.Von);
            var b = Stationen.FirstOrDefault(s => s.Id == v.Nach);
            if (a == null || b == null) continue;

            canvas.Children.Add(new Line
            {
                X1 = a.X, Y1 = a.Y, X2 = b.X, Y2 = b.Y,
                Stroke = Brushes.LightGray,
                StrokeThickness = 2
            });
        }

        foreach (var s in Stationen)
        {
            var dot = new Ellipse { Width = 14, Height = 14, Fill = Brushes.SteelBlue };
            Canvas.SetLeft(dot, s.X - 7);
            Canvas.SetTop(dot, s.Y - 7);
            canvas.Children.Add(dot);

            var lbl = new TextBlock { Text = s.Name, FontSize = 11, Foreground = Brushes.Black };
            Canvas.SetLeft(lbl, s.X + 8);
            Canvas.SetTop(lbl, s.Y - 16);
            canvas.Children.Add(lbl);
        }
    }

    public void ZeichnePfad(List<int> ids, Brush farbe, bool nummeriert)
    {
        for (int i = 0; i < ids.Count - 1; i++)
        {
            var a = Stationen.FirstOrDefault(s => s.Id == ids[i]);
            var b = Stationen.FirstOrDefault(s => s.Id == ids[i + 1]);
            if (a == null || b == null) continue;

            canvas.Children.Add(new Line
            {
                X1 = a.X, Y1 = a.Y, X2 = b.X, Y2 = b.Y,
                Stroke = farbe,
                StrokeThickness = 4
            });
        }

        if (nummeriert)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                var s = Stationen.FirstOrDefault(st => st.Id == ids[i]);
                if (s == null) continue;

                var num = new TextBlock
                {
                    Text = (i + 1).ToString(),
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Foreground = Brushes.White,
                    Background = farbe,
                    Padding = new Thickness(4, 0, 4, 0)
                };
                Canvas.SetLeft(num, s.X - 14);
                Canvas.SetTop(num, s.Y + 8);
                canvas.Children.Add(num);

            }
        }
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        
        Dispatcher.Invoke(() =>
        {
            try { HandleMessage(m); }
            catch (Exception ex) { lblStatus.Text = "Empfang: " + ex.Message; }
        });
    }

    public void TransferDisconnected(Transfer t)
    {
        Dispatcher.Invoke(() => lblStatus.Text = "Verbindung getrennt.");
    }

    private void HandleMessage(MSG m)
    {
        // TODO Aufgabe 2.2 / 2.3 / 2.4:
        // Auf MsgType.InitAnswer reagieren -> Stationen/Verbindungen uebernehmen + Redraw + ComboBoxen / lbTour befuellen.
        // Auf MsgType.PathAnswer reagieren -> Pfad rot zeichnen (ZeichnePfad).
        // Auf MsgType.TourAnswer reagieren -> Tour blau zeichnen (ZeichnePfad mit Nummerierung).
        // Auf MsgType.Error reagieren -> Fehlertext im Statustext anzeigen.
        if (m.Type == MsgType.InitAnswer)
        {
                lblStatus.Text = "Verbindung getrennt.";
                cbNach.Items.Clear();
                cbVon.Items.Clear();
                Stationen = m.Stationen;
                Verbindungen = m.Verbindungen;
                foreach (var item in m.Stationen)
                {
                    cbVon.Items.Add(item);
                    cbNach.Items.Add(item);
                    lbTour.Items.Add(item);
                }
                Redraw();
        }
        else if(m.Type == MsgType.PathAnswer)
        {
            ZeichnePfad(m.PathIds, Brushes.Red, false);
            lblStatus.Text = m.Distanz.ToString();

        }
        else if (m.Type == MsgType.TourAnswer)
        {
            ZeichnePfad(m.PathIds, Brushes.Blue, true);
        }
    }

    private void btnVerbinden_Click(object sender, RoutedEventArgs e)
    {
        // TODO Aufgabe 2.1: TcpClient zu Host:Port oeffnen,
        // Transfer instanziieren, Start() aufrufen,
        // MSG mit Type=InitRequest senden, lblStatus aktualisieren.
        transfer = new Transfer(new TcpClient(Host, 5050), this);
        transfer.Start();
        transfer.Send(new MSG { Type = MsgType.InitRequest });
    }

    private void btnPfad_Click(object sender, RoutedEventArgs e)
    {
        // TODO Aufgabe 2.3 (Client-Teil): MSG mit PathRequest, FromId/ToId aus den ComboBoxen senden.
        StationDto von = (StationDto)cbVon.SelectedItem;
        StationDto nach = (StationDto)cbNach.SelectedItem;

        transfer.Send(new MSG { FromId = von.Id, ToId = nach.Id, Type = MsgType.PathRequest });
    }

    private void btnTour_Click(object sender, RoutedEventArgs e)
    {
        // TODO Aufgabe 2.4 (Client-Teil): SelectedItems aus lbTour (max 6) zu Liste<int>,
        // MSG mit TourRequest senden.
        if (lbTour.SelectedItems.Count > 6)
        {
            MessageBox.Show("Maximal 6 Städte für eine Tour auswählen!");
        }
        else
        {
            List<int> SelectedIds = new List<int>();
            foreach (var item in lbTour.SelectedItems)
            {
                StationDto stat = (StationDto)item;
                SelectedIds.Add(stat.Id);
            }
            transfer.Send(new MSG {Type = MsgType.TourRequest, SelectedIds = SelectedIds});
            
        }
    }

    private void miSpeichern_Click(object sender, RoutedEventArgs e)
    {
        // TODO Aufgabe 2.5: SaveDialog oeffnen, Name pruefen, Eintrag via linq2db in RouteEintraege schreiben,
        // lbGespeichert aktualisieren.
        throw new NotImplementedException("Speichern: aktuellen Pfad/Tour in routen.db ablegen.");
    }

    private void miLaden_Click(object sender, RoutedEventArgs e)
    {
        // TODO Aufgabe 2.6: LoadDialog oeffnen, gewaehlten Eintrag aus routen.db lesen,
        // Pfad/Tour rekonstruieren und ZeichnePfad aufrufen.
        throw new NotImplementedException("Laden: gespeicherten Eintrag aus routen.db zeichnen.");
    }

    private void LadeGespeicherte()
    {
        try
        {
            using var db = new RoutenDb(DbPfad);
            lbGespeichert.Items.Clear();
            foreach (var r in db.RouteEintraege.OrderBy(r => r.Name))
                lbGespeichert.Items.Add($"{r.Name} ({r.Typ}, {r.Distanz:0})");
        }
        catch (Exception ex)
        {
            lblStatus.Text = "DB-Fehler: " + ex.Message;
        }
    }

    public void MerkeAktuell(string typ, List<int> ids, double distanz)
    {
        letzterTyp = typ;
        letzterPfad = new List<int>(ids);
        letzteDistanz = distanz;
    }

    public string AktuellerTyp => letzterTyp;
    public List<int> AktuellerPfad => letzterPfad;
    public double AktuelleDistanz => letzteDistanz;
}
