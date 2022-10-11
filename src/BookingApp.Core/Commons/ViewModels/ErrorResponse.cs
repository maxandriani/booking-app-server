namespace BookingApp.Core.Commons.ViewModels;

/// <summary>
/// Envelope padrão de resposta de erro da API
/// </summary>
/// <param name="Message">Mensagem de erro</param>
/// <param name="Type">Tipo da mensagem</param>
/// <param name="Properties">Atributos que originaram o erro.</param>
public record ErrorResponse(string Message, string Type, Dictionary<string, string[]>? Properties = null);
