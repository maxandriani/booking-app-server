using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.ViewModels;
using Booking.RestServer.V1.Controllers.Guests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.RestServer.Controllers.Guests;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/guests")]
public class GuestController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<GuestWithContactsResponse>), 200)]
    [ProducesResponseType( 400 )]
    public async Task<IActionResult> Search([FromQuery] SearchGuestsQuery query)
    {
        var response = await _mediator.Send(query);
        Response.Headers.AddCollectionHeaders(response.TotalCount);
        return Ok(await response.Items.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    [Produces( "application/json" )]
    [ProducesResponseType( typeof( GuestWithContactsResponse ), 200 )]
    [ProducesResponseType( 400 )]
    [ProducesResponseType( 404 )]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetGuestByKeyQuery(id)));

    [Produces( "application/json" )]
    [ProducesResponseType( typeof( GuestWithContactsResponse ), 201 )]
    [ProducesResponseType( 400 )]
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] CreateGuestWithContactsCmd body)
    {
        var result = await _mediator.Send(body);
        return CreatedAtAction(nameof(Get), new { Id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType( 204 )]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGuestBody body)
        => Ok(await _mediator.Send(new UpdateGuestCmd(id, body.Name)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteGuestCmd(id));
        return NoContent();
    }
}