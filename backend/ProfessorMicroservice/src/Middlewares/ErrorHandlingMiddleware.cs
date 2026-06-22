using System.Net;
using System.Text.Json;

namespace ProfessorAPI.src.Middlewares
{
    public class ErrorHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção capturada pelo middleware: {Mensagem}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }

        }

        //  O switch expression mapeia cada tipo de exceção para um status HTTP. Para adicionar novos casos no futuro, basta incluir uma nova linha no switch — sem mexer no resto do código.
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, mensagem) = ex switch
            {
                // Regra de negócio violada — 400
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),

                // Recurso não encontrado — 404
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),

                // Argumento inválido — 400
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),

                // Qualquer outro erro não mapeado — 500
                _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente mais tarde.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var body = JsonSerializer.Serialize(new
            {
                status = (int)statusCode,
                erro = mensagem,
                path = context.Request.Path.Value,
                momento = DateTime.UtcNow
            });

            await context.Response.WriteAsync(body);
        }
    }
}
