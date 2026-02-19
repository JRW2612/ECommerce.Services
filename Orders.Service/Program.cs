using Common.Logging;
using Orders.Service.Data;
using Orders.Service.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.RegisterServices(builder.Configuration);
builder.Services.RegisterInfraServices(builder.Configuration);

//Serilog Configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, seeder) =>
{
    var logger = seeder.GetRequiredService<ILogger<OrderContext>>();
    OrderContextSeedData.SeedDataAsync(context, logger).Wait();
});
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
