using BookingApp.Core.Bookings.Commands;
using BookingApp.RestServer.V1.ViewModels.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.RestServer.V1.Controllers.Bookings;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/bookings/{bookingId:guid}/guests")]
public class BookingGuestController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingGuestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Produces("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [HttpPost]
    public async Task<IActionResult> AddGuest(Guid bookingId, [FromBody] AddBookingGuestBody body)
    {
        await _mediator.Send(new AddBookingGuestCmd(bookingId, body.GuestId, body.IsPrimary));
        return NoContent();
    }

    [Produces("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [HttpDelete("{guestId:guid}")]
    public async Task<IActionResult> RemoveGuest(Guid bookingId, Guid guestId)
    {
        await _mediator.Send(new DeleteBookingGuestCmd(bookingId, guestId));
        return NoContent();
    }

    // [Produces("application/json")]
    // [ProducesResponseType(204)]
    // [ProducesResponseType(400)]
    // [HttpPost("{guestId:guid}/primary")]
    // public async Task<IActionResult> SetAsPrimary(Guid bookingId, Guid guestId)
    // {
    //     await _mediator.Send(new SetPrimaryBookingGuestCmd(bookingId, guestId));
    //     return NoContent();
    // }
}