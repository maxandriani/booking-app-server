namespace Booking.Core.Commons.Responses;

/// <summary>
/// Retorna uma intervalo de uma coleção com metadados de consulta.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public class CollectionResponse<TResponse>
{
    public IAsyncEnumerable<TResponse> Items { get; set; }
    public int TotalCount { get; set; }
    public int? Take { get; set; }
    public int? Skip { get; set; }

    public CollectionResponse(IAsyncEnumerable<TResponse> items, int totalCount, int? take, int? skip)
    {
        Items = items;
        TotalCount = totalCount;
        Take = take;
        Skip = skip;
    }
}