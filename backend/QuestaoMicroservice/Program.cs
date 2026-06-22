using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestaoAPI.Data;
using QuestaoAPI.Middlewares;
using QuestaoAPI.Repositories;
using QuestaoAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var supabaseUrl = builder.Configuration["Supabase:Url"]
                  ?? throw new InvalidOperationException("Supabase URL não configurado.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{supabaseUrl}/auth/v1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = $"{supabaseUrl}/auth/v1",
            ValidateAudience = true,
            ValidAudiences = new[] { "authenticated" },
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));

builder.Services.AddScoped<IProvaRepository, ProvaRepository>();
builder.Services.AddScoped<IQuestaoRepository, QuestaoRepository>();
builder.Services.AddScoped<IAlternativaRepository, AlternativaRepository>();

builder.Services.AddScoped<IProvaService, ProvaService>();
builder.Services.AddScoped<IQuestaoService, QuestaoService>();
builder.Services.AddScoped<IAlternativaService, AlternativaService>();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();