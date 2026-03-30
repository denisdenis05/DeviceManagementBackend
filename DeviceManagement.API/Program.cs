using MongoDB.Driver;
using DeviceManagement.Business.Services;
using DeviceManagement.Data;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services.AddControllers();
webApplicationBuilder.Services.AddEndpointsApiExplorer();
webApplicationBuilder.Services.AddSwaggerGen();

var connectionString = webApplicationBuilder.Configuration
    .GetConnectionString("DeviceManagementDatabase");

var databaseName = webApplicationBuilder.Configuration["MongoDbSettings:DatabaseName"];

var mongoClient = new MongoClient(connectionString);

webApplicationBuilder.Services.AddSingleton<IMongoClient>(mongoClient);
webApplicationBuilder.Services.AddScoped<IDevicesRepository, DeviceRepository>();
webApplicationBuilder.Services.AddApplicationServices();

webApplicationBuilder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

var webApplication = webApplicationBuilder.Build();

if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.UseAuthorization();
webApplication.MapControllers();

webApplication.Run();