using Booking.Core.Guests.Queries;
using Booking.Core.Places.Commands;
using Booking.Core.Places.Queries;
using Booking.Core.Places.ViewModels;
using Booking.RestServer.V1.Controllers.Places;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.RestServer.Controllers.Places;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/places")]
public class PlaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<PlaceResponse>), 200)]
    [ProducesResponseType( 400 )]
    public async Task<IActionResult> Search([FromQuery] SearchPlaceQuery query)
    {
        var response = await _mediator.Send(query);
        Response.Headers.AddCollectionHeaders(response.TotalCount);
        return Ok(await response.Items.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    [Produces( "application/json" )]
    [ProducesResponseType( typeof( PlaceResponse ), 200 )]
    [ProducesResponseType( 400 )]
    [ProducesResponseType( 404 )]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetPlaceByKeyQuery(id)));

    [Produces( "application/json" )]
    [ProducesResponseType( typeof( PlaceResponse ), 201 )]
    [ProducesResponseType( 400 )]
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] CreatePlaceCmd body)
    {
        var result = await _mediator.Send(body);
        return CreatedAtAction(nameof(Get), new { Id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType( 204 )]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlaceBody body)
        => Ok(await _mediator.Send(new UpdatePlaceCmd(id, body.Name, body.Address)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeletePlaceCmd(id));
        return NoContent();
    }
}