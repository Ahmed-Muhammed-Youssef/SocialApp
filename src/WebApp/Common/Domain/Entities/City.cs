namespace Domain.Entities;

public class City
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // Foreign Key
    public int RegionId { get; set; }
    public Region? Region { get; set; } = null;
}
