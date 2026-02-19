using Basket.Service.Handlers;
using Basket.Service.Helpers;
using Common.Logging;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Serilog Configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);
//Add swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.MyServices(builder.Configuration);

//Register Mediatr
var assemblies = new Assembly[]
{
  Assembly.GetExecutingAssembly(),
  typeof(CreateShoppingCartHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

//Register Redis service

builder.Services.AddStackExchangeRedisCache(options =>
   {
       options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
   });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
