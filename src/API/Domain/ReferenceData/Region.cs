namespace Domain.ReferenceData;

public class Region : EntityBase
{
    private Region() { }

    public Region(string name, int countryId)
    {
        Name = name;
        CountryId = countryId;
    }

    public string Name { get; private set; } = string.Empty;
    public int CountryId { get; private set; }
    public ICollection<City> Cities { get; private set; } = [];
}
