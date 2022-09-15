namespace Microsoft.AspNetCore.Http;

public static class HeaderDictionaryExtensions
{
    public static IHeaderDictionary AddCollectionHeaders(this IHeaderDictionary headers, int totalCount)
    {
        headers.Add("x-total-count", $"{totalCount}");
        return headers;
    }
}