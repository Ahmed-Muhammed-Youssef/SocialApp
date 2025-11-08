namespace Domain.ApplicationUserAggregate;

public class Country : EntityBase
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Language { get; set; }

    public ICollection<Region> Regions { get; set; } = [];
}
