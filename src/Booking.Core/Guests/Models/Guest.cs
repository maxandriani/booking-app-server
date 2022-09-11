namespace Booking.Core.Guests.Models;

/// <summary>
/// Aquele que aluga um espaço.
/// </summary>
public class Guest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}