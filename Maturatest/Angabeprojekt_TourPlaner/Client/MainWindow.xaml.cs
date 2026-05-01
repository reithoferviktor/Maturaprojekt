using System.Globalization;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NetworkLib;

namespace Client;

public partial class MainWindow : Window, Receiver
{
    private Transfer? transfer;
    private List<POI> pois = new();
    private List<EdgeInfo> edges = new();

    private static readonly Brush[] ClusterColors =
    {
        Brushes.IndianRed, Brushes.SteelBlue, Brushes.SeaGreen,
        Brushes.DarkOrange, Brushes.MediumPurple, Brushes.Teal,
        Brushes.SaddleBrown, Brushes.HotPink
    };

    private static readonly Dictionary<string, Brush> CategoryColors = new()
    {
        { "Sehenswuerdigkeit", Brushes.SteelBlue },
        { "Park",              Brushes.SeaGreen },
        { "Cafe",              Brushes.SaddleBrown },
        { "Aussicht",          Brushes.DarkOrange },
        { "Markt",             Brushes.MediumPurple },
        { "Knotenpunkt",       Brushes.Gray }
    };

    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnConnect_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var c = new TcpClient(tbHost.Text, int.Parse(tbPort.Text));
            transfer = new Transfer(c, this);
            transfer.Start();
            transfer.Send(new MSG { Type = MsgType.InitRequest });
            tbStatus.Text = "Verbunden, lade Daten ...";
            tbStatus.Foreground = Brushes.Black;
        }
        catch (Exception ex)
        {
            tbStatus.Text = "Verbindung fehlgeschlagen: " + ex.Message;
            tbStatus.Foreground = Brushes.DarkRed;
        }
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        Dispatcher.Invoke(() => Handle(m));
    }

    private void Handle(MSG m)
    {
        switch (m.Type)
        {
            case MsgType.InitAnswer:
                pois = m.Pois ?? new();
                edges = m.Edges ?? new();
                FillUi();
                Redraw();
                tbStatus.Text = $"{pois.Count} POIs, {edges.Count} Verbindungen geladen.";
                break;
            case MsgType.PathAnswer:
                Redraw();
                HighlightPath(m.PathIds, Brushes.Red, 4);
                tbStatus.Text = $"Pfad: {m.PathIds?.Count} POIs, Distanz {m.Distance:F1}";
                break;
            case MsgType.ClusterAnswer:
                Redraw(m.Clusters);
                tbStatus.Text = $"Gruppiert in {m.Clusters?.Select(c => c.Cluster).Distinct().Count()} Cluster.";
                break;
            case MsgType.TourAnswer:
                Redraw();
                HighlightPath(m.OrderedIds, Brushes.RoyalBlue, 3);
                NumberStops(m.OrderedIds);
                tbStatus.Text = $"Tour: {m.OrderedIds?.Count} POIs, Distanz {m.Distance:F1}";
                break;
            case MsgType.Error:
                tbStatus.Text = "Fehler: " + m.Error;
                tbStatus.Foreground = Brushes.DarkRed;
                break;
        }
    }

    private void FillUi()
    {
        cbFrom.Items.Clear();
        cbTo.Items.Clear();
        lbTour.Items.Clear();
        foreach (var p in pois)
        {
            cbFrom.Items.Add(p);
            cbTo.Items.Add(p);
            lbTour.Items.Add(p);
        }
        cbFrom.DisplayMemberPath = "Name";
        cbTo.DisplayMemberPath = "Name";
        lbTour.DisplayMemberPath = "Name";
    }

    private void Redraw(List<ClusterAssignment>? clusters = null)
    {
        canvas.Children.Clear();

        foreach (var e in edges)
        {
            var a = pois.FirstOrDefault(p => p.Id == e.From);
            var b = pois.FirstOrDefault(p => p.Id == e.To);
            if (a == null || b == null) continue;
            canvas.Children.Add(new Line
            {
                X1 = a.X, Y1 = a.Y, X2 = b.X, Y2 = b.Y,
                Stroke = Brushes.LightGray, StrokeThickness = 1
            });
        }

        foreach (var p in pois)
        {
            Brush fill;
            if (clusters != null)
            {
                var c = clusters.FirstOrDefault(x => x.PoiId == p.Id);
                fill = ClusterColors[Math.Abs(c?.Cluster ?? 0) % ClusterColors.Length];
            }
            else
            {
                fill = CategoryColors.TryGetValue(p.Kategorie, out var b) ? b : Brushes.Gray;
            }

            var ell = new Ellipse
            {
                Width = 14, Height = 14,
                Fill = fill, Stroke = Brushes.Black, StrokeThickness = 1
            };
            Canvas.SetLeft(ell, p.X - 7);
            Canvas.SetTop(ell, p.Y - 7);
            canvas.Children.Add(ell);

            var text = new TextBlock
            {
                Text = p.Name,
                FontSize = 11,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(text, p.X + 8);
            Canvas.SetTop(text, p.Y - 6);
            canvas.Children.Add(text);
        }
    }

    private void HighlightPath(List<int>? path, Brush brush, double thick)
    {
        if (path == null || path.Count < 2) return;
        for (int i = 0; i < path.Count - 1; i++)
        {
            var a = pois.FirstOrDefault(p => p.Id == path[i]);
            var b = pois.FirstOrDefault(p => p.Id == path[i + 1]);
            if (a == null || b == null) continue;
            canvas.Children.Add(new Line
            {
                X1 = a.X, Y1 = a.Y, X2 = b.X, Y2 = b.Y,
                Stroke = brush, StrokeThickness = thick
            });
        }
    }

    private void NumberStops(List<int>? order)
    {
        if (order == null) return;
        for (int i = 0; i < order.Count; i++)
        {
            var p = pois.FirstOrDefault(x => x.Id == order[i]);
            if (p == null) continue;
            var label = new TextBlock
            {
                Text = (i + 1).ToString(),
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = Brushes.White,
                Background = Brushes.RoyalBlue,
                Padding = new Thickness(4, 0, 4, 0)
            };
            Canvas.SetLeft(label, p.X - 18);
            Canvas.SetTop(label, p.Y - 8);
            canvas.Children.Add(label);
        }
    }

    private void btnPath_Click(object sender, RoutedEventArgs e)
    {
        if (transfer == null) { tbStatus.Text = "Nicht verbunden."; return; }
        if (cbFrom.SelectedItem is not POI from || cbTo.SelectedItem is not POI to) return;
        transfer.Send(new MSG { Type = MsgType.PathRequest, FromId = from.Id, ToId = to.Id });
    }

    private void btnCluster_Click(object sender, RoutedEventArgs e)
    {
        if (transfer == null) { tbStatus.Text = "Nicht verbunden."; return; }
        if (!int.TryParse(tbK.Text, out int k) || k <= 0) { tbStatus.Text = "k ungueltig."; return; }
        transfer.Send(new MSG { Type = MsgType.ClusterRequest, K = k });
    }

    private void btnTour_Click(object sender, RoutedEventArgs e)
    {
        if (transfer == null) { tbStatus.Text = "Nicht verbunden."; return; }
        var ids = lbTour.SelectedItems.OfType<POI>().Select(p => p.Id).ToList();
        if (ids.Count < 2) { tbStatus.Text = "Mindestens 2 POIs auswaehlen."; return; }
        if (ids.Count > 8) { tbStatus.Text = "Maximal 8 POIs."; return; }
        transfer.Send(new MSG { Type = MsgType.TourRequest, SelectedIds = ids });
    }

    public void TransferDisconnected(Transfer t)
    {
        Dispatcher.Invoke(() =>
        {
            tbStatus.Text = "Verbindung getrennt.";
            tbStatus.Foreground = Brushes.DarkRed;
        });
    }
}
