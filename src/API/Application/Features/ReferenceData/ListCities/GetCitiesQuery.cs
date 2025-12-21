namespace Application.Features.ReferenceData.ListCities;

public record GetCitiesQuery() : IQuery<Result<List<CityDTO>>>;
