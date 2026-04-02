using DeviceManagement.API.Controllers;
using DeviceManagement.API.Requests.Search;
using DeviceManagement.Business.Services.Search;
using DeviceManagement.Data.Models.Devices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeviceManagement.Tests;

public class SearchControllerTests
{
    private readonly Mock<ISearchService> _searchServiceMock;
    private readonly SearchController _searchController;

    public SearchControllerTests()
    {
        _searchServiceMock = new Mock<ISearchService>();
        _searchController = new SearchController(_searchServiceMock.Object);
    }

    [Fact]
    public async Task Search_WhenQueryProvided_ShouldReturnOkWithRankedResults()
    {
        const string searchQuery = " Samsung  ";
        const string normalizedQuery = "Samsung";
        var searchRequest = new SearchRequest { Query = searchQuery };
        var searchResults = new List<DeviceDto> { new DeviceDto { Id = "1", Name = "Galaxy" } };
        _searchServiceMock.Setup(service => service.SearchDevicesAsync(normalizedQuery))
            .ReturnsAsync(searchResults);

        var actionResult = await _searchController.Search(searchRequest);

        actionResult.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(searchResults);
    }
}
