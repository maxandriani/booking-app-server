namespace BookingApp.Bookings;

public class CreateUpdateContactInfoDto
{
  /// <summary>
  /// Chave extrangeira.
  /// </summary>
  /// <value></value>
  public int BookingId { get; set; }
  /// <summary>
  /// <inheritdoc cref="ContactInfo.Kind" /> 
  /// </summary>
  public string Kind { get; set; } = string.Empty;
  /// <summary>
  /// <inheritdoc cref="ContactInfo.Value" />
  /// </summary>
  public string Value { get; set; } = string.Empty;
}