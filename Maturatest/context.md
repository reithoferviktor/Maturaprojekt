# Context — Übergabedokument für die Matura-Test-Erstellung

> **Zweck dieses Dokuments:** Wenn der ursprüngliche Bearbeiter (z. B. wegen Token-/Zeit-Limits) die Arbeit nicht abschließen kann, muss ein Kollege ohne Vorwissen mit diesem Dokument **direkt weiterarbeiten** können.

---

## 1. Was wird gebaut?

Eine vollständige **Matura-Prüfungsangabe** für einen 5h-Test (4h Praxis + 1h Theorie) für eine HTL-Informatik-Klasse.

- **Aufgabe 1 (Praxis, 2h):** TableScript — eine kleine DSL für berechnete Tabellen (Lexer + Parser + Auswertung + WPF-UI)
- **Aufgabe 2 (Praxis, 2h):** TourPlaner — TCP Client-Server-Anwendung mit Dijkstra, K-Means und Permutationsgenerator
- **Theorie (1h):** 13 Fragen zu UML, Algorithmen, Parser-Theorie, Netzwerk

**Sprache:** Deutsch
**Punkteschema:** 80 Praxis (40+40) / 20 Theorie (7+7+4+2) = 100 gesamt
**Format:** PDF (aus Markdown via pandoc/wkhtmltopdf)

---

## 2. Lieferobjekte und Status

| Datei / Ordner | Zweck | Status |
|---|---|---|
| `context.md` | Dieses Dokument | **fertig** |
| `Angabe_Maturatest.md` | Prüfungsdokument für Schüler:innen | offen |
| `Angabe_Maturatest.pdf` | PDF-Konvertierung | offen |
| `Angabeprojekt_TourPlaner/` | Starter-Projekt für Aufgabe 2 | offen |
| ↳ `TourPlaner.sln` | Visual Studio Solution | offen |
| ↳ `NetworkLib/` | Geteilte Netzwerk-Lib (Transfer, MSG, Receiver) | offen |
| ↳ `Server/` | TCP-Server mit Daten-Loader und Algorithmen-**Stubs** | offen |
| ↳ `Client/` | WPF-Client mit fertigem Canvas-Rendering | offen |
| ↳ `data/pois.csv` | Beispiel-POIs (≥15 Einträge) | offen |
| ↳ `data/verbindungen.csv` | Beispiel-Kanten | offen |
| `Loesungsskizze_Theorie.md` | Musterlösungen zu Theoriefragen | offen |
| `Loesungsskizze_Theorie.pdf` | PDF-Konvertierung | offen |

---

## 3. Wichtige Designentscheidungen

### 3.1 Aufgabe 1 wird "von Grund auf" gebaut
Schüler:innen erstellen die WPF-Solution selbst. Es gibt **kein** Starter-Projekt. Begründung: Aufgabe 1 ist konzeptionell schlank (nur Lexer, Parser, kleine UI) und das Erstellen eines neuen WPF-Projekts gehört zum Lernziel.

### 3.2 Aufgabe 2 hat ein Starter-Projekt
TourPlaner braucht Boilerplate (Solution, 3 Projekte, NetworkLib, CSV-Loader, Canvas-Rendering). Damit die Schüler:innen sich auf **Algorithmen und Netzwerk-Logik** konzentrieren können, gibt es ein vorbereitetes Projekt mit:

- Funktionierendem TCP-Gerüst (Transfer + MSG)
- Funktionierendem CSV-Loader
- Funktionierendem Canvas-Rendering der POIs
- **Stubs** (`throw new NotImplementedException()`) für `DijkstraSolver`, `KMeansSolver`, `PermutationsTourSolver` und für die drei Operations-Handler im Server-Receiver
- Stubs für die drei Aktionen im Client (Pfad / Cluster / Tour)

### 3.3 Stilvorgaben (gilt für alle generierten C#-Dateien!)

- **Sehr wenig Kommentare**, im Idealfall keine
- C# / WPF, .NET 8 oder neuer
- Klassennamen sprechend, aber kompakt (analog `Rechner`/`PA2` aus dem Bestand)
- `Node`/`Edge` als schlanke Datentypen (analog `AStarCanvas`)
- `NetworkLib` analog zur Struktur in `PA3_5A_2026_Reithofer/NetworkLib/` (Transfer, MSG, Receiver-Interface)
- Operator-Precedence-Hierarchie analog `Rechner_2026-04-30_1052/RechnerServer/` (PLUSMINUS → MALDIVIDIERT → HOCH)
- Statement-Dispatcher analog `PA2_Reithofer_Viktor (1)/stmt.cs`

### 3.4 Was die Angabe NICHT enthält
- Keinen Lösungsweg, keine Algorithmus-Pseudocodes für die Aufgaben (nur in der **Lösungsskizze** für den Lehrer)
- Keine Hinweise auf konkrete Klassennamen / Methodensignaturen für Aufgabe 1 (Schüler entwerfen selbst)
- Keine fertigen Code-Snippets — nur **Beispiel-Eingabe** und **erwartetes Ergebnis**

---

## 4. Inspirations-Projekte (in `c:\Users\Viktor Reithofer\Downloads\Maturaprojekt\`)

| Bestandsprojekt | Wofür als Vorlage | Wichtige Dateien |
|---|---|---|
| `PA3_5A_2026_Reithofer` | Netzwerk-Boilerplate (Transfer, MSG, XML-Serialisierung) | `NetworkLib/Transfer.cs`, `NetworkLib/MSG.cs` |
| `Rechner_2026-04-30_1052` | Operator-Precedence-Hierarchie, Server-Receiver-Pattern | `RechnerServer/PLUSMINUS.cs`, `RechnerServer/Program.cs` |
| `PA2_Reithofer_Viktor (1)` | Statement-Dispatcher, Tokenizer-Pattern | `stmt.cs`, `MainWindow.xaml.cs` (btnRun_Click) |
| `AStarCanvas` | Canvas-Rendering von Graphen, Node/Edge-Datentypen | `Node.cs`, `Edge.cs`, `MainWindow.xaml.cs` |
| `Fillialien` | K-Means-Implementierung mit Geo-Daten | `MainWindow.xaml.cs` (kmeans-Methode) |
| `SubwayNetz` | Dijkstra-Implementierung in WPF | `MainWindow.xaml.cs` |
| `ROBOTIKERN` | Token + AbstractExpression-Hierarchie | `AbstractExpression.cs`, `STMT.cs`, `WENN.cs` |

**Wichtig:** Die neuen Projekte dürfen NICHT 1:1 abkupfern. Inspirieren ja, kopieren nein. Vor allem die Beispielprogramme (TableScript-Code, TourPlaner-Operationen) müssen **inhaltlich neu** sein.

---

## 5. Reihenfolge der Erstellung (empfohlen)

1. ✅ `context.md` (dieses Dokument)
2. 🔲 `Angabe_Maturatest.md` schreiben — Hauptdokument, blockiert nichts anderes
3. 🔲 `Angabeprojekt_TourPlaner/` aufbauen
   1. `TourPlaner.sln` + 3 csproj-Dateien (Client/Server/NetworkLib)
   2. `NetworkLib`: `Transfer.cs`, `MSG.cs`, `Receiver.cs`, `City.cs`-äquivalent
   3. `data/pois.csv` + `data/verbindungen.csv`
   4. `Server`: CSV-Loader, Receiver mit Stubs für die drei Operationen, `DijkstraSolver`, `KMeansSolver`, `PermutationsTourSolver` als Stubs
   5. `Client`: `MainWindow.xaml` + Canvas-Rendering, drei Buttons mit Stub-Click-Handlern
4. 🔲 `Loesungsskizze_Theorie.md` (kann auch parallel zu (2) gemacht werden)
5. 🔲 PDF-Konvertierung beider Markdown-Dokumente

---

## 6. PDF-Konvertierung

Zwei Wege, je nachdem was am System verfügbar ist:

**Variante A — pandoc (bevorzugt):**
```powershell
pandoc Angabe_Maturatest.md -o Angabe_Maturatest.pdf --pdf-engine=xelatex -V mainfont="Calibri" -V geometry:margin=2cm
```

**Variante B — markdown → HTML → PDF (wenn pandoc fehlt):**
```powershell
# Falls Node verfügbar:
npx markdown-pdf Angabe_Maturatest.md
```

**Variante C — manueller Fallback:** Markdown in Visual Studio Code öffnen, Extension "Markdown PDF" installieren, "Markdown PDF: Export (pdf)" aufrufen.

> **Hinweis an den Kollegen:** Wenn keiner dieser Wege funktioniert, **die Markdown-Datei abliefern und im Übergabe-Mail erwähnen**, dass die PDF-Konvertierung nicht durchgeführt werden konnte. Der User kann sie selbst in VS Code per Knopfdruck konvertieren.

---

## 7. Kompilierbarkeit des Starter-Projekts

Das Angabeprojekt **muss compilen**, auch wenn die Algorithmen `NotImplementedException` werfen. Nur Methoden-Bodies werfen Exceptions, **keine Syntaxfehler**, **keine fehlenden Klassen-Member**, **keine kaputten Bindings**.

Pflichtprüfung am fertigen Starter:
- [ ] `dotnet build` läuft ohne Fehler
- [ ] Server startet, lädt CSVs, gibt "Lauschet auf Port 5050" o.ä. aus
- [ ] Client startet, verbindet sich, zeigt POIs als Kreise im Canvas an
- [ ] Klick auf "Pfad anzeigen" / "Clustern" / "Beste Tour" wirft kontrolliert eine Fehlermeldung (NotImplementedException) anstatt das Programm abstürzen zu lassen — das ist OK und Teil des Designs

---

## 8. Verbleibende offene Punkte

- Die genaue Auswahl der 15 POIs ist Geschmacksache — wir nehmen Wiener Sehenswürdigkeiten (`Stephansdom`, `Schönbrunn`, `Prater`, ...). Wenn der Kollege etwas anderes präferiert, gerne ändern.
- Der Theoriegraph für Frage 4 (Dijkstra-Trockenlauf) wird als ASCII-Skizze ins Angabe-Dokument eingebunden — das reicht für eine schriftliche Prüfung.
- Punkteschlüssel im Theorieteil: Block A=7, B=7, C=4, D=2. Die Punkte aus dem Plan (8, 8, 4 / 6, 5, 4, 5 / 6, 3, 3 / 3, 3, 2) müssen anteilig auf 7/7/4/2 normiert werden — siehe konkrete Zahlen unten in der Angabe.

---

## 9. Kontakt-Hinweis

Falls Unklarheit besteht:
- Plan-Datei: `C:\Users\Viktor Reithofer\.claude\plans\ziel-eine-angabe-f-r-cheeky-peach.md`
- Bestandsprojekte: `c:\Users\Viktor Reithofer\Downloads\Maturaprojekt\` (alles außer `Maturatest/`)
