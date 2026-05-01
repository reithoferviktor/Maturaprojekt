# A* auf Canvas-Graph – Angabe

## Voraussetzungen
- .NET 10 SDK  →  https://dotnet.microsoft.com/download
- Visual Studio 2022 (17.12+) oder JetBrains Rider

## Projekt öffnen
Doppelklick auf `AStarCanvas.sln`

---

## Aufgabe

Implementiere die drei markierten Methoden in:

    AStarCanvas/GraphPathfinder.cs

| Methode           | Aufgabe | Punkte |
|-------------------|---------|--------|
| `Heuristic()`     | T1      | 10 Pt. |
| `GetNeighbors()`  | T2      | 10 Pt. |
| `FindPath()`      | T3      | 50 Pt. |

Weitere Aufgaben laut Angabe-Blatt (Kommentare, Dokumentation).

---

## Wie der Graph aufgebaut ist

- 20 Knoten  (`Node` – hat `Id`, `Name`, `X`, `Y`)
- ~38 Kanten (`Edge` – hat `From`, `To`, `Weight`)
- Kanten sind **ungerichtet** (bidirektional)
- Kantengewicht = euklidische Distanz der Canvas-Koordinaten

Die Adjazenzliste `_adjacency` ist bereits fertig aufgebaut –
du musst sie nur in `GetNeighbors` zurückgeben.

---

## Bedienung der App

| Aktion              | Beschreibung                    |
|---------------------|---------------------------------|
| Linksklick Knoten   | Startknoten setzen              |
| Rechtsklick Knoten  | Zielknoten setzen               |
| ComboBox Start/Ziel | Alternativ über Dropdown wählen |
| ▶ Pfad suchen       | Ruft `FindPath()` auf           |
| ↺ Reset             | Pfad zurücksetzen               |

---

## Abgabe
`Nachname_Vorname_AStarCanvas.zip` mit:
- `AStarCanvas/GraphPathfinder.cs`  (deine Implementierung)
