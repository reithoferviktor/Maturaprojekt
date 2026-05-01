using LinqToDB;
using OsterHasenHilfe.Data;
using OsterHasenHilfe.Models;
using System.IO;
using System.Numerics;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OsterHasenHilfe
{
    public partial class MainWindow : Window
    {
        // Kartenausschnitt (Geo-Koordinaten von Wiener Neustadt laut Angabe)
        private const double LonLeft = 16.209652;
        private const double LonRight = 16.281017;
        private const double LatBottom = 47.786898;
        private const double LatTop = 47.846533;

        // GeoMapper für Koordinaten-Umrechnung (wird erstellt sobald Canvas-Größe bekannt)
        private GeoMapper? mapper;

        // Farben für die Helfer-Gruppen
        private static readonly Brush[] HelperColors =
        {
            Brushes.Blue, Brushes.Green,
            Brushes.Orange, Brushes.Magenta, Brushes.Cyan,
            Brushes.Brown, Brushes.Purple, Brushes.DarkGreen, Brushes.Gold
        };

        // Ergebnis der K-Means Aufteilung: Index = Person, Wert = Helfer-Nr.
        private List<List<Person>> clusterAssignment = new List<List<Person>>();
        private List<Person> currentPersons = new();

        // Startpunkte der Helfer (Pixel-Koordinaten auf der Karte)
        private Dictionary<int, Point> helperStartPoints = new();

        private string DbPath => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo.db");

        public MainWindow()
        {
            InitializeComponent();
            InitDb();
            LoadPersonList();

            // Wenn sich die Canvas-Größe ändert, GeoMapper neu erstellen
            MapCanvas.SizeChanged += (s, e) =>
            {
                mapper = new GeoMapper(LonLeft, LonRight, LatBottom, LatTop,
                                       MapCanvas.ActualWidth, MapCanvas.ActualHeight);
            };
        }

        // Datenbank und Tabelle erstellen falls nötig
        private void InitDb()
        {
            using var db = new OsterDb(DbPath);
            db.EnsureCreated();
        }

        // ===== Aufgabe 1: Person hinzufügen =====
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TbName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                TbStatus.Text = "Bitte einen Namen eingeben.";
                return;
            }

            // Longitude und Latitude parsen (Komma und Punkt erlaubt)
            if (!double.TryParse(TbLon.Text.Replace(',', '.'),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double lon) ||
                !double.TryParse(TbLat.Text.Replace(',', '.'),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double lat))
            {
                TbStatus.Text = "Bitte gültige Koordinaten eingeben.";
                return;
            }

            // In DB speichern mittels ORM (linq2db)
            using var db = new OsterDb(DbPath);
            db.InsertWithIdentity(new Person { Name = name, Longitude = lon, Latitude = lat });

            TbStatus.Text = $"{name} wurde registriert.";
            TbName.Clear();
            TbLon.Clear();
            TbLat.Clear();
            LoadPersonList();
        }

        // Personenliste in der ListBox aktualisieren
        private void LoadPersonList()
        {
            using var db = new OsterDb(DbPath);
            var persons = db.Personen.ToList();
            LbPersonen.ItemsSource = persons;
        }

        // ===== Aufgabe 2: Alle Personen auf der Karte anzeigen =====
        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            MapCanvas.Children.Clear();
            helperStartPoints.Clear();
            clusterAssignment = null;

            using var db = new OsterDb(DbPath);
            currentPersons = db.Personen.ToList();

            // Jede Person als Kreis auf der Karte zeichnen
            foreach (var p in currentPersons)
                DrawPerson(p, Brushes.Red);

            TbStatus.Text = $"{currentPersons.Count} Personen angezeigt.";
        }

        // ===== Aufgabe 3: K-Means Clustering für Helfer-Aufteilung =====
        private void BtnCluster_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TbHelfer.Text, out int k) || k < 1)
            {
                TbStatus.Text = "Bitte gültige Helfer-Anzahl eingeben.";
                return;
            }

            using var db = new OsterDb(DbPath);
            currentPersons = db.Personen.ToList();

            if (currentPersons.Count == 0)
            {
                TbStatus.Text = "Keine Personen registriert.";
                return;
            }
             
            if (k > currentPersons.Count) k = currentPersons.Count;

            // TODO: K-Means selbst implementieren
            clusterAssignment = KMeans(currentPersons, k);

            // Karte neu zeichnen - jeder Helfer bekommt eine eigene Farbe
            MapCanvas.Children.Clear();
            helperStartPoints.Clear();

            for (int i = 0; i < clusterAssignment.Count; i++)
            {
                foreach (var p in clusterAssignment[i])
                {
                    Brush color = HelperColors[i % HelperColors.Length];
                    DrawPerson(p, color);
                }
            }

            TbStatus.Text = $"Aufgeteilt auf {k} Helfer.";
        }

        /// <summary>
        /// TODO: K-Means implementieren.
        /// Erwartung: Rückgabe-Array Länge = persons.Count, Wertbereich = 0..k-1.
        /// </summary>
        private List<List<Person>> KMeans(List<Person> persons, int k)
        {
            bool converged = false;
            List<Person> neger = new List<Person>(persons);

            List<Person> centroids = new List<Person>();
            Random random = new Random();
            List<List<Person>> clusters1 = new List<List<Person>>();


            for (int i = 0; i < k; i++)
            {
                int x = random.Next(0, neger.Count);
                centroids.Add(neger[x]);
                neger.RemoveAt(x);
            }
            while (converged == false)
            {
                List<List<Person>> clusters = new List<List<Person>>();
                for (int i = 0; i < k; i++)
                {
                    clusters.Add(new List<Person>());
                }
                for (int i = 0; i < persons.Count -1 ;i++)
                {
                    Person point = persons[i];
                    int closestindex = 0;
                    double sd = Measure(point, centroids[0]);
                    for (int j = 1; j <  k; j++)
                    {
                        double d = Measure(point, centroids[j]);
                        if (sd > d) 
                        {   
                            closestindex = j;
                            sd = d;
                        }

                    }
                    clusters[closestindex].Add(point);
                }
                List<Person> newcentroids = new List<Person>();
                bool same = true;
                for (int i = 0; i < k; i++)
                {
                    List<double> sums = new List<double>();
                    sums.Add(0);
                    sums.Add(0);


                    foreach (var point in clusters[i])
                    {
                        sums[0] += point.Longitude;
                        sums[1] += point.Latitude;
                    }
                    Person centroid = new Person();
                    centroid.Longitude = sums[0] / clusters[i].Count;
                    centroid.Latitude = sums[1] / clusters[i].Count;
                    if (centroid.Latitude != centroids[i].Latitude  && centroid.Longitude != centroids[i].Longitude)
                    {
                        same = false;
                    }
                        newcentroids.Add(centroid);
                }
                if (same)
                {
                    converged = true;
                    clusters1 = clusters;
                }
                else
                {
                    centroids = newcentroids;

                }
            }
            return  clusters1;
        }
        public double distance(List<Person> Centroids)
        {
            double shortestdist = double.PositiveInfinity;
            for(int i = 0;i < Centroids.Count;i++)
            {
                double curr = Measure(Centroids[i] , Centroids[i+1]);
                shortestdist += curr;
            }
            return shortestdist; 
        }
        double Measure(Person p1, Person p2)

        {

            var R = 6378.137; // Radius of earth in km

            var dLat = p2.Latitude * Math.PI / 180 - p1.Latitude * Math.PI / 180;

            var dLon = p2.Longitude * Math.PI / 180 - p1.Longitude * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(p1.Latitude * Math.PI / 180) *

                    Math.Cos(p2.Latitude * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // km

        }

        // ===== Aufgabe 4: Routenplanung =====

        // Klick auf die Karte setzt den Startpunkt für einen Helfer
        private void MapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!int.TryParse(TbRouteHelfer.Text, out int helferNr) || helferNr < 1)
                return;

            int helferIndex = helferNr - 1;
            Point clickPos = e.GetPosition(MapCanvas);

            helperStartPoints[helferIndex] = clickPos;
            Brush color = HelperColors[helferIndex % HelperColors.Length];

            // Kreuz als Startpunkt-Markierung zeichnen
            MapCanvas.Children.Add(new Line
            {
                X1 = clickPos.X - 8,
                Y1 = clickPos.Y - 8,
                X2 = clickPos.X + 8,
                Y2 = clickPos.Y + 8,
                Stroke = color,
                StrokeThickness = 3
            });
            MapCanvas.Children.Add(new Line
            {
                X1 = clickPos.X + 8,
                Y1 = clickPos.Y - 8,
                X2 = clickPos.X - 8,
                Y2 = clickPos.Y + 8,
                Stroke = color,
                StrokeThickness = 3
            });

            TbStatus.Text = $"Startpunkt für Helfer {helferNr} gesetzt.";
        }

        // Route für einen Helfer berechnen und zeichnen
        private void BtnRoute_Click(object sender, RoutedEventArgs e)
        {
            if (mapper == null || clusterAssignment == null)
            {
                TbStatus.Text = "Bitte zuerst die Helfer-Aufteilung machen.";
                return;
            }

            if (!int.TryParse(TbRouteHelfer.Text, out int helferNr) || helferNr < 1)
            {
                TbStatus.Text = "Bitte gültige Helfer-Nr. eingeben.";
                return;
            }

            int helferIndex = helferNr - 1;

            if (!helperStartPoints.ContainsKey(helferIndex))
            {
                TbStatus.Text = "Bitte zuerst Startpunkt auf Karte klicken.";
                return;
            }

            // Alle Pixel-Positionen der Personen dieses Helfers sammeln
            var points = new List<Point>();
            for (int i = 0; i < clusterAssignment.Count; i++)
            {
                points.Clear();
                foreach (var p in clusterAssignment[i])
                    points.Add(mapper.GeoToPixel(p.Longitude, p.Latitude));
                // TODO: Route-Algorithmus selbst implementieren
                var route = NearestNeighborRoute(helperStartPoints[i], points);

                // Route als Linien in der Helfer-Farbe zeichnen
                Brush color = HelperColors[i % HelperColors.Length];
                for (int j = 0; j < route.Count - 1; j++)
                {
                    MapCanvas.Children.Add(new Line
                    {
                        X1 = route[j].X,
                        Y1 = route[j].Y,
                        X2 = route[j + 1].X,
                        Y2 = route[j + 1].Y,
                        Stroke = color,
                        StrokeThickness = 2
                    });
                }

            }

            if (points.Count == 0)
            {
                TbStatus.Text = "Keine Personen für diesen Helfer.";
                return;
            }



            TbStatus.Text = $"Route für Helfer {helferNr} berechnet ({points.Count} Stationen).";
            }

        /// <summary>
        /// TODO: Nearest-Neighbor (oder eigener Route-Algorithmus) implementieren.
        /// Erwartung: Rückgabe-Liste enthält Startpunkt + alle Punkte in Besuchsreihenfolge.
        /// </summary>
        private List<Point> NearestNeighborRoute(Point start, List<Point> points)
        {
            Dictionary<Point, bool> allvertices = new Dictionary<Point, bool>();
            List<Point> points1 = new List<Point>();
            points1.Add(start);
            allvertices.Add(start, true);
            foreach (Point point in points)
            {
                allvertices.Add(point, false);
            }
            while (true)
            {
                var curry = allvertices.Where(s => s.Value == false);
                double shortestdist = double.PositiveInfinity;
                Point shortestpoint;
                foreach(var v in curry)
                {
                    double dist = Math.Abs((v.Key.X - points1.Last().X) / (v.Key.Y - points1.Last().Y));
                    if (dist < shortestdist)
                    {
                        shortestdist = dist;
                        shortestpoint = v.Key;
                    }
                }
                if(!allvertices.Any(s => s.Value == false))
                {
                    break;
                }
                else
                {
                    allvertices[shortestpoint] = true;
                    points1.Add(shortestpoint);
                }
            }

            return points1;
        }

        // ===== Hilfsfunktionen =====

        // Person als Kreis auf der Karte zeichnen
        private void DrawPerson(Person p, Brush color)
        {
            if (mapper == null) return;

            // Geo-Koordinaten in Pixel umrechnen
            Point pos = mapper.GeoToPixel(p.Longitude, p.Latitude);

            var ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = color,
                Stroke = Brushes.Black,
                StrokeThickness = 0.5,
                ToolTip = p.Name
            };

            // Ellipse zentriert auf die Position setzen
            Canvas.SetLeft(ellipse, pos.X - 5);
            Canvas.SetTop(ellipse, pos.Y - 5);
            MapCanvas.Children.Add(ellipse);
        }
    }
}