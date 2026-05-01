using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
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
using System.Xml;

namespace Fillialien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Filiale
    {
        public int id;
        public string name;
        public double lat;
        public double lon;
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        public List<Filiale> Filialen = new List<Filiale>();
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-Dateien|*.xml";
  bool? result = dialog.ShowDialog();

            if (result == true)
            {

                 XmlReader reader = XmlReader.Create(dialog.OpenFile());

                while (reader.Read())
                {

                    // Prüfen, ob es sich aktuell um ein Element handelt

                    if (reader.NodeType == XmlNodeType.Element)
                    {

                        // Alle relevanten Elemente untersuchen
                        Filiale filiale;
                        switch (reader.Name)
                        {
                            case "Filiale":

                                // Neue Person erzeugen und in Liste eintragen
                                // liste.Add(person);
                                filiale = new Filiale();
                                Filialen.Add(filiale);
                                if (reader.HasAttributes)
                                {

                                    // Attributsliste durchlaufen

                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "id")
                                            filiale.id = int.Parse(reader.Value);
                                        else if (reader.Name == "name")
                                            filiale.name = reader.Value;
                                        else if (reader.Name == "lat")
                                            filiale.lat = double.Parse(reader.Value.Replace(".", ","));
                                        else if (reader.Name == "lon")
                                            filiale.lon = double.Parse(reader.Value.Replace(".", ","));
                                    }
                                }
                                break;
                        }
                    }
                }
                    foreach (var item in Filialen)
                    {
                        Ellipse el = new Ellipse
                        {
                            ToolTip = item.name,
                            Fill = Brushes.DarkTurquoise,
                            Width = 20,
                            Height = 20
                        };
                       
                        Canvas.SetRight(el, canvas.ActualWidth * (17.3 - item.lon) / (17.3 - 9.4));
                        Canvas.SetTop(el, canvas.ActualHeight * (49.1 - item.lat) / (49.1 - 46.2));
                        canvas.Children.Add(el);

                    }
                
                }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            clusterdia dialog = new clusterdia();
            int clusteranzahl = 0;
            dialog.rgn.Click  += (sender ,e) => {
                clusteranzahl = int.Parse(dialog.tb.Text);
                List<List<Filiale>> cluster = kmeans(clusteranzahl, Filialen);
                canvas.Children.Clear();
                Brush[] farben = { Brushes.Magenta, Brushes.Gray, Brushes.Green, Brushes.Blue, Brushes.IndianRed, Brushes.LightCyan, Brushes.LemonChiffon };
                for (int i = 0; i < clusteranzahl; i++)
                {
                    foreach (var item in cluster[i])
                    {
                        Ellipse el = new Ellipse
                        {
                            ToolTip = item.name,
                            Fill = farben[i],
                            Width = 20,
                            Height = 20
                        };

                        Canvas.SetRight(el, canvas.ActualWidth * (17.3 - item.lon) / (17.3 - 9.4));
                        Canvas.SetTop(el, canvas.ActualHeight * (49.1 - item.lat) / (49.1 - 46.2));
                        canvas.Children.Add(el);
                    }
                }
                dialog.Close();
            };
            dialog.Show();



        }
        public List<List<Filiale>> kmeans(int k, List<Filiale> filiales)
        {
            List<List<Filiale>> clusters = new List<List<Filiale>>();
            List<Filiale> Centroids = new List<Filiale>();
            Random random = new Random();
            double xrand;
            double yrand;
            for (int i = 0; i < k; i++)
            {
                xrand = random.NextDouble() * (17.3 - 9.4) + 9.4;
                yrand = random.NextDouble() * (49.1 - 46.2) + 46.2;
                Centroids.Add(new Filiale { id = 9, lat = yrand, lon=xrand, name="centroid" });
                clusters.Add(new List<Filiale>());
            }
            foreach (var item in filiales)
            {
                double distance = double.PositiveInfinity;
                int index = -1;
                for (int i = 0; i < k; i++)
                {
                    double currdist = dist(item.lon, Centroids[i].lon , item.lat, Centroids[i].lat);
                    if (currdist < distance)
                    {
                        distance = currdist;
                        index = i;
                    }

                }
                clusters[index].Add(item);
            }

            bool konvergiert = false;
            while (!konvergiert)
            {
                konvergiert = true;

                for (int i = 0; i < k; i++)
                {
                    double xmeans = 0;
                    double ymeans = 0;
                    for (int j = 0; j < clusters[i].Count; j++)
                    {
                        xmeans += clusters[i][j].lon;
                        ymeans += clusters[i][j].lat;
                    }
                    Centroids[i].lat = ymeans / clusters[i].Count;
                    Centroids[i].lon = xmeans / clusters[i].Count;

                }
                List<List<Filiale>> newcluster = new List<List<Filiale>>();
                for (int i = 0; i < k; i++)
                {
                    newcluster.Add(new List<Filiale>(clusters[i]));
                }
                foreach (var item1 in newcluster)

                {
                    foreach (var item in item1)
                    {
                        double distance = double.PositiveInfinity;
                        int index = -1;
                        for (int i = 0; i < k; i++)
                        {
                            double currdist = dist(item.lon, Centroids[i].lon, item.lat, Centroids[i].lat);
                            if (currdist < distance)
                            {
                                distance = currdist;
                                index = i;
                            }

                        }
                        if (index != newcluster.IndexOf(item1))
                        {
                            clusters[index].Add(item);
                            clusters[newcluster.IndexOf(item1)].Remove(item);
                            konvergiert = false;
                        }
                    }

      

                }


            }

            return clusters;
        }

        public double dist(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }


        
        
    }
}