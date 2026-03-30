using Microsoft.Extensions.DependencyInjection;
using DeviceManagement.Business.Services.Devices;

namespace DeviceManagement.Business.Services;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDevicesService, DevicesService>();
    }
}
