namespace Domain.ReferenceData;

public class Country : EntityBase
{
    private Country() { }

    public Country(string name, string code, string language)
    {
        Name = name;
        Code = code;
        Language = language;
    }

    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string Language { get; private set; } = string.Empty;

    public ICollection<Region> Regions { get; private set; } = [];
}
