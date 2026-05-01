
using DataModel;
using LinqToDB;
using SubwayGraph;
using System.DirectoryServices;
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
using System.Xml.Linq;

namespace SubwayNetz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SubwayDb db = new SubwayDb(new DataOptions<SubwayDb>(new DataOptions().UseConnectionString("SQLite.MS", "Data Source=subway.db;")));
        List<Stationen> stations = new();
        List<Verbindungen> connections = new();
        List<SubwayGraph.SubwayStation> stationns = new();
        List<SubwayGraph.SubwayConnection> connectionns = new();

        public MainWindow()
        {
            InitializeComponent();
            var curry = db.Stationens;
            foreach (var item in curry)
            {
                stations.Add(item);
                stationns.Add(
                    new SubwayGraph.SubwayStation
                    {
                        Id = (int)item.Id,
                        Name = item.Name,
                        Linie = item.Linie,
                        PosX = item.PosX,
                        PosY = item.PosY
                    });
            }
            var curry2 = db.Verbindungens;
            foreach (var item in curry2)
            {
                connections.Add(item);
                connectionns.Add(
                    new SubwayGraph.SubwayConnection {
                        FahrzeitMinuten = (int)item.FahrzeitMinuten,
                        FromId = (int)item.VonId,
                        IsBidirectional = item.IsBidirectional == 1 ? true :false,
                        ToId = (int)item.NachId,

                    }
                    );
            }
            giraffe.LoadNetwork(stationns, connectionns); 
        }

        private void giraffe_StartStationSelected(object sender, int e)
        {
            SubwayMapControl mcgraph =    sender as SubwayMapControl;
            lb.Content = stationns.FirstOrDefault(x => x.Id == e).Name;
        }

        private void giraffe_EndStationSelected(object sender, int e)
        {
            SubwayMapControl mcgraph = sender as SubwayMapControl;
            Dictionary<SubwayStation, Dictionary<SubwayStation, double>> adjuszenz = new();
            Dictionary<SubwayStation, double> dist = new();
            Dictionary<SubwayStation, SubwayStation> prev = new();
            PriorityQueue<SubwayStation, double> queue = new PriorityQueue<SubwayStation, double>();
            Color[] color = {  Colors.SaddleBrown, Colors.Salmon, Colors.SeaGreen, Colors.Beige };
            foreach (var item in stationns)
            {
                adjuszenz[item] = new Dictionary<SubwayStation, double>();
                if (item.Id == mcgraph.StartStationId)
                {
                    prev[item] = null;
                    dist[item] = 0;
                    queue.Enqueue(item, dist[item]);
                }
                else
                {
                    prev[item] = null;
                    dist[item] = double.PositiveInfinity;
                }
            }
            foreach (var item in connectionns)
            {
                adjuszenz[stationns.FirstOrDefault(x => x.Id == item.FromId)][stationns.FirstOrDefault(x => x.Id == item.ToId)] = (double)item.FahrzeitMinuten;
                if (item.IsBidirectional)
                {
                    adjuszenz[stationns.FirstOrDefault(x => x.Id == item.ToId)][stationns.FirstOrDefault(x => x.Id == item.FromId)] = (double)item.FahrzeitMinuten;
                }

            }
            while (queue.Count > 0)
            {
                SubwayStation q = queue.Dequeue();
                foreach (var item in adjuszenz[q])
                {
                    var alt  = item.Value + dist[q]; 
                    if (alt < dist[item.Key])
                    {
                        dist[item.Key] = alt;
                        prev[item.Key] = q;
                        queue.Enqueue(item.Key, alt);

                    }
                }
            }
            SubwayStation endstation = stationns.FirstOrDefault(x => x.Id == mcgraph.EndStationId);
            string start = lb.Content.ToString();
            lb.Content = "";
            if (prev[endstation] == null)
            {
                MessageBox.Show("kein WEG GEFUNDEN!");
            }
            else
            {
                double fahrzeit = dist[endstation];
                List<int> weg = new List<int>();
                while (prev[endstation] != null)
                {
                    weg.Add(endstation.Id);
                    lb.Content = endstation.Name + " >> " + lb.Content;
                    endstation = prev[endstation];

                }
                weg.Add(endstation.Id);
                Random random  = new Random();
                mcgraph.ShowRoute(weg, color[random.Next(0, color.Length - 1)]);
                lb.Content = start + " >> " + lb.Content + " Fahrzeit: " + fahrzeit.ToString();
                
            }


        }
    }
}