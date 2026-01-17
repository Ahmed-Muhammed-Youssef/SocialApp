namespace Domain.ReferenceData;

public class City : EntityBase
{
    public required string Name { get; set; }

    // Foreign Key
    public int RegionId { get; set; }
    public Region? Region { get; set; } = null;
}
