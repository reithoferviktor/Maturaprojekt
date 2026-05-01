using System.Windows;
using Client.DataModel;

namespace Client;

public partial class LoadDialog : Window
{
    public RouteEintrag? Auswahl => lbRouten.SelectedItem as RouteEintrag;

    public LoadDialog(IEnumerable<RouteEintrag> eintraege)
    {
        InitializeComponent();
        foreach (var r in eintraege)
            lbRouten.Items.Add(r);
    }

    private void btnOk_Click(object sender, RoutedEventArgs e) { DialogResult = Auswahl != null; }
    private void btnAbbrechen_Click(object sender, RoutedEventArgs e) { DialogResult = false; }
}
