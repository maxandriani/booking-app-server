namespace Booking.Core.Commons.ViewModels;

/// <summary>
/// Retorna uma intervalo de uma coleção com metadados de consulta.
/// </summary>
/// <typeparam name="TResponse">Formato da resposta.</typeparam>
/// <param name="Items">Enumerador da página da resposta.</param>
/// <param name="TotalCount">Total de registros na consulta.</param>
public record CollectionResponse<TResponse>(IAsyncEnumerable<TResponse> Items, int TotalCount);