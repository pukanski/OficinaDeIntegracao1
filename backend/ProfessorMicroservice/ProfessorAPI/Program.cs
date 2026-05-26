using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfessorAPI.src.Data;
using ProfessorAPI.src.Middlewares;
using ProfessorAPI.src.Repositories;
using ProfessorAPI.src.Services;

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

// postgre
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));

//registro das dependências
builder.Services.AddScoped<IProfessorRepository,ProfessorRepository>();
builder.Services.AddScoped<IProfessorService,ProfessorService>();

var app = builder.Build();

//middlewares 
app.UseMiddleware<RequestLoggingMiddleware>(); //envolve tudo
app.UseMiddleware<ErrorHandlingMiddleware>();  //captura exceções

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
