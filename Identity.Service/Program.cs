using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore;
using Common.Logging;
using Identity.Service.Data;
using Identity.Service.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.AccessControl;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//Serilog Configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);
//EF+Identity
builder.Services.AddDbContext<ApplicationUserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationUserDbContext>()
    .AddDefaultTokenProviders();
//JWT
var jwt = builder.Configuration.GetSection("JWT");
builder.Services.AddAuthentication(token =>
{
    token.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    token.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(w =>
    {
        w.RequireHttpsMetadata = false;
        w.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]))
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();

