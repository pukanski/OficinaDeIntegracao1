using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuestaoAPI.Data;
using QuestaoAPI.Middlewares;
using QuestaoAPI.Repositories;
using QuestaoAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "QuestaoAPI",
        Version     = "v1",
        Description = "API responsável pelo gerenciamento de provas, questões e alternativas da plataforma."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// ── Banco de dados (PostgreSQL) ───────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Injeção de dependências ───────────────────────────────────────────────────
builder.Services.AddScoped<IProvaRepository,       ProvaRepository>();
builder.Services.AddScoped<IQuestaoRepository,     QuestaoRepository>();
builder.Services.AddScoped<IAlternativaRepository, AlternativaRepository>();

builder.Services.AddScoped<IProvaService,       ProvaService>();
builder.Services.AddScoped<IQuestaoService,     QuestaoService>();
builder.Services.AddScoped<IAlternativaService, AlternativaService>();

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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuestaoAPI v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
