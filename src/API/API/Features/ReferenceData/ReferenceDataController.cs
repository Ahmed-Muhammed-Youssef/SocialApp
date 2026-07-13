namespace API.Features.ReferenceData;

[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets a list of all available cities.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of cities.</returns>
    [HttpGet("cities")]
    [ProducesResponseType(typeof(List<CityDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
