using System.Net;
using System.Text.Json;

namespace DadosAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next   = next;
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, mensagem) = ex switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest,          ex.Message),
                KeyNotFoundException      => (HttpStatusCode.NotFound,            ex.Message),
                ArgumentException         => (HttpStatusCode.BadRequest,          ex.Message),
                _                         => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente mais tarde.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)statusCode;

            var body = JsonSerializer.Serialize(new
            {
                status  = (int)statusCode,
                erro    = mensagem,
                path    = context.Request.Path.Value,
                momento = DateTime.UtcNow
            });

            await context.Response.WriteAsync(body);
        }
    }

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            _logger.LogInformation("→ {Metodo} {Path} iniciado",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            sw.Stop();

            _logger.LogInformation("← {Metodo} {Path} | Status: {Status} | Duração: {Ms}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds);
        }
    }
}
