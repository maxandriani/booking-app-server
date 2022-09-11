using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.RestServer.Controllers.Guests;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/guests")]
public class GuestController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<GuestResponse>>> Search([FromQuery] SearchGuestsQuery query)
    {
        var response = await _mediator.Send(query);
        
        if (query.Take != null)
            Response.Headers.Add("x-page-take", query.Take.Value.ToString());
        
        if (query.Skip != null)
            Response.Headers.Add("x-page-skip", query.Skip.Value.ToString());

        Response.Headers.Add("x-total-count", response.TotalCount.ToString());

        return Ok(await response.Items.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GuestResponse>> Get(Guid id)
        => Ok(await _mediator.Send(new GetGuestByKeyQuery(id)));

    [HttpPost]
    public async Task<ActionResult<GuestResponse>> Insert([FromBody] GuestCreateUpdateBody body)
    {
        var result = await _mediator.Send(new CreateGuestCmd(body));
        return CreatedAtAction(nameof(Get), new { Id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GuestResponse>> Update(Guid id, [FromBody] GuestCreateUpdateBody body)
        => Ok(await _mediator.Send(new UpdateGuestCmd(id, body)));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteGuestCmd(id));
        return NoContent();
    }
}