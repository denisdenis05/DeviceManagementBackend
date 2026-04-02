using Microsoft.Extensions.DependencyInjection;
using DeviceManagement.Business.Services.Devices;
using DeviceManagement.Business.Services.Auth;
using DeviceManagement.Business.Services.AI;
using DeviceManagement.Business.Services.Search;

namespace DeviceManagement.Business.Services;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDevicesService, DevicesService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAiService, AiService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddHttpClient();
    }
}

