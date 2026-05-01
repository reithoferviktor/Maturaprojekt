using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AStarCanvas;

public partial class MainWindow : Window
{
    // ── Daten ─────────────────────────────────────────────────────────────
    private readonly List<Node> _nodes;
    private readonly List<Edge> _edges;

    // ── Farben ─────────────────────────────────────────────────────────────
    private static readonly Color ColBg       = Color.FromRgb(0x0d, 0x10, 0x20);
    private static readonly Color ColEdge     = Color.FromRgb(0x22, 0x2c, 0x45);
    private static readonly Color ColEdgePath = Color.FromRgb(0xfb, 0xbf, 0x24);
    private static readonly Color ColNode     = Color.FromRgb(0x1e, 0x29, 0x45);
    private static readonly Color ColNodeBord = Color.FromRgb(0x3a, 0x4e, 0x7a);
    private static readonly Color ColStart    = Color.FromRgb(0x16, 0xa3, 0x5c);
    private static readonly Color ColEnd      = Color.FromRgb(0xdc, 0x26, 0x26);
    private static readonly Color ColPath     = Color.FromRgb(0xfb, 0xbf, 0x24);
    private static readonly Color ColHover    = Color.FromRgb(0x2a, 0x3a, 0x60);
    private static readonly Color ColText     = Color.FromRgb(0xc0, 0xcf, 0xe8);

    private const double NodeRadius    = 18;
    private const double CanvasW       = 800;
    private const double CanvasH       = 560;

    // ── Zustand ────────────────────────────────────────────────────────────
    private List<Node>? _path;
    private Node?       _hoveredNode;
    private double      _scaleX = 1, _scaleY = 1;

    // ── Canvas-Elemente ────────────────────────────────────────────────────
    private readonly Dictionary<int, Ellipse>   _ellipses  = new();
    private readonly Dictionary<int, TextBlock> _labels    = new();
    private readonly List<Line>                 _edgeLines = new();

    public MainWindow()
    {
        InitializeComponent();
        var (nodes, edges) = GraphData.Create();
        _nodes = nodes;
        _edges = edges;

        PopulateComboBoxes();
        BuildLegend();

        GraphCanvas.MouseMove += Canvas_MouseMove;
        GraphCanvas.MouseDown += Canvas_MouseDown;

        Loaded += (_, _) => DrawAll();
    }

    // ── ComboBoxen ────────────────────────────────────────────────────────
    private void PopulateComboBoxes()
    {
        foreach (var n in _nodes)
        {
            CbStart.Items.Add(n);
            CbEnd.Items.Add(n);
        }
        CbStart.SelectedIndex = 0;   // Hauptbahnhof
        CbEnd.SelectedIndex   = 19;  // Endstation
        CbStart.DisplayMemberPath = "Name";
        CbEnd.DisplayMemberPath   = "Name";
    }

    // ── Zeichnen ──────────────────────────────────────────────────────────
    private void DrawAll()
    {
        if (GraphCanvas.ActualWidth < 10) return;

        _scaleX = GraphCanvas.ActualWidth  / CanvasW;
        _scaleY = GraphCanvas.ActualHeight / CanvasH;

        GraphCanvas.Children.Clear();
        _ellipses.Clear();
        _labels.Clear();
        _edgeLines.Clear();

        DrawEdges();
        DrawNodes();
    }

    private void DrawEdges()
    {
        var pathSet = BuildPathEdgeSet();

        foreach (var edge in _edges)
        {
            bool onPath = pathSet.Contains((Math.Min(edge.From.Id, edge.To.Id),
                                            Math.Max(edge.From.Id, edge.To.Id)));

            var line = new Line
            {
                X1              = edge.From.X * _scaleX,
                Y1              = edge.From.Y * _scaleY,
                X2              = edge.To.X   * _scaleX,
                Y2              = edge.To.Y   * _scaleY,
                Stroke          = onPath
                    ? new SolidColorBrush(ColEdgePath)
                    : new SolidColorBrush(ColEdge),
                StrokeThickness = onPath ? 3.5 : 1.5,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap   = PenLineCap.Round,
                Opacity = onPath ? 1.0 : 0.7
            };

            GraphCanvas.Children.Add(line);
            _edgeLines.Add(line);

            // Kantengewicht-Label in der Mitte
            if (!onPath)
            {
                double mx = (edge.From.X + edge.To.X) / 2 * _scaleX;
                double my = (edge.From.Y + edge.To.Y) / 2 * _scaleY;
                var wLabel = new TextBlock
                {
                    Text       = $"{edge.Weight:F0}",
                    Foreground = new SolidColorBrush(Color.FromRgb(0x30, 0x40, 0x60)),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize   = 9,
                };
                wLabel.Measure(new Size(50, 20));
                Canvas.SetLeft(wLabel, mx - wLabel.DesiredSize.Width / 2);
                Canvas.SetTop (wLabel, my - wLabel.DesiredSize.Height / 2);
                GraphCanvas.Children.Add(wLabel);
            }
        }
    }

    private void DrawNodes()
    {
        Node? start = CbStart.SelectedItem as Node;
        Node? end   = CbEnd.SelectedItem   as Node;

        foreach (var node in _nodes)
        {
            double cx = node.X * _scaleX;
            double cy = node.Y * _scaleY;
            double r  = NodeRadius;

            bool isStart   = node.Id == start?.Id;
            bool isEnd     = node.Id == end?.Id;
            bool isOnPath  = _path?.Contains(node) == true;
            bool isHovered = node.Id == _hoveredNode?.Id;

            Color fill = isStart ? ColStart
                       : isEnd   ? ColEnd
                       : isOnPath ? ColPath
                       : isHovered ? ColHover
                       : ColNode;

            Color stroke = isStart  ? Color.FromRgb(0x4a, 0xd8, 0x90)
                         : isEnd    ? Color.FromRgb(0xf8, 0x71, 0x71)
                         : isOnPath ? Color.FromRgb(0xff, 0xd0, 0x60)
                         : ColNodeBord;

            double strokeW = (isStart || isEnd || isOnPath) ? 2.5 : 1.5;

            // Glow für Pfadknoten
            var ellipse = new Ellipse
            {
                Width           = r * 2,
                Height          = r * 2,
                Fill            = new SolidColorBrush(fill),
                Stroke          = new SolidColorBrush(stroke),
                StrokeThickness = strokeW,
                Tag             = node,
            };
            if (isOnPath || isStart || isEnd)
            {
                ellipse.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color     = isStart ? ColStart : isEnd ? ColEnd : ColPath,
                    BlurRadius = 10,
                    ShadowDepth = 0,
                    Opacity = 0.7
                };
            }

            Canvas.SetLeft(ellipse, cx - r);
            Canvas.SetTop (ellipse, cy - r);
            Panel.SetZIndex(ellipse, 10);
            GraphCanvas.Children.Add(ellipse);
            _ellipses[node.Id] = ellipse;

            // Kürzel im Knoten
            string abbrev = node.Name.Length >= 2 ? node.Name[..2] : node.Name;
            var inner = new TextBlock
            {
                Text       = abbrev,
                Foreground = new SolidColorBrush(
                    (isStart || isEnd || isOnPath) ? Colors.White : ColText),
                FontFamily  = new FontFamily("Consolas"),
                FontSize    = 9,
                FontWeight  = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
            };
            inner.Measure(new Size(r * 2, r * 2));
            Canvas.SetLeft(inner, cx - inner.DesiredSize.Width / 2);
            Canvas.SetTop (inner, cy - inner.DesiredSize.Height / 2);
            Panel.SetZIndex(inner, 11);
            GraphCanvas.Children.Add(inner);

            // Name-Label unter dem Knoten
            bool showLabel = isStart || isEnd || isOnPath || isHovered;
            if (showLabel)
            {
                var lbl = new TextBlock
                {
                    Text       = node.Name,
                    Foreground = new SolidColorBrush(
                        isStart ? Color.FromRgb(0x4a, 0xd8, 0x90)
                      : isEnd   ? Color.FromRgb(0xf8, 0x71, 0x71)
                      : isOnPath ? Color.FromRgb(0xff, 0xd0, 0x60)
                      : ColText),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize   = 10,
                    FontWeight = isHovered ? FontWeights.Bold : FontWeights.Normal,
                    TextAlignment = TextAlignment.Center,
                };
                lbl.Measure(new Size(150, 30));
                Canvas.SetLeft(lbl, cx - lbl.DesiredSize.Width / 2);
                Canvas.SetTop (lbl, cy + r + 2);
                Panel.SetZIndex(lbl, 12);
                GraphCanvas.Children.Add(lbl);
                _labels[node.Id] = lbl;
            }
        }

        // Pfad-Index-Labels
        if (_path is not null)
        {
            for (int i = 0; i < _path.Count; i++)
            {
                var n  = _path[i];
                double cx = n.X * _scaleX;
                double cy = n.Y * _scaleY;

                var idx = new TextBlock
                {
                    Text       = i.ToString(),
                    Foreground = new SolidColorBrush(Color.FromRgb(0x0d, 0x10, 0x20)),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize   = 9,
                    FontWeight = FontWeights.Bold,
                };
                idx.Measure(new Size(20, 20));
                Canvas.SetLeft(idx, cx - idx.DesiredSize.Width / 2);
                Canvas.SetTop (idx, cy - idx.DesiredSize.Height / 2);
                Panel.SetZIndex(idx, 13);
                GraphCanvas.Children.Add(idx);
            }
        }
    }

    private HashSet<(int, int)> BuildPathEdgeSet()
    {
        var set = new HashSet<(int, int)>();
        if (_path is null || _path.Count < 2) return set;
        for (int i = 0; i < _path.Count - 1; i++)
        {
            int a = Math.Min(_path[i].Id, _path[i + 1].Id);
            int b = Math.Max(_path[i].Id, _path[i + 1].Id);
            set.Add((a, b));
        }
        return set;
    }

    // ── Events ────────────────────────────────────────────────────────────
    private void BtnFind_Click(object sender, RoutedEventArgs e)
    {
        if (CbStart.SelectedItem is not Node start ||
            CbEnd.SelectedItem   is not Node end)
        {
            SetStatus("Bitte Start und Ziel auswählen.", "#f7614f");
            return;
        }

        try
        {
            var pf = new GraphPathfinder(_nodes, _edges);
            _path = pf.FindPath(start, end);

            if (_path is null)
            {
                PathInfo.Text = "Kein Pfad gefunden!";
                CostInfo.Text = "";
                SetStatus("Kein Pfad zwischen den gewählten Knoten.", "#f7614f");
            }
            else
            {
                double cost = 0;
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    double dx = _path[i].X - _path[i + 1].X;
                    double dy = _path[i].Y - _path[i + 1].Y;
                    cost += Math.Sqrt(dx * dx + dy * dy);
                }

                PathInfo.Text = string.Join(" →\n", _path.Select(n => n.Name));
                CostInfo.Text = $"Gesamtkosten: {cost:F1}";
                SetStatus($"Pfad gefunden: {_path.Count} Knoten, Kosten ≈ {cost:F1}", "#36d9a0");
            }
        }
        catch (NotImplementedException ex)
        {
            PathInfo.Text = ex.Message;
            CostInfo.Text = "";
            SetStatus($"NotImplementedException: {ex.Message}", "#f7c34f");
        }
        catch (Exception ex)
        {
            PathInfo.Text = ex.Message;
            CostInfo.Text = "";
            SetStatus($"Fehler: {ex.Message}", "#f7614f");
        }

        DrawAll();
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        _path = null;
        PathInfo.Text = "Kein Pfad berechnet.";
        CostInfo.Text = "";
        SetStatus("Reset.", "#606880");
        DrawAll();
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        var pos = e.GetPosition(GraphCanvas);
        var hit = HitTestNode(pos);
        if (hit?.Id != _hoveredNode?.Id)
        {
            _hoveredNode = hit;
            DrawAll();
        }
    }

    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var pos  = e.GetPosition(GraphCanvas);
        var node = HitTestNode(pos);
        if (node is null) return;

        if (e.ChangedButton == MouseButton.Left)
        {
            CbStart.SelectedItem = node;
        }
        else if (e.ChangedButton == MouseButton.Right)
        {
            CbEnd.SelectedItem = node;
        }

        _path = null;
        PathInfo.Text = "Kein Pfad berechnet.";
        CostInfo.Text = "";
        DrawAll();
    }

    private Node? HitTestNode(Point pos)
    {
        foreach (var node in _nodes)
        {
            double dx = node.X * _scaleX - pos.X;
            double dy = node.Y * _scaleY - pos.Y;
            if (Math.Sqrt(dx * dx + dy * dy) <= NodeRadius + 4)
                return node;
        }
        return null;
    }

    private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawAll();

    private void SetStatus(string msg, string hex)
    {
        StatusBar.Text       = msg;
        StatusBar.Foreground = new SolidColorBrush(
            (Color)ColorConverter.ConvertFromString(hex));
    }

    // ── Legende ───────────────────────────────────────────────────────────
    private void BuildLegend()
    {
        var items = new (Color fill, Color stroke, string label)[]
        {
            (ColStart, Color.FromRgb(0x4a,0xd8,0x90), "S  Startknoten"),
            (ColEnd,   Color.FromRgb(0xf8,0x71,0x71), "E  Zielknoten"),
            (ColPath,  Color.FromRgb(0xff,0xd0,0x60), "●  Pfadknoten"),
            (ColNode,  ColNodeBord,                    "○  Normaler Knoten"),
        };

        foreach (var (fill, stroke, label) in items)
        {
            var row = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin      = new Thickness(0, 2, 0, 2)
            };
            row.Children.Add(new Ellipse
            {
                Width           = 14, Height = 14,
                Fill            = new SolidColorBrush(fill),
                Stroke          = new SolidColorBrush(stroke),
                StrokeThickness = 1.5,
                Margin          = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            });
            row.Children.Add(new TextBlock
            {
                Text      = label,
                Foreground = new SolidColorBrush(Color.FromRgb(0x80, 0x90, 0xb0)),
                FontFamily = new FontFamily("Consolas"),
                FontSize   = 11,
                VerticalAlignment = VerticalAlignment.Center
            });
            LegendPanel.Children.Add(row);
        }

        // Kanten-Legende
        LegendPanel.Children.Add(new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin      = new Thickness(0, 2, 0, 0),
            Children    =
            {
                new Line
                {
                    X1 = 0, Y1 = 7, X2 = 14, Y2 = 7,
                    Stroke = new SolidColorBrush(ColEdgePath),
                    StrokeThickness = 3,
                    Margin = new Thickness(0,0,8,0),
                    VerticalAlignment = VerticalAlignment.Center
                },
                new TextBlock
                {
                    Text      = "Pfad-Kante",
                    Foreground = new SolidColorBrush(Color.FromRgb(0x80, 0x90, 0xb0)),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize   = 11,
                    VerticalAlignment = VerticalAlignment.Center
                }
            }
        });

        LegendPanel.Children.Add(new TextBlock
        {
            Text       = "\nLinksklick = Start setzen\nRechtsklick = Ziel setzen",
            Foreground  = new SolidColorBrush(Color.FromRgb(0x40, 0x50, 0x70)),
            FontFamily  = new FontFamily("Consolas"),
            FontSize    = 10,
            LineHeight  = 16
        });
    }
}
