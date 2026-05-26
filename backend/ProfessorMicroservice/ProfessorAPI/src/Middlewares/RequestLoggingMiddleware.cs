using System.Diagnostics;

namespace ProfessorAPI.src.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // O Stopwatch mede o tempo de cada requisição em milissegundos. Isso é útil para identificar endpoints lentos no futuro.
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "→ {Metodo} {Path} iniciado",
                context.Request.Method,
                context.Request.Path
            );

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "← {Metodo} {Path} | Status: {Status} | Duração: {Ms}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
