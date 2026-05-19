using Microsoft.EntityFrameworkCore;
using ProfessorAPI.src.Data;
using ProfessorAPI.src.Middlewares;
using ProfessorAPI.src.Repositories;
using ProfessorAPI.src.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//registro das dependências
builder.Services.AddScoped<IProfessorRepository,ProfessorRepository>();
builder.Services.AddScoped<IProfessorService,ProfessorService>();

var app = builder.Build();

//middlewares 
app.UseMiddleware<RequestLoggingMiddleware>(); //envolve tudo
app.UseMiddleware<ErrorHandlingMiddleware>();  //captura exceções

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
