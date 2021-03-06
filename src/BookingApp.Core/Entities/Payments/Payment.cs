using BookingApp.Bookings;
using BookingApp.Places;

namespace BookingApp.Payments;

/// <summary>
/// Any invoide record
/// </summary>
public class Payment
{
  public int Id { get; set; }
  /// <summary>
  /// What place is this money for/come
  /// </summary>
  public int PlaceId { get; set; }
  /// <summary>
  /// <inheritdoc cref="Payment.PlaceId" />
  /// </summary>
  public Place? Place { get; set; }
  /// <summary>
  /// What account this money deposits/withdraw
  /// </summary>
  public int AccountId { get; set; }
  /// <summary>
  /// <inheritdoc cref="Payment.Account" />
  /// </summary>
  public Account? Account { get; set; }
  /// <summary>
  /// Is there a related booking?
  /// </summary>
  public int? BookingId { get; set; }
  /// <summary>
  /// <inheritdoc cref="Payment.Booking" />
  /// </summary>
  public Booking? Booking { get; set; }
  /// <summary>
  /// When this balance occurs
  /// </summary>
  public DateTime When { get; set; }
  /// <summary>
  /// Is this balance confirmed? 
  /// </summary>
  public DateTime? ConfirmedAt { get; set; }
  /// <summary>
  /// How mucht is this record?
  /// </summary>
  public double Amount { get; set; }
  /// <summary>
  /// 255 Characters to describe the meaning of this record.
  /// </summary>
  /// <value></value>
  public string Description { get; set; } = string.Empty;
}
