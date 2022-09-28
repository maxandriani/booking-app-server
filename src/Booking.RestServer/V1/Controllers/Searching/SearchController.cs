using Booking.Core.Places.Queries;
using Booking.Core.Places.ViewModels;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Booking.RestServer.V1.Controllers.Searching;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/search")]
public class SearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Produces("application/json")]
    [ProducesResponseType(typeof(List<PlaceResponse>), 200)]
    [ProducesResponseType(400)]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] SearchAvailablePlacesForBookingQuery query)
    {
        var response = await _mediator.Send(query);
        Response.Headers.AddCollectionHeaders(response.TotalCount);
        return Ok(await response.Items.ToListAsync());
    }
}
