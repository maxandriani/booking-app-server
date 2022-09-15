using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.GuestContacts.ViewModels;
using Booking.RestServer.V1.ViewModels;
using Booking.RestServer.V1.ViewModels.GuestContacts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.RestServer.V1.Controllers.GuestContacts;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/guests/{guestId:guid}/contacts")]
public class GuestContactController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestContactController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<GuestContactResponse>), 200)]
    [ProducesResponseType( 400 )]
    public async Task<IActionResult> Search(Guid guestId, [FromQuery] SearchGuestContactParams query)
    {
        var response = await _mediator.Send(new SearchGuestContactQuery(
            guestId,
            query.Take,
            query.Skip,
            query.SortBy,
            query.Search));
        Response.Headers.AddCollectionHeaders(response.TotalCount);
        return Ok(await response.Items.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    [Produces( "application/json" )]
    [ProducesResponseType( typeof( GuestContactResponse ), 200 )]
    [ProducesResponseType( 400 )]
    [ProducesResponseType( 404 )]
    public async Task<IActionResult> Get(Guid id, Guid guestId)
        => Ok(await _mediator.Send(new GetGuestContactByKeyQuery(id, guestId)));

    [Produces( "application/json" )]
    [ProducesResponseType( typeof( GuestContactResponse ), 201 )]
    [ProducesResponseType( 400 )]
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] CreateGuestContactCmd body)
    {
        var result = await _mediator.Send(body);
        return CreatedAtAction(nameof(Get), new { Id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType( 204 )]
    public async Task<IActionResult> Update(Guid id, Guid guestId, [FromBody] UpdateGuestContactBody body)
        => Ok(await _mediator.Send(new UpdateGuestContactCmd(id, guestId, body.Type, body.Value)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, Guid guestId)
    {
        await _mediator.Send(new DeleteGuestContactCmd(id, guestId));
        return NoContent();
    }
}