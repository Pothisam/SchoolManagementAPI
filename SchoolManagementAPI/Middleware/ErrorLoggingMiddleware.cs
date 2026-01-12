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
                // Log to file first
                await LogToFileAsync(ex, context);
                
                // Also log using ILogger as fallback
                _logger.LogError(ex, "Exception occurred: {Message}. Path: {Path}, Method: {Method}", 
                    ex.Message, 
                    context.Request?.Path.ToString() ?? "-", 
                    context.Request?.Method ?? "-");

                // Re-throw the exception so the pipeline can handle returning the appropriate response
                throw;
            }
        }

        private async Task LogToFileAsync(Exception ex, HttpContext context)
        {
            try
            {
                // Determine the logs directory
                var logsDir = GetLogsDirectory();
                
                // Ensure directory exists with proper error handling
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                }

                var fileName = DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log";
                var filePath = Path.Combine(logsDir, fileName);

                var sb = new StringBuilder();
                sb.AppendLine("================================================================================");
                sb.AppendLine("Timestamp: " + DateTime.UtcNow.ToString("o"));
                sb.AppendLine("RequestPath: " + (context.Request?.Path.ToString() ?? "-"));
                sb.AppendLine("Method: " + (context.Request?.Method ?? "-"));
                sb.AppendLine("QueryString: " + (context.Request?.QueryString.ToString() ?? "-"));
                sb.AppendLine("Message: " + ex.Message);
                sb.AppendLine("Type: " + ex.GetType().FullName);
                sb.AppendLine("StackTrace: " + ex.StackTrace);
                
                if (ex.InnerException != null)
                {
                    sb.AppendLine("InnerException: " + ex.InnerException.Message);
                    sb.AppendLine("InnerExceptionType: " + ex.InnerException.GetType().FullName);
                    sb.AppendLine("InnerStackTrace: " + ex.InnerException.StackTrace);
                }
                
                sb.AppendLine("================================================================================");
                sb.AppendLine();

                // Use async file writing with proper error handling
                await File.AppendAllTextAsync(filePath, sb.ToString());
                
                _logger.LogInformation("Error logged to file: {FilePath}", filePath);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogError(uaEx, "Access denied when writing error log to file. Check file system permissions for the ErrorLogs directory.");
            }
            catch (Exception logEx)
            {
                // If logging to file fails, fall back to ILogger to avoid swallowing the original exception
                _logger.LogError(logEx, "Failed to write error to file. Error: {ErrorMessage}", logEx.Message);
            }
        }

        private string GetLogsDirectory()
        {
            // Try multiple paths for robustness across different environments
            string logsDir;
            
            // First try: Use ContentRootPath (where the application is running)
            if (!string.IsNullOrEmpty(_env.ContentRootPath))
            {
                logsDir = Path.Combine(_env.ContentRootPath, "ErrorLogs");
                if (TryCreateDirectory(logsDir))
                {
                    return logsDir;
                }
            }

            // Second try: Use current directory
            logsDir = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLogs");
            if (TryCreateDirectory(logsDir))
            {
                return logsDir;
            }

            // Third try: Use temp directory as last resort
            logsDir = Path.Combine(Path.GetTempPath(), "SchoolManagementAPI_ErrorLogs");
            if (TryCreateDirectory(logsDir))
            {
                _logger.LogWarning("Using temporary directory for error logs: {LogsDir}", logsDir);
                return logsDir;
            }

            // Fallback to temp path without creating
            return Path.Combine(Path.GetTempPath(), "SchoolManagementAPI_ErrorLogs");
        }

        private bool TryCreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
