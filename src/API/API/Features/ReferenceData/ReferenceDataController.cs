namespace API.Features.ReferenceData;

[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController(IMediator mediator) : ControllerBase
{
    // POST: api/ReferenceData/cities
    [HttpGet("cities")]
    public async Task<ActionResult<List<CityDTO>>> GetCities(CancellationToken cancellationToken)
    {
        Result<List<CityDTO>> result = await mediator.Send(new GetCitiesQuery(), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
