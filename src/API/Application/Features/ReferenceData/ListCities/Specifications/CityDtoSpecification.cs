namespace Application.Features.ReferenceData.ListCities.Specifications;

public class CityDtoSpecification : Specification<City, CityDTO>
{
    public CityDtoSpecification() : base(c => new CityDTO(c.Id, c.Name))
    {
    }
}
