using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DadosAPI.Data;
using DadosAPI.Middlewares;
using DadosAPI.Repositories;
using DadosAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "DadosAPI",
        Version     = "v1",
        Description = "API responsável por registrar respostas dos alunos e fornecer dados de desempenho para o dashboard."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// ── Banco de dados (PostgreSQL) ───────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Injeção de dependências ───────────────────────────────────────────────────
builder.Services.AddScoped<IDadosRepository, DadosRepository>();
builder.Services.AddScoped<IDadosService,    DadosService>();

var app = builder.Build();

// ── Middlewares ───────────────────────────────────────────────────────────────
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

// ── Swagger ───────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DadosAPI v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
