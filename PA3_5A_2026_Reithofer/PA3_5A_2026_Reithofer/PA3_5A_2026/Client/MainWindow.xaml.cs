using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window, Receiver
    {
        public static Transfer client;
        public MainWindow()
        {
            InitializeComponent();
            client = new Transfer(new TcpClient("localhost", 12345), this);
            client.Start();
        }

        public void AddDebugInfo(Transfer t, string m, bool sent)
        {
            return;
        }

        public void ReceiveMessage(MSG m, Transfer t)
        {
            if (m.type == Type.Answer)
            {
                Dispatcher.Invoke(() => { canvas.Children.Clear(); });
                Dispatcher.Invoke(() => { lb1.Items.Clear(); });
                if (m.city != null)
                {
                    List<City> list = m.city;
                    foreach (City city in list)
                    {
                        Dispatcher.Invoke(() => { lb1.Items.Add(city); });
                    }
                    foreach (var item in Dispatcher.Invoke(()=>lbs.Items))
                    {
                        City stadt = item as City;
                        list.Add(stadt);
                    }

                    for (int i = 0; i < list.Count; i++)
                    {



                        Rectangle el = new Rectangle();
                        el.Height = 20;
                        el.Width = 20;
                        el.Fill = Brushes.Red;

                        el.StrokeThickness = 1;
                        el.Tag = list[i];

                        Dispatcher.Invoke(() => { Canvas.SetBottom(el, canvas.ActualHeight * ((double)list[i].lat - (-90)) / (90 - (-90))); });
                        Dispatcher.Invoke(() =>{ Canvas.SetLeft(el, canvas.ActualWidth * ((double)list[i].lon - (-180)) / (180 - (-180))); }) ;
                        el.Visibility = Visibility.Visible;
                        Dispatcher.Invoke(() => { canvas.Children.Add(el); });

                       
                    }
                }
            }
        }

        public void TransferDisconnected(Transfer t)
        {
            return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string search = tb.Text;
            client.Send(new MSG { type = Type.Request, search = search });
        }

        private void save1_Click(object sender, RoutedEventArgs e)
        {
            lbs.Items.Add(lb1.SelectedItem);
        }
       /* private void routeanzeigen_Click(object sender, RoutedEventArgs e)
        {
            string colour = "";
            var perso = new List<Personen>();
            Personen p = db.Personens.Find((long)selectedplayer);
            foreach (UIElement rl in Map.Children)
            {
                if (rl is Rectangle rel)
                {
                    Personen prl = (Personen)rel.Tag;
                    if (prl.Id == p.Id)
                    {
                        colour = rel.Fill.ToString();

                    }
                }

            }
            foreach (UIElement rl in Map.Children)
            {
                if (rl is Rectangle rel)
                {
                    if (rel.Fill.ToString() == colour)
                    {
                        perso.Add((Personen)rel.Tag);
                    }
                }

            }

            int n = perso.Count;
            double[,] distanzmatrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double dx = (double)perso[i].Longitude - (double)perso[j].Longitude;
                    double dy = (double)perso[i].Lattitude - (double)perso[j].Lattitude;
                    distanzmatrix[i, j] = Math.Sqrt(dx * dx + dy * dy);
                }
            }
            int startindex = perso.FindIndex(f => f.Id == (long)selectedplayer);
            var res = NearestNeighbor(distanzmatrix, n, startindex);

            for (int i = 0; i < res.path.Count - 2; i++)
            {
                Personen a = perso[res.path[i]];
                Personen b = perso[res.path[i + 1]];
                /*
                double x1 = Map.ActualWidth * ((double)a.Longitude - LINKS_LONGITUDE) / (RECHTS_LONGITUDE - LINKS_LONGITUDE);
                double y1 = Map.ActualHeight * ((double)a.Lattitude - UNTEN_LATITUDE) / (OBEN_LATITUDE - UNTEN_LATITUDE);
                */
               /* double x1 = canvas.ActualWidth * ((double)a.Longitude - 16.209652) / (16.281017 - 16.209652);
                double y1 = Map.ActualHeight * ((double)a.Lattitude - 47.786898) / (47.846533 - 47.786898);

                double x2 = Map.ActualWidth * ((double)b.Longitude - 16.209652) / (16.281017 - 16.209652);
                double y2 = Map.ActualHeight * ((double)b.Lattitude - 47.786898) / (47.846533 - 47.786898);

                Line line = new Line();
                line.X1 = x1;
                line.Y1 = Map.ActualHeight - y1;
                line.X2 = x2;
                line.Y2 = Map.ActualHeight - y2;
                line.Stroke = (Brush)new BrushConverter().ConvertFromString(colour); //farbe zu brush
                line.StrokeThickness = 2;

                Map.Children.Add(line);
            }


        }*/
        static (double cost, List<int> path) NearestNeighbor(
double[,] dist, int n, int start = 0)
        {
            var visited = new bool[n];
            var path = new List<int> { start };
            visited[start] = true;
            double cost = 0;
            int current = start;

            for (int i = 1; i < n; i++)
            {
                int nearest = -1;
                double minD = double.MaxValue;
                for (int j = 0; j < n; j++)
                    if (!visited[j] && dist[current, j] < minD)
                    { minD = dist[current, j]; nearest = j; }

                visited[nearest] = true;
                path.Add(nearest);
                cost += minD;
                current = nearest;
            }
            cost += dist[current, start];
            path.Add(start);
            return (cost, path);
        }
        static double Distance(double[] a, double[] b)
            => Math.Sqrt(a.Zip(b, (x, y) => (x - y) * (x - y)).Sum());
    }
}