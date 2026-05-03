# Lernhilfe Matura — GomokuManager

Alle Patterns in einem Projekt. Einprägen = dieses Dokument + die Snippets abtippen.

---

## Projektbeschreibung

**GomokuManager** ist eine WPF-App mit:
- Gomoku-Spiel (lokal + Netzwerk) im **MVC**-Pattern
- Spieler-Profil mit Bild (Magick.NET, Bitmap, File/Directory)
- Konfiguration via XML (OpenFileDialog)
- Spielerprofil-Bild via OpenFolderDialog
- Spielerstatistik via MoreLinq
- Spielhistorie in SQLite (linq2db)

---

## 1. MVC: Gomoku

### Model

```csharp
// CellState.cs
public enum CellState { Empty, Black, White }

// Cell.cs
public class Cell : INotifyPropertyChanged
{
    private CellState state = CellState.Empty;
    public CellState State
    {
        get => state;
        set { state = value; OnPropertyChanged(); }
    }
    public int X { get; set; }
    public int Y { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null!)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// Field.cs
public class Field
{
    public ObservableCollection<Cell> Cells { get; set; } = new();
    public int Size { get; set; }

    public Field(int size)
    {
        Size = size;
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                Cells.Add(new Cell { X = x, Y = y });
    }

    public Cell Get(int x, int y) => Cells.First(c => c.X == x && c.Y == y);
}
```

### Controller

```csharp
// AbstractController.cs
public abstract class AbstractController
{
    public abstract Field Gameboard { get; set; }
    public abstract void Clicked(Cell cell);
    public abstract bool IsOver();
}

// LokalController.cs
public class LokalController : AbstractController
{
    private bool isBlack = true;
    public override Field Gameboard { get; set; } = new Field(15);

    public override void Clicked(Cell cell)
    {
        if (cell.State != CellState.Empty) return;
        cell.State = isBlack ? CellState.Black : CellState.White;
        isBlack = !isBlack;
    }

    public override bool IsOver() => false; // TODO: 5-in-a-row prüfen
}
```

### View (MainWindow.xaml.cs)

```csharp
AbstractController ctrl = new LokalController();
DataContext = ctrl.Gameboard;
```

---

## 2. DataBinding + ItemsPanelTemplate

```xml
<!-- UniformGrid als Board -->
<ItemsControl ItemsSource="{Binding Cells}">
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <UniformGrid Rows="15" Columns="15"/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <Button Width="40" Height="40"
                    Click="Cell_Click"
                    Tag="{Binding}">
                <Button.Style>
                    <Style TargetType="Button">
                        <Setter Property="Background" Value="Beige"/>
                        <Style.Triggers>
                            <DataTrigger Binding="{Binding State}" Value="Black">
                                <Setter Property="Background" Value="Black"/>
                            </DataTrigger>
                            <DataTrigger Binding="{Binding State}" Value="White">
                                <Setter Property="Background" Value="White"/>
                            </DataTrigger>
                        </Style.Triggers>
                    </Style>
                </Button.Style>
            </Button>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```

```csharp
// Click-Handler
private void Cell_Click(object sender, RoutedEventArgs e)
{
    var cell = (Cell)((Button)sender).Tag;
    ctrl.Clicked(cell);
}
```

---

## 3. Netzwerk

```csharp
// NetzController.cs
public class NetzController : AbstractController, Receiver
{
    private Transfer transfer;
    public override Field Gameboard { get; set; } = new Field(15);

    public NetzController(string host, int port)
    {
        var client = new TcpClient(host, port);
        transfer = new Transfer(client, this);
        transfer.Start();
    }

    public override void Clicked(Cell cell)
    {
        if (cell.State != CellState.Empty) return;
        cell.State = CellState.Black;
        transfer.Send(new MSG { Type = MsgType.Move, X = cell.X, Y = cell.Y });
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            if (m.Type == MsgType.Move)
                Gameboard.Get(m.X, m.Y).State = CellState.White;
        });
    }

    public void TransferDisconnected(Transfer t) { }
    public override bool IsOver() => false;
}
```

---

## 4. Datenbank — linq2db

### Scaffolding-Befehl (auswendig lernen!)

```
dotnet linq2db scaffold -p SQLite -c "Data Source=gomoku.db" --context-name "GomokuDb" -o Models
```

### Generierte Klassen (Beispiel)

```csharp
// Models/GomokuDb.cs  (auto-generated, nicht ändern)
public class GomokuDb : DataConnection
{
    public GomokuDb() : base("SQLite", "Data Source=gomoku.db") { }
    public ITable<Spieler>  Spielers  => this.GetTable<Spieler>();
    public ITable<Partie>   Parties   => this.GetTable<Partie>();
}

// Models/Spieler.cs  (auto-generated)
[Table("Spieler")]
public class Spieler
{
    [Column(IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true)]
    public long   Id   { get; set; }
    [Column] public string Name      { get; set; } = null!;
    [Column] public string Profilbild { get; set; } = null!;
}
```

### Nutzung

```csharp
using var db = new GomokuDb();

// SELECT
var alle = db.Spielers.ToList();
var s = db.Spielers.FirstOrDefault(x => x.Name == "Viktor");

// INSERT
db.Insert(new Spieler { Name = "Viktor", Profilbild = "viktor.png" });

// UPDATE
db.Update(spieler);

// DELETE
db.Delete(spieler);
```

---

## 5. OpenFileDialog + XML lesen

### OpenFileDialog

```csharp
var ofd = new OpenFileDialog { Filter = "XML Dateien|*.xml|Alle Dateien|*.*" };
if (ofd.ShowDialog() == true)
{
    LoadConfig(ofd.FileName);
}
```

### XML mit XmlSerializer

```xml
<!-- config.xml -->
<Config>
    <Groesse>15</Groesse>
    <Zeitlimit>60</Zeitlimit>
    <SpielerName>Viktor</SpielerName>
</Config>
```

```csharp
// Config-Klasse
public class Config
{
    public int    Groesse     { get; set; }
    public int    Zeitlimit   { get; set; }
    public string SpielerName { get; set; } = "";
}

// Laden
private void LoadConfig(string path)
{
    var xml = new XmlSerializer(typeof(Config));
    using var stream = File.OpenRead(path);
    var cfg = (Config)xml.Deserialize(stream)!;
    // cfg.Groesse, cfg.Zeitlimit etc. verwenden
}

// Speichern
private void SaveConfig(Config cfg, string path)
{
    var xml = new XmlSerializer(typeof(Config));
    using var stream = File.OpenWrite(path);
    xml.Serialize(stream, cfg);
}
```

### XML mit XmlReader (manuell, für komplexe Strukturen)

```csharp
using var reader = XmlReader.Create(path);
while (reader.Read())
{
    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Spieler")
    {
        string name = reader.GetAttribute("Name") ?? "";
        // ...
    }
}
```

---

## 6. OpenFolderDialog

```csharp
// WPF .NET 6+ — Microsoft.Win32
var dialog = new OpenFolderDialog
{
    Title = "Profilbild-Ordner wählen"
};
if (dialog.ShowDialog() == true)
{
    string ordner = dialog.FolderName;
    var bilder = Directory.GetFiles(ordner, "*.jpg")
                          .Concat(Directory.GetFiles(ordner, "*.png"))
                          .ToList();
}
```

---

## 7. File & Directory

```csharp
// Existiert?
bool exists = File.Exists(path);
bool dirExists = Directory.Exists(path);

// Ordner erstellen
Directory.CreateDirectory("profile");

// Alle Dateien eines Typs
string[] jpgs = Directory.GetFiles(ordner, "*.jpg");
string[] all  = Directory.GetFiles(ordner, "*.*", SearchOption.AllDirectories);

// Kopieren / Löschen
File.Copy(quelle, ziel, overwrite: true);
File.Delete(path);

// Lesen / Schreiben
string inhalt = File.ReadAllText(path);
File.WriteAllText(path, "Inhalt");
string[] lines = File.ReadAllLines(path);
```

---

## 8. Magick.NET (ImageMagick)

NuGet: `Magick.NET-Q16-AnyCPU`

```csharp
using ImageMagick;

// Bild verkleinern und speichern
using var img = new MagickImage(inputPath);
img.Resize(80, 80);
img.Write(outputPath);   // Format erkennt Magick aus Dateiendung

// Bild zu BitmapSource konvertieren (für WPF-Anzeige)
using var img2 = new MagickImage(inputPath);
img2.Resize(80, 80);
BitmapSource bmp = img2.ToBitmapSource();
myImage.Source = bmp;

// Format konvertieren
using var img3 = new MagickImage(inputPath);
img3.Format = MagickFormat.Png;
img3.Write("output.png");
```

---

## 9. Bitmap Encode / Decode

### Decode (Datei → BitmapSource, z.B. für Image-Control)

```csharp
// Variante 1 — BitmapImage (einfachste Methode)
var bmp = new BitmapImage(new Uri(path, UriKind.Absolute));
myImage.Source = bmp;

// Variante 2 — BitmapDecoder (wenn man auf Frames zugreifen muss)
var decoder = BitmapDecoder.Create(
    new Uri(path, UriKind.Absolute),
    BitmapCreateOptions.None,
    BitmapCacheOption.OnLoad);
BitmapSource frame = decoder.Frames[0];
```

### Encode (BitmapSource → Datei speichern)

```csharp
// PNG
var encoder = new PngBitmapEncoder();
encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
using var fs = File.OpenWrite("output.png");
encoder.Save(fs);

// JPEG
var jpegEncoder = new JpegBitmapEncoder { QualityLevel = 90 };
jpegEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
using var fs2 = File.OpenWrite("output.jpg");
jpegEncoder.Save(fs2);
```

### Canvas/UI als Bild speichern (Screenshot)

```csharp
private void SaveBoardScreenshot(UIElement element, string path)
{
    var rtb = new RenderTargetBitmap(
        (int)element.RenderSize.Width,
        (int)element.RenderSize.Height,
        96, 96, PixelFormats.Pbgra32);
    rtb.Render(element);

    var encoder = new PngBitmapEncoder();
    encoder.Frames.Add(BitmapFrame.Create(rtb));
    using var fs = File.OpenWrite(path);
    encoder.Save(fs);
}
```

---

## 10. MoreLinq

NuGet: `morelinq`

```csharp
using MoreLinq;

// MaxBy / MinBy — Objekt mit maximalem Wert finden
var besterSpieler = spieler.MaxBy(s => s.Siege);      // ein Element
var schlechtester = spieler.MinBy(s => s.Siege);

// Permutations — alle Reihenfolgen erzeugen
var perms = new List<int> { 1, 2, 3 }.Permutations();
foreach (var p in perms)
    Console.WriteLine(string.Join(",", p));

// Batch — in Gruppen aufteilen
var gruppen = liste.Batch(3);     // je 3 Elemente

// DistinctBy
var unique = liste.DistinctBy(x => x.Name);

// ForEach
spieler.ForEach(s => Console.WriteLine(s.Name));
```

---

## Zusammenfassung: Was du auswendig können musst

| Topic | Key-Pattern |
|---|---|
| INotifyPropertyChanged | `get => field; set { field = value; OnPropertyChanged(); }` + Event + CallerMemberName |
| ItemsPanelTemplate | `<ItemsControl.ItemsPanel><ItemsPanelTemplate><UniformGrid .../></ItemsPanelTemplate></ItemsControl.ItemsPanel>` |
| DataTrigger | In `<Style.Triggers>`: `<DataTrigger Binding="{Binding State}" Value="Black"><Setter .../></DataTrigger>` |
| MVC-Pattern | Abstract `Controller` mit `Gameboard`, `Clicked(Cell)` — View bindet an `Controller.Gameboard` |
| linq2db Befehl | `dotnet linq2db scaffold -p SQLite -c "Data Source=X.db" --context-name "XDb" -o Models` |
| OpenFileDialog | `new OpenFileDialog { Filter = "*.xml" }; if (ofd.ShowDialog() == true) { ... ofd.FileName ... }` |
| OpenFolderDialog | `new OpenFolderDialog(); if (d.ShowDialog() == true) { ... d.FolderName ... }` |
| XML lesen | `new XmlSerializer(typeof(T)); (T)xml.Deserialize(File.OpenRead(path))` |
| Magick.NET | `new MagickImage(path); img.Resize(w,h); img.Write(out);` oder `.ToBitmapSource()` |
| Bitmap Decode | `new BitmapImage(new Uri(path))` |
| Bitmap Encode | `new PngBitmapEncoder(); encoder.Frames.Add(BitmapFrame.Create(src)); encoder.Save(fs)` |
| File/Directory | `Directory.GetFiles(path, "*.jpg")`, `File.Copy(s,d,true)`, `Directory.CreateDirectory(p)` |
| MoreLinq | `using MoreLinq;` → `.MaxBy(x => x.Prop)`, `.Permutations()`, `.Batch(n)` |

---

## Angabe (Klausurformat)

Im Angabe-Projektmappe findest du eine WPF-Anwendung `GomokuManager`. Verwende die vorhanden Klassen und erweitere sie.

**1. Spielfeld (DataBinding + DataTrigger + ItemsPanelTemplate) — 6 P**
Binde das `Field.Cells` an ein `ItemsControl` mit `UniformGrid` als `ItemsPanelTemplate`. Jede Zelle soll einen Button oder Border anzeigen, dessen Hintergrundfarbe per `DataTrigger` von `Cell.State` abhängt (`Empty`=Beige, `Black`=Schwarz, `White`=Weiß).

**2. MVC-Controller — 4 P**
Erstelle `LokalController : AbstractController`. Beim Klick auf eine leere Zelle wechselt der Stein zwischen Schwarz und Weiß.

**3. Netzwerk — 6 P**
Erstelle `NetzController : AbstractController, Receiver`. Beim lokalen Klick → schwarz setzen + `MSG` an Server. Bei empfangener `MSG` → weiß setzen.

**4. Konfiguration (OpenFileDialog + XML) — 5 P**
Menü „Datei → Konfiguration laden": `OpenFileDialog` öffnet eine `*.xml`-Datei. Lese `Groesse` und `SpielerName` per `XmlSerializer`. Passe Spielfeldgröße entsprechend an.

**5. Datenbank (linq2db) — 5 P**
Binde `gomoku.db` via linq2db ein (Befehl zeigen). Speichere nach Spielende eine Partie (Datum, Gewinner) in Tabelle `Partie`. Zeige die letzten 5 Partien in einer ListBox.

**6. Profil-Bilder (OpenFolderDialog + File/Directory + Magick.NET + Bitmap) — 7 P**
Menü „Spieler → Profilbild": `OpenFolderDialog`, alle `.jpg`/`.png` im Ordner auflisten. Das gewählte Bild per `Magick.NET` auf 80×80 verkleinern, als `profile.png` speichern (`File.Copy` in App-Ordner). Das Bild dann per `BitmapImage` in einem `Image`-Control anzeigen.

**7. MoreLinq-Statistik — 2 P**
Zeige aus der Datenbank den Spieler mit den meisten Siegen per `MaxBy`. Zeige alle Spieler nach Siegen sortiert in einer ListBox.
