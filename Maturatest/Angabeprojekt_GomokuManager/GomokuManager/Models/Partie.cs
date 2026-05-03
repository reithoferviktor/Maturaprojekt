// Auto-generiert durch:
// dotnet linq2db scaffold -p SQLite -c "Data Source=gomoku.db" --context-name "GomokuDb" -o Models

using LinqToDB.Mapping;

namespace GomokuManager.Models;

[Table("Partie")]
public class Partie
{
    [Column(IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)]
    public long   Id       { get; set; }
    [Column] public string Gewinner { get; set; } = null!;
    [Column] public string Datum    { get; set; } = null!;
    [Column] public int    Zuege    { get; set; }
}
