using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;

namespace Client.DataModel;

public class RoutenDb : DataConnection
{
    public RoutenDb(string dbPath)
        : base(new DataOptions().UseSQLite($"Data Source={dbPath};"))
    {
        this.Execute(@"
            CREATE TABLE IF NOT EXISTS RouteEintraege (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                Typ TEXT NOT NULL,
                StationsIds TEXT NOT NULL,
                Distanz REAL NOT NULL
            );");
    }

    public ITable<RouteEintrag> RouteEintraege => this.GetTable<RouteEintrag>();
}
