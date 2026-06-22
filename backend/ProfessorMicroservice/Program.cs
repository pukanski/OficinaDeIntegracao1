using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfessorAPI.src.Data;
using ProfessorAPI.src.Middlewares;
using ProfessorAPI.src.Repositories;
using ProfessorAPI.src.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. TRAVA DE SEGURANÇA E JWT (Supabase OIDC)
var supabaseUrl = builder.Configuration["Supabase:Url"]
                  ?? throw new InvalidOperationException("Supabase URL não configurado. O contêiner não pode iniciar.");

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

// 2. CONEXÃO COM O BANCO DE DADOS (PostgreSQL / Supabase)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));

// 3. INJEÇÃO DE DEPENDÊNCIAS
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();

var app = builder.Build();

// 4. MIDDLEWARES DA SUA EQUIPE
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

// IMPORTANTE: HttpsRedirection REMOVIDO para não quebrar o tráfego interno do Docker.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();