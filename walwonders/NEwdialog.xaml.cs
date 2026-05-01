using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace walwonders
{
    /// <summary>
    /// Interaktionslogik für NEwdialog.xaml
    /// </summary>
    public class BILDERIMKOPF
    {
        public string psychose = string.Empty;
        public override string ToString()
        {
            return System.IO.Path.GetFileNameWithoutExtension(psychose);
        }
    }
    public partial class NEwdialog : Window
    {
        public NEwdialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Bild Datein|*.jpg";
             bool? result = dialog.ShowDialog();
            if (result == true)
            {
                lb.Items.Add(new BILDERIMKOPF {  psychose = dialog.FileName });
            }
        }
    }
}
