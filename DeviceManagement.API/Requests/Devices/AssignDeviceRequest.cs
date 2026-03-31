namespace DeviceManagement.API.Requests.Devices;

public class AssignDeviceRequest
{
    public string DeviceIdentifier { get; set; } = string.Empty;
    public string UserIdentifier { get; set; } = string.Empty;
}
