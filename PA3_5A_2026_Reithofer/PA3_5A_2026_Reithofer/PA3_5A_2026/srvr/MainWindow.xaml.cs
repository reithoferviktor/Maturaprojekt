using System.Net.Sockets;
using System.Security.Cryptography.Xml;
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
using LinqToDB;
namespace srvr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Receiver
    {
        public CitiesDb db { get; set; } =
    new CitiesDb(
        new DataOptions<CitiesDb>(
            new DataOptions()
                .UseConnectionString(ProviderName.SQLiteMS, "Data Source=worldcities.sqlite")));
        TcpListener server = new TcpListener(System.Net.IPAddress.Any, 12345);
        List<Transfer> clients = new List<Transfer>();
        public MainWindow()
        {
            InitializeComponent();
            server.Start();

            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        Transfer t = new Transfer(client, this);
                        lock (clients) { clients.Add(t); }
                        t.Start();
                    }
                    catch { }
                }
            });
        }

        public void AddDebugInfo(Transfer t, string m, bool sent)
        {
           return;
        }

        public void ReceiveMessage(MSG m, Transfer t)
        {
            if (m.type == Type.Request)
            {
                var query = db.Worldcities.Where(f => f.City.ToLower().Contains(m.search.ToLower()) || f.City.StartsWith(m.search.ToLower())|| f.City.EndsWith(m.search.ToLower()));
                if (query != null)
                {
                    MSG ms = new MSG
                    {
                        city = new List<City>(),
                        type = Type.Answer,

                    };
                    foreach (var item in query)
                    {
                        City stadt = new City();
                        stadt.lat = (Double)item.Lat;
                        stadt.lon = (Double)item.Lng;
                        stadt.name = item.City;
                        ms.city.Add(stadt);
                    }
                    t.Send(ms);
                }
            }
        }

        public void TransferDisconnected(Transfer t)
        {
            lock (clients) { clients.Remove(t); }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            server.Stop();
        }
    }
}