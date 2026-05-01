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
namespace Gokumoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
               InitializeComponent();
        
            ChooseDial cd = new ChooseDial();
            cd.multiplayer.Click += (sender, e) => {
                feld.ABS = new NEtController(int.Parse(cd.tb.Text));
                feld.DataContext = feld.ABS.gameboard;
                cd.Close();
            };
            cd.singleplayer.Click += (sender, e) => {
                feld.ABS = new Lokalspielen(int.Parse(cd.tb.Text));
                feld.DataContext = feld.ABS.gameboard;
                cd.Close();
            };
               cd.Show();

        }



    }
}