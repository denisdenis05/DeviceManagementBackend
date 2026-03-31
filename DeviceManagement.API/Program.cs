using MongoDB.Driver;
using DeviceManagement.Business.Services;
using DeviceManagement.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services.AddControllers();
webApplicationBuilder.Services.AddEndpointsApiExplorer();
webApplicationBuilder.Services.AddSwaggerGen();

webApplicationBuilder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var connectionString = webApplicationBuilder.Configuration
    .GetConnectionString("DeviceManagementDatabase");

var databaseName = webApplicationBuilder.Configuration["MongoDbSettings:DatabaseName"];

var mongoClient = new MongoClient(connectionString);

webApplicationBuilder.Services.AddSingleton<IMongoClient>(mongoClient);
webApplicationBuilder.Services.AddScoped<IDevicesRepository, DeviceRepository>();
webApplicationBuilder.Services.AddScoped<IAuthRepository, AuthRepository>();
webApplicationBuilder.Services.AddApplicationServices();

var jwtSettings = webApplicationBuilder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

webApplicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

webApplicationBuilder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

var webApplication = webApplicationBuilder.Build();

webApplication.UseSwagger();
webApplication.UseSwaggerUI();

webApplication.UseHttpsRedirection();
webApplication.UseCors("AllowAll");
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.MapControllers();

webApplication.Run();