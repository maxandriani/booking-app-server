namespace Booking.Core.Commons.Queries;

public interface ISortableQuery
{
    public string? SortBy { get; init; }
}