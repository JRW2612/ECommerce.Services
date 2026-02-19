using ECommerceApiGateWay.Middleware;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= JWT =================
var jwt = builder.Configuration.GetSection("JWT");
var key = jwt["Key"] ?? throw new Exception("JWT Key missing");
var issuer = jwt["Issuer"];
var audience = jwt["Audience"];

builder.Configuration.AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddOcelot();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// ================= CORS =================
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("FrontEndPolicy", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials());
});

var app = builder.Build();

// ================= PIPELINE =================
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseRouting();

app.UseCors("FrontEndPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.UseOcelot();

app.Run();
