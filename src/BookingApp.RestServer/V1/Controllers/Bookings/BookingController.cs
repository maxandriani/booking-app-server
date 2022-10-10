using System.Net;
using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Queries;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.RestServer.V1.ViewModels.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.RestServer.V1.Controllers.Bookings;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/bookings")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<BookingResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Search([FromQuery] SearchBookingsQuery query)
    {
        var response = await _mediator.Send(query);
        Response.Headers.AddCollectionHeaders(response.TotalCount);
        return Ok(await response.Items.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BookingResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetBookingByKeyQuery(id)));

    [Produces("application/json")]
    [ProducesResponseType(typeof(BookingResponse), 201)]
    [ProducesResponseType(400)]
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] CreateBookingCmd body)
    {
        var result = await _mediator.Send(body);
        return CreatedAtAction(nameof(Get), new { Id = result.Id }, result);
    }

    [Produces("application/json")]
    [ProducesResponseType(204)]
    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        await _mediator.Send(new ConfirmBookingCmd(id));
        return NoContent();
    }

    [Produces("application/json")]
    [ProducesResponseType(204)]
    [HttpPost("{id:guid}/unconfirm")]
    public async Task<IActionResult> UnConfirm(Guid id)
    {
        await _mediator.Send(new UnConfirmBookingCmd(id));
        return NoContent();
    }

    [Produces("application/json")]
    [ProducesResponseType(204)]
    [HttpPost("{bookingId:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid bookingId)
    {
        await _mediator.Send(new CancelBookingCmd(bookingId));
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookingBody body)
        => Ok(await _mediator.Send(new UpdateBookingCmd(id, body.PlaceId, body.CheckIn, body.CheckOut, body.Description)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBookingCmd(id));
        return NoContent();
    }
}