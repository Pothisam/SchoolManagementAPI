using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SchoolManagementAPI.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                try
                {
                    var logsDir = Path.Combine(_env.ContentRootPath ?? Directory.GetCurrentDirectory(), "ErrorLogs");
                    Directory.CreateDirectory(logsDir);
                    var fileName = DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log";
                    var filePath = Path.Combine(logsDir, fileName);

                    var sb = new StringBuilder();
                    sb.AppendLine("Timestamp: " + DateTime.UtcNow.ToString("o"));
                    sb.AppendLine("RequestPath: " + (context.Request?.Path.ToString() ?? "-"));
                    sb.AppendLine("Method: " + (context.Request?.Method ?? "-"));
                    sb.AppendLine("Message: " + ex.Message);
                    sb.AppendLine("StackTrace: " + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        sb.AppendLine("InnerException: " + ex.InnerException.Message);
                        sb.AppendLine("InnerStackTrace: " + ex.InnerException.StackTrace);
                    }
                    sb.AppendLine(new string('-', 80));

                    await File.AppendAllTextAsync(filePath, sb.ToString());
                }
                catch (Exception logEx)
                {
                    // If logging to file fails, fall back to ILogger to avoid swallowing the original exception
                    _logger.LogError(logEx, "Failed to write error to file");
                }

                // Re-throw the exception so the pipeline can handle returning the appropriate response
                throw;
            }
        }
    }
}
