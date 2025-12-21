namespace Application.Features.ReferenceData.ListCities;

public class GetCitiesHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetCitiesQuery, Result<List<CityDTO>>>
{
    public async ValueTask<Result<List<CityDTO>>> Handle(GetCitiesQuery query, CancellationToken cancellationToken)
    {
        CityDtoSpecification spec = new();

        List<CityDTO> cities = await unitOfWork.CityRepository.ListAsync(spec, cancellationToken);

        return Result<List<CityDTO>>.Success(cities);
    }
}
