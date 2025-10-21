namespace Domain.Entities;

public class Region
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public ICollection<City> Cities { get; set; } = [];
}
