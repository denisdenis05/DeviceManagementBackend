using DeviceManagement.API.Requests.Search;
using DeviceManagement.Business.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] SearchRequest request)
    {
        var normalizedQuery = request.ToNormalizedQuery();
        var searchResults = await _searchService.SearchDevicesAsync(normalizedQuery);
        
        return Ok(searchResults);
    }
}
