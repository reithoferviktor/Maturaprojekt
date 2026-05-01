using LinqToDB.Mapping;

namespace OsterHasenHilfe.Models
{
    // Eine Person die an den Osterhasen glaubt
    [Table("personen")]
    public class Person
    {
        [Column("id"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }
    }
}
