using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System.Diagnostics;

namespace AbcCompany.WebApp.API.Config
{
    public class LogMiddleware
    {
        const string MessageTemplate = "HTTP {0} {1} responded {2} in {3} ms";

        readonly ILogger _logger;
        static readonly HashSet<string> HeaderWhitelist = new HashSet<string> { "Content-Type", "Content-Length", "User-Agent" };

        readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<LogMiddleware>() ??
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var start = Stopwatch.GetTimestamp();
            try
            {
                await _next(httpContext);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                _logger.LogInformation(GetLogMessage(httpContext.Request.Method, GetPath(httpContext), statusCode, elapsedMs));
            }
            catch (Exception ex) when (LogException(httpContext, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex)) { }
        }

        private bool LogException(HttpContext httpContext, double elapsedMs, Exception ex)
        {
            _logger.LogError(ex, GetLogMessage(httpContext.Request.Method, GetPath(httpContext), httpContext.Response?.StatusCode, elapsedMs));
            return false;
        }


        static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }

        static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
        }

        static string GetLogMessage(string method, string path, int? statusCode, double elapsedMs)
        {
            return string.Format(MessageTemplate, method, path, statusCode, elapsedMs);
        }
    }

    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogMiddleware>();
        }
    }
}
