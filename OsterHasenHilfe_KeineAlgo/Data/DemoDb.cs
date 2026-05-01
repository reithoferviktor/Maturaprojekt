using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using OsterHasenHilfe.Models;

namespace OsterHasenHilfe.Data
{
    public class OsterDb : DataConnection
    {
        public OsterDb(string dbPath)
            : base(SQLiteTools.GetDataProvider(), $"Data Source={dbPath}")
        {
        }

        // Tabelle für alle Personen
        public ITable<Person> Personen => this.GetTable<Person>();

        // Tabelle erstellen falls sie noch nicht existiert
        public void EnsureCreated()
        {
            try
            {
                this.CreateTable<Person>();
            }
            catch
            {
                // Tabelle existiert bereits
            }
        }
    }
}
