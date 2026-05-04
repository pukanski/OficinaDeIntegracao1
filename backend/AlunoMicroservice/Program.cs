using AlunoAPI.Data;
using AlunoAPI.Repositories;
using AlunoAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AlunoAPI",
        Version = "v1",
        Description = "API responsável pelo gerenciamento de alunos da plataforma de questões.",
        Contact = new OpenApiContact
        {
            Name = "Luiz Fernando",
            Email = "luizfmd21@gmail.com"
        }
    });

    // Lê os comentários XML dos controllers para exibir no Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// 🔌 Configura o banco (SQLite para dev local — fácil, sem instalar nada)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase")));

// 📦 Registro das dependências — Scoped = uma instância por requisição HTTP
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IAlunoService, AlunoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AlunoAPI v1");
        options.RoutePrefix = string.Empty; // Swagger na raiz: https://localhost:8000/
    });
}

//app.UseHttpsRedirection();
app.MapControllers();
app.Run();