using DataModel;
using LinqToDB;
using System.Collections.ObjectModel;
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

namespace walwonders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public  class halluzination
    {
        public string name {get; set;}
        public string desc {get; set;}
        public Waldwunder waldwunder {get; set;}
    }
    public partial class MainWindow : Window
    {
        public WaldwunderDb db = new WaldwunderDb (new DataOptions<WaldwunderDb>(new DataOptions().UseConnectionString("SQLite.MS", "Data Source=Waldwunder.db;")));
        public ObservableCollection<halluzination>  OPS { get; set;}


        public MainWindow()
        {

            InitializeComponent();
            OPS = new ObservableCollection<halluzination>();

            DataContext = this;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NEwdialog nd = new NEwdialog();
            nd.comboprov.ItemsSource = db.Waldwunders.Select(x => x.Province).Distinct();
            nd.addbtn.Click += (sender, e)  =>
            {
                Waldwunder x = new Waldwunder
                {
                    Province = nd.comboprov.SelectedItem.ToString(),
                    Name = nd.tbname.Text,
                    Latitude = (decimal?)float.Parse(nd.tblat.Text),
                    Longitude = (decimal?)float.Parse(nd.tblon.Text),
                    Description = nd.tbdesc.Text,
                    Type = nd.tbtype.Text

                };
                var curry =  db.InsertWithIdentity(x);
                nd.Close();
                foreach (var item in nd.lb.Items)
                {
                    var tu = (BILDERIMKOPF)item;

                    File.Copy(tu.psychose, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images/" + System.IO.Path.GetFileName(tu.psychose)));
                    Bilder bilder = new Bilder {
                        FkBilder00 = db.Waldwunders.FirstOrDefault(x => x.Id == (System.Int64)curry),
                        Name = System.IO.Path.GetFileNameWithoutExtension(tu.psychose)
                    };
                    db.InsertWithIdentity(bilder);

                }
            };
            nd.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var curry = from wund in db.Waldwunders
                             where wund.Name.Contains(swtb.Text) || wund.Description.Contains(swtb.Text)
                             select wund;
            OPS.Clear();
            canvas.Children.Clear();

            foreach (var item in curry)
            {
                OPS.Add(
                    new halluzination
                    {
                        name = item.Name,
                        desc = item.Description,
                        waldwunder = item
                    });
                Ellipse el = new Ellipse {
                    Fill = Brushes.Red,
                    ToolTip = item.Name,
                    Width = 20,
                    Height = 20,
                    Tag = item
                };
                el.MouseLeftButtonDown += (sender, e) => {
                    lb.SelectedIndex = OPS.IndexOf(OPS.FirstOrDefault(t => t.waldwunder == el.Tag));
                };
                Canvas.SetRight(el, canvas.ActualWidth * ((17.231941 - (double)item.Longitude) / (17.231941 - 9.362383)));
                Canvas.SetTop(el, canvas.ActualHeight * ((49.063175 - (double)item.Latitude) / (49.063175 - 46.308597)));
                canvas.Children.Add(el);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var z = (halluzination)lb.Items[lb.SelectedIndex];
            DetailDial detailDial = new DetailDial();
            var curry = db.Bilders.Where(x => x.FkBilder00 == z.waldwunder);
            detailDial.tlbl.Content = z.waldwunder.Type;
            detailDial.Lonlbl.Content = z.waldwunder.Longitude.ToString();
            detailDial.latlbl.Content = z.waldwunder.Latitude.ToString();
            detailDial.nlbl.Content = z.waldwunder.Name;
            detailDial.dlbl.TextWrapping = TextWrapping.Wrap;
            detailDial.dlbl.Text = z.waldwunder.Description;
            foreach (var item in curry)
            {
                string pfad = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images/" + item.Name);
                var img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(pfad, UriKind.Absolute);
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                detailDial.Obs.Add(new DetailDial.pfade { bitmapSource = img });
            }
            detailDial.Show();
        }
    }
}