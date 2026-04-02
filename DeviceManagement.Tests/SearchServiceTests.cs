using DeviceManagement.Business.Services.Search;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Devices;
using FluentAssertions;
using Moq;

namespace DeviceManagement.Tests;

public class SearchServiceTests
{
    private readonly Mock<IDevicesRepository> _devicesRepositoryMock;
    private readonly SearchService _searchService;

    public SearchServiceTests()
    {
        _devicesRepositoryMock = new Mock<IDevicesRepository>();
        _searchService = new SearchService(_devicesRepositoryMock.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SearchDevicesAsync_WhenQueryIsNullOrWhitespace_ShouldReturnAllDevices(string? invalidQuery)
    {
        var allDevices = new List<DeviceDto> { new DeviceDto { Id = "1" }, new DeviceDto { Id = "2" } };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(allDevices);

        var results = await _searchService.SearchDevicesAsync(invalidQuery!);

        results.Should().BeEquivalentTo(allDevices);
    }

    [Fact]
    public async Task SearchDevicesAsync_WhenQueryMatchesSpecificFields_ShouldRankByDeterministicWeights()
    {
        var galaxyPhone = new DeviceDto
        {
            Id = "1",
            Name = "Galaxy S23",
            Manufacturer = "Samsung",
            Processor = "Snapdragon",
            RamAmount = 8
        };
        var appleLaptop = new DeviceDto
        {
            Id = "2",
            Name = "MacBook Pro",
            Manufacturer = "Apple",
            Processor = "M2",
            RamAmount = 16
        };
        var allDevices = new List<DeviceDto> { appleLaptop, galaxyPhone };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(allDevices);

        var results = (await _searchService.SearchDevicesAsync("Samsung Galaxy 16 M2")).ToList();

        results.Should().HaveCount(2);
        results[1].Id.Should().Be("2");
        results[0].Id.Should().Be("1");
    }

    [Fact]
    public async Task SearchDevicesAsync_WhenTokensMatchProcessor_ShouldAccumulateProcessorWeight()
    {
        var deviceWithMatches = new DeviceDto
        {
            Id = "1",
            Name = "Name",
            Manufacturer = "Manufacturer",
            Processor = "Snapdragon",
            RamAmount = 16
        };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(new List<DeviceDto> { deviceWithMatches });

        var results = (await _searchService.SearchDevicesAsync("Snapdragon")).ToList();

        results.Should().HaveCount(1);
    }

    [Fact]
    public async Task SearchDevicesAsync_WhenNoDevicesMatch_ShouldReturnEmptyList()
    {
        var allDevices = new List<DeviceDto> { new DeviceDto { Name = "iPhone", Manufacturer = "Apple", Processor = "A15", RamAmount = 4 } };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(allDevices);

        var results = await _searchService.SearchDevicesAsync("Samsung");

        results.Should().BeEmpty();
    }
}
