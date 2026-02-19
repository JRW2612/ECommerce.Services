using Discount.Service.Extensions;
using Discount.Service.Handlers;
using Discount.Service.Helpers;
using Discount.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Register gRPC
builder.Services.AddGrpc();

// Register MediatR
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountHandler).Assembly
};

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(assemblies));

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Database migration
app.MigrateDataBase<Program>();

// ✅ Map gRPC service (modern style)
app.MapGrpcService<DiscountService>();

// Optional REST controllers
app.MapControllers();

app.Run();
