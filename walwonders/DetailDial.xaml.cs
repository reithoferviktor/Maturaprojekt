using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace walwonders
{
    /// <summary>
    /// Interaktionslogik für DetailDial.xaml
    /// </summary>

    public partial class DetailDial : Window
    {
        public class pfade
        {
            public BitmapSource bitmapSource {  get; set; }
        }

        public ObservableCollection<pfade> Obs {  get; set; }
        public DetailDial()
        {
            Obs = new ObservableCollection<pfade>();
            InitializeComponent();
            DataContext = this;

        }
    }
}
