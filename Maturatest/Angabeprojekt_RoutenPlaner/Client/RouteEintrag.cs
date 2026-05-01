using LinqToDB.Mapping;

#nullable enable

namespace Client.DataModel;

[Table("RouteEintraege")]
public class RouteEintrag
{
    [Column("Id", IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)]
    public long Id { get; set; }

    [Column("Name", CanBeNull = false)]
    public string Name { get; set; } = "";

    [Column("Typ", CanBeNull = false)]
    public string Typ { get; set; } = "";

    [Column("StationsIds", CanBeNull = false)]
    public string StationsIds { get; set; } = "";

    [Column("Distanz")]
    public double Distanz { get; set; }
}
