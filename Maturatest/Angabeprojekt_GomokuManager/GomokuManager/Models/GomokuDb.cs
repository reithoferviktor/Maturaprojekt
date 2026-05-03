// Auto-generiert durch:
// dotnet linq2db scaffold -p SQLite -c "Data Source=gomoku.db" --context-name "GomokuDb" -o Models

using LinqToDB;
using LinqToDB.Data;
using Microsoft.Data.Sqlite;

namespace GomokuManager.Models;

public class GomokuDb : DataConnection
{
    public GomokuDb() : base(new DataOptions()
        .UseConnectionString("SQLite.MS", "Data Source=gomoku.db")) { }

    public ITable<Partie> Parties => this.GetTable<Partie>();

    public static void Init()
    {
        using var con = new SqliteConnection("Data Source=gomoku.db");
        con.Open();
        var cmd = con.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Partie (
                Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                Gewinner TEXT    NOT NULL,
                Datum    TEXT    NOT NULL,
                Zuege    INTEGER NOT NULL
            )";
        cmd.ExecuteNonQuery();
    }
}
