using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.API.Requests.Search;

public static class SearchExtensions
{
    public static string ToNormalizedQuery(this SearchRequest request)
    {
        return request.Query.Trim();
    }
}
