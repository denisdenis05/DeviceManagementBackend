using DeviceManagement.API.Requests.Search;
using FluentAssertions;

namespace DeviceManagement.Tests;

public class SearchExtensionsTests
{
    [Fact]
    public void ToNormalizedQuery_WhenPassedQueryWithWhitespace_ShouldReturnTrimmedQuery()
    {
        var searchRequest = new SearchRequest { Query = "  Samsung Galaxy  " };

        var normalizedQuery = searchRequest.ToNormalizedQuery();

        normalizedQuery.Should().Be("Samsung Galaxy");
    }
}
