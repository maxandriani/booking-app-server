namespace Booking.RestServer.V1.ViewModels;

public record SearchGuestContactParams(
    int? Take = null,
    int? Skip = null,
    string? SortBy = null,
    string? Search = null
);