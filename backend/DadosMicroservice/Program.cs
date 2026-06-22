using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DadosAPI.Data;
using DadosAPI.Middlewares;
using DadosAPI.Repositories;
using DadosAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. VALIDAÇÃO DE SEGURANÇA (Supabase JWT OIDC)
var supabaseUrl = builder.Configuration["Supabase:Url"]
                  ?? throw new InvalidOperationException("Supabase URL não está configurada.");

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

// 2. CONEXÃO COM O POSTGRESQL (Mapeamento unificado)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));

// 3. INJEÇÃO DE DEPENDÊNCIAS
builder.Services.AddScoped<IDadosRepository, DadosRepository>();
builder.Services.AddScoped<IDadosService, DadosService>();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();