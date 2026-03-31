using Microsoft.Extensions.DependencyInjection;
using DeviceManagement.Business.Services.Devices;
using DeviceManagement.Business.Services.Auth;

namespace DeviceManagement.Business.Services;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDevicesService, DevicesService>();
        services.AddScoped<IAuthService, AuthService>();
    }
}
