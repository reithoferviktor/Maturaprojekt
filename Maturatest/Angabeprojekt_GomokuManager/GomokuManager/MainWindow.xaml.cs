using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using GomokuManager.Controller;
using GomokuManager.Models;
using GomokuManager.Services;
using LinqToDB;
using Microsoft.Win32;
using MoreLinq;

namespace GomokuManager;

public partial class MainWindow : Window
{
    private AbstractController? ctrl;
    private int zuege = 0;

    public MainWindow()
    {
        InitializeComponent();
        LoadPartien();
    }

    // ----------------------------------------------------------
    // Spiel starten
    // ----------------------------------------------------------

    private void MenuLokal_Click(object sender, RoutedEventArgs e)
    {
        ctrl = new LokalController();
        DataContext = ctrl.Gameboard;
        tbStatus.Text = "Lokal: Schwarz beginnt.";
        zuege = 0;
    }

    private void MenuNetz_Click(object sender, RoutedEventArgs e)
    {
        // TODO 3 aufrufen: NetzController erstellen
        // Tipp: Host + Port z.B. per InputDialog abfragen
        ctrl = new NetzController("127.0.0.1", 5050);
        DataContext = ctrl.Gameboard;
        tbStatus.Text = "Netzwerk: warte auf Gegner...";
        zuege = 0;
    }

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        if (ctrl == null) return;
        var cell = (Cell)((Button)sender).Tag;
        ctrl.Clicked(cell);
        zuege++;
    }

    private void MenuBeenden_Click(object sender, RoutedEventArgs e) => Close();

    // ----------------------------------------------------------
    // TODO 5 — Datenbank: Partie speichern
    // ----------------------------------------------------------
    // Scaffolding-Befehl (einmal ausfuehren, generiert Models/):
    //   dotnet linq2db scaffold -p SQLite -c "Data Source=gomoku.db"
    //                           --context-name "GomokuDb" -o Models
    //
    // Implementierung:
    //   var p = new Partie { Gewinner="Schwarz", Datum=DateTime.Now.ToString("s"), Zuege=zuege };
    //   using var db = new GomokuDb();
    //   db.Insert(p);
    //   LoadPartien();
    private void MenuSpeichern_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Partie in DB speichern (linq2db Insert)
        throw new NotImplementedException("TODO 5 Datenbank speichern");
    }

    // ----------------------------------------------------------
    // TODO 5b — Datenbank: Partien laden und in ListBox anzeigen
    // ----------------------------------------------------------
    //   using var db = new GomokuDb();
    //   var liste = db.Parties.OrderByDescending(p => p.Id).Take(10).ToList();
    //   lbPartien.ItemsSource = liste;
    private void LoadPartien()
    {
        // TODO: alle Partien aus DB laden und lbPartien.ItemsSource setzen
        try
        {
            using var db = new GomokuDb();
            lbPartien.ItemsSource = db.Parties.ToList();
        }
        catch { }
    }

    // ----------------------------------------------------------
    // TODO 6 — OpenFileDialog + XML lesen
    // ----------------------------------------------------------
    // OpenFileDialog:
    //   var ofd = new OpenFileDialog { Filter = "XML Dateien|*.xml" };
    //   if (ofd.ShowDialog() == true) { ... ofd.FileName ... }
    //
    // XML deserialisieren:
    //   var xml = new XmlSerializer(typeof(Config));
    //   using var stream = File.OpenRead(ofd.FileName);
    //   var cfg = (Config)xml.Deserialize(stream)!;
    //   // cfg.Groesse, cfg.Spieler1 etc. verwenden
    private void MenuXml_Click(object sender, RoutedEventArgs e)
    {
        // TODO: OpenFileDialog + XmlSerializer
        throw new NotImplementedException("TODO 6 XML laden");
    }

    // ----------------------------------------------------------
    // TODO 7 — OpenFolderDialog + File/Directory
    // ----------------------------------------------------------
    // OpenFolderDialog:
    //   var dlg = new OpenFolderDialog { Title = "Ordner waehlen" };
    //   if (dlg.ShowDialog() == true) { string ordner = dlg.FolderName; ... }
    //
    // Alle Bilder im Ordner:
    //   var bilder = Directory.GetFiles(ordner, "*.jpg")
    //                         .Concat(Directory.GetFiles(ordner, "*.png"))
    //                         .ToArray();
    //
    // Profilbild kopieren + verkleinern (ImageService):
    //   Directory.CreateDirectory("profile");
    //   string dest = Path.Combine("profile", "profile.png");
    //   ImageService.ResizeAndSave(bilder[0], dest);   // TODO 9 Magick
    //
    // Bild in UI laden (TODO 10 Bitmap Decode):
    //   imgProfil.Source = ImageService.Load(dest);
    private void MenuProfil_Click(object sender, RoutedEventArgs e)
    {
        // TODO: OpenFolderDialog + Directory.GetFiles + ImageService.ResizeAndSave + ImageService.Load
        throw new NotImplementedException("TODO 7+9+10 Profilbild");
    }

    // ----------------------------------------------------------
    // TODO 11 — MoreLinq: MaxBy
    // ----------------------------------------------------------
    // using MoreLinq;
    //   using var db = new GomokuDb();
    //   var partien = db.Parties.ToList();
    //   var beste = partien.GroupBy(p => p.Gewinner)
    //                      .MaxBy(g => g.Count());
    //   MessageBox.Show($"Bester Spieler: {beste.Key} ({beste.Count()} Siege)");
    private void MenuStat_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Partien aus DB holen, GroupBy Gewinner, MaxBy(Count)
        throw new NotImplementedException("TODO 11 MoreLinq");
    }
}
