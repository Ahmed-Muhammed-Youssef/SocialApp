namespace Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Language { get; set; }

    public ICollection<Region> Regions { get; set; } = [];
}
