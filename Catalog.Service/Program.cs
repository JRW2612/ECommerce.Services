using Catalog.Service.Data;
using Catalog.Service.Handlers;
using Catalog.Service.Helpers;
using Common.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Serilog Configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);
//Add swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register Mediatr Service
var assemlblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetAllBrandHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemlblies));

// Bind configuration
builder.Services.Configure<Constants>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IConfiguration>().GetSection("DatabaseSettings").Get<Constants>());

// Register repositories
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

//Seed Mongo Data at Application Startup

using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var constants = config.GetSection("DatabaseSettings").Get<Constants>();
    await DBSeederClass.SeedDataAsync(constants);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

