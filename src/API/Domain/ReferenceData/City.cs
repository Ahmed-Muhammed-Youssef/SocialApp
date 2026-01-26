namespace Domain.ReferenceData;

public class City : EntityBase
{
    private City() { }

    public City(string name, int regionId)
    {
        Name = name;
        RegionId = regionId;
    }

    public string Name { get; private set; } = string.Empty;

    // Foreign Key
    public int RegionId { get; private set; }
    public Region? Region { get; private set; }
}
