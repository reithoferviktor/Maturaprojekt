using System.Windows;

namespace Client;

public partial class SaveDialog : Window
{
    public string EingegebenerName => txtName.Text.Trim();

    public SaveDialog() { InitializeComponent(); }

    private void btnOk_Click(object sender, RoutedEventArgs e) { DialogResult = true; }
    private void btnAbbrechen_Click(object sender, RoutedEventArgs e) { DialogResult = false; }
}
