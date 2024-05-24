using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Configuration.EntityFramework;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


Console.Title = "Configuration API";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentityServerConfiguration(opt => {})
    .AddClientConfigurationStore();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
const string connectionString = @"Data Source=Duende.IdentityServer.Quickstart.EntityFramework.db";

builder.Services.AddConfigurationDbContext<ConfigurationDbContext>(options =>
{
    options.ConfigureDbContext = b =>
        b.UseSqlite(connectionString);
});

builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = "https://localhost:5001";
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://localhost:5001", // The issuer your application expects
            ValidateAudience = false,
            ValidTypes = new[] { "at+jwt" },
        };
    });
   
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("DCR", policy =>
    {
        policy.RequireClaim("scope", "Api1");
    });
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapDynamicClientRegistration();

app.Run();