# Context — Übergabedokument für die Matura-Test-Erstellung

> **Zweck:** Wenn der ursprüngliche Bearbeiter wegen Token-Limits abbrechen muss, kann ein Kollege mit diesem Dokument **direkt weiterarbeiten**.

---

## 1. Was wird gebaut?

Eine **Matura-Prüfungsangabe** für eine HTL-Informatik-Klasse, 5h gesamt.

- **Aufgabe 1 (Praxis, ~2h, 35 P):** AnimSkript — kleine DSL für eine Punktanimation auf Canvas, klassischer AST-Parser
- **Aufgabe 2 (Praxis, ~2h, 35 P):** RoutenPlaner — Client-Server-Anwendung mit Dijkstra + Permutationsgenerator + linq2db (SQLite)
- **Theorie (1h, 30 P):** auf das **POS-Theorie-Skript** abgestimmt (ABNF, Chomsky, Graphentheorie, UML, Software-Pattern, XML/Threads/ORM)

**Format:** PDF (über Markdown via `npx md-to-pdf`)
**Sprache:** Deutsch
**Notenschlüssel** (analog Beispiel-PA):
- 0–50 → 5
- 51–63 → 4
- 64–75 → 3
- 76–88 → 2
- 89–100 → 1

---

## 2. Aufbau jeder Aufgabe (1 Matura-Aufgabe = 1 PA)

Jede Praxisaufgabe ist nach dem **PA-Schema** strukturiert:
- Nummerierte Teilaufgaben (1, 2, 3, ...)
- Pro Teilaufgabe Punkte
- **Vorgegebene WPF-Vorlage** im Starter-Projekt (GUI ist nicht Teil der Aufgabe)
- Klare erwartete Verhaltensweisen
- Keine Lösungsanleitung

---

## 3. Lieferobjekte

| Datei / Ordner | Status |
|---|---|
| `context.md` | **fertig** |
| `Angabe_Maturatest.md` + `.pdf` | offen |
| `Angabeprojekt_AnimSkript/` | offen |
| `Angabeprojekt_RoutenPlaner/` | offen |
| `Loesungsskizze_Theorie.md` + `.pdf` | offen |

---

## 4. Designentscheidungen

### 4.1 Beide Aufgaben haben Starter-Projekte (anders als beim ersten Anlauf)

Schüler:innen bekommen eine **Vorlage** mit:
- Fertige Solution + Projekte
- Fertige WPF-GUI (XAML mit Canvas/ComboBoxen/Buttons)
- Fertige Boilerplate (Tokenizer-Klasse leer, NetworkLib komplett, linq2db-Models scaffolded)
- Algorithmen-Stubs mit `throw new NotImplementedException();`

Dies entspricht dem PA-Pattern aus dem Bestand und lässt 2h Zeit für die eigentliche Aufgabe.

### 4.2 AnimSkript — neue Domain mit klassischem AST-Baum

Sprache, die einen Punkt auf einem Canvas animiert. Sprachelemente:
- `POSITION x y` (absolute Position, kein Strich)
- `BEWEGE dx dy` (relativ, mit Strich)
- `FARBE name` / `DICKE n`
- `WIEDERHOLE n MAL { ... }` (Composite)
- `WENN <vergl> DANN { ... } SONST { ... }` (Composite)
- Vergleich: `POSX | POSY | SCHRITT` `< | > | ==` zahl

Output: gezeichnete Linie auf einem WPF-Canvas. Klarer AST-Baum mit Composite-Pattern.

**Bewusste Abgrenzung zu Bestand:**
- Anders als PA2 (Turtle): keine relativen Drehungen, sondern absolute Positionen
- Anders als ROBOTIKERN (Grid): freier 2D-Raum
- Hat WENN-Bedingungen mit Vergleichsoperatoren (PA2/ROBOTIKERN nicht so)

### 4.3 RoutenPlaner — Dijkstra + Permutationen + linq2db

- **Dijkstra**: kürzester Pfad zwischen 2 Stationen
- **Permutationsgenerator**: beste Tour (TSP mit ≤ 6 Stationen)
- **linq2db + SQLite** auf der **Client-Seite** (`routen.db`) für die Save/Load-Funktionalität — das matched die PA-Vorgabe ("Erstellen Sie eine SQLite Datenbank … binden Sie die Datenbank mittels ORM in das Projekt ein")
- Server hat hardcoded Station/Verbindungs-Daten (für Konsistenz und Einfachheit, kein zweites DB-File nötig)
- TCP/XML-Protokoll mit `MSG`-Klasse, analog `PA3_5A_2026_Reithofer/NetworkLib/`

### 4.4 Stilvorgaben (für alle generierten C#-Dateien)

- C# / WPF, .NET 8 oder neuer
- **Wenig bis keine Kommentare**
- Sprechende, kompakte Klassennamen
- linq2db-Models analog `SubwayNetz/Stationen.cs` (Scaffold-Stil mit `[Table]`/`[Column]`)
- NetworkLib-Struktur analog `PA3_5A_2026_Reithofer/NetworkLib/`

---

## 5. Inspirations-Projekte

| Bestandsprojekt | Wofür als Vorlage |
|---|---|
| `PA3_5A_2026_Reithofer` | Netzwerk-Boilerplate (Transfer/MSG/XML), PA-Aufbau-Schema |
| `PA2_Reithofer_Viktor (1)` | Tokenizer + Statement-Dispatcher + AST |
| `ROBOTIKERN` | AbstractExpression-Hierarchie für AST |
| `Rechner_2026-04-30_1052` | Operator-Precedence-Hierarchie |
| `SubwayNetz` | linq2db-Model-Stil (scaffolded) + Dijkstra-Idee |
| `WhatsApp_Image_2026-03-12_at_13.45.42.jpeg` | **PA-Format-Vorlage** (1–7 Teilaufgaben, Notenschlüssel) |

---

## 6. Reihenfolge der Erstellung

1. ✅ `context.md` (dieses Dokument)
2. 🔲 `Angabe_Maturatest.md` (Hauptdokument, PA-Format)
3. 🔲 `Angabeprojekt_AnimSkript/` (WPF Standalone)
4. 🔲 `Angabeprojekt_RoutenPlaner/` (Solution mit Server, Client, NetworkLib + linq2db)
5. 🔲 `Loesungsskizze_Theorie.md`
6. 🔲 PDF-Konvertierung
7. 🔲 Git commit + push

---

## 7. PDF-Konvertierung

```powershell
cd Maturatest
npx --yes md-to-pdf Angabe_Maturatest.md
npx --yes md-to-pdf Loesungsskizze_Theorie.md
```

Funktioniert mit Node ≥ 14 (auf diesem System bereits geprüft).

---

## 8. Git-Push

Repo: `https://github.com/reithoferviktor/Maturaprojekt.git`, Branch `main`.

```powershell
git add Maturatest
git commit -m "Matura-Test: AnimSkript + RoutenPlaner + Theorie"
git push
```

`.gitignore` für `bin/`, `obj/`, `*.db` (außer geseedeten Test-Daten) ist sinnvoll, falls noch keine vorhanden ist.

---

## 9. Theorie-Skript-Bezug

Die 13 Theorie-Fragen sind direkt an das vom User gelieferte **POS-Theorie-Skript** angelehnt:

| Block | Skript-Themen | Punkte |
|---|---|--:|
| A. Sprache & Parser | ABNF-Grammatik, Chomsky-Hierarchie, Compiler-Phasen, Tokenisierung | 8 |
| B. Graphentheorie & Algorithmen | Begriffe (Schlinge, Clique, bipartit, zentraler Knoten, aufspannender Baum), Dijkstra, Permutationen-Komplexität | 10 |
| C. UML & Software-Pattern | Beziehungstypen, Klassendiagramm, Composite/Interpreter/Observer/Singleton erkennen | 8 |
| D. Netzwerk, ORM, XML | XML-Regeln (Standard-Konstruktor, XmlIgnore, Polymorphie-Wrapper), Threads pro Client/Server, ORM, Datenbinding | 4 |

---

## 10. Bei Übergabe an Kollegen

Wenn du die Implementierung weiterführen musst:

1. **Lies zuerst dieses Dokument vollständig**
2. **Pull aktuellster Stand** (`git pull` auf `main`)
3. **Identifiziere offene Punkte** in der Tabelle aus Abschnitt 3
4. **Hauptregel:** Wenig Kommentare im Code, knappe Sprache in der Angabe, an POS-Skript halten in der Theorie
5. **Verifizieren am Ende:** beide Starter-Projekte kompilieren, beide PDFs öffnen sauber, `git push` läuft durch
