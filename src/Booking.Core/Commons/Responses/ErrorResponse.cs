namespace Booking.Core.Commons.Responses;

/// <summary>
/// Envelope padrão de resposta de erro da API
/// </summary>
public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Properties { get; set; }
}