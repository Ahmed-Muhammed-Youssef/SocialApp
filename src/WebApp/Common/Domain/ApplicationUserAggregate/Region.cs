namespace Domain.ApplicationUserAggregate;

public class Region : EntityBase
{
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public ICollection<City> Cities { get; set; } = [];
}
