# ErrorLoggingMiddleware - Production Fix Documentation

## Issue
The ErrorLoggingMiddleware was working in local development but not in production.

## Root Causes Identified
1. **File System Permissions**: Production servers may restrict write access to application directories
2. **Missing Logging Configuration**: No production-specific logging configuration
3. **No Global Exception Handler**: Exceptions were being rethrown without a proper handler
4. **Single Directory Fallback**: Only tried one location for log files

## Changes Made

### 1. Program.cs Updates
- Removed unused `Serilog` import
- Added comprehensive logging providers (Console, Debug, EventLog)
- Added global exception handler using `UseExceptionHandler` for production
- Added `/error` endpoint to handle unhandled exceptions gracefully
- Configured logging to work in both development and production

### 2. ErrorLoggingMiddleware.cs Enhancements
- **Multi-path Fallback**: Tries multiple directories for log files:
  1. ContentRootPath/ErrorLogs (primary)
  2. CurrentDirectory/ErrorLogs (secondary)
  3. TempPath/SchoolManagementAPI_ErrorLogs (fallback)
- **Better Error Information**: Added query string, exception type, and better formatting
- **Permission Handling**: Catches `UnauthorizedAccessException` specifically
- **Dual Logging**: Logs to both file AND ILogger for redundancy
- **Better Error Messages**: More descriptive error messages for troubleshooting

### 3. appsettings.Production.json
Created production-specific configuration with appropriate log levels.

## Verification Steps for Production

### Check if ErrorLoggingMiddleware is Working

1. **Check Application Event Log** (Windows):
   ```powershell
   Get-EventLog -LogName Application -Source ".NET Runtime" -Newest 20
   ```

2. **Check Console Output** (if accessible):
   Look for log entries from ErrorLoggingMiddleware

3. **Check File Locations** (in order of priority):
   - `{ApplicationRoot}/ErrorLogs/`
   - `{CurrentDirectory}/ErrorLogs/`
   - `%TEMP%/SchoolManagementAPI_ErrorLogs/`

4. **Check IIS Application Pool Identity** (if using IIS):
   Ensure the App Pool identity has write permissions to the application directory

### Grant Permissions (Windows Server/IIS)

If logs aren't being created, grant write permissions:

```powershell
# For IIS DefaultAppPool
icacls "C:\inetpub\wwwroot\SchoolManagementAPI\ErrorLogs" /grant "IIS AppPool\DefaultAppPool:(OI)(CI)M"

# For custom app pool (replace YourAppPoolName)
icacls "C:\inetpub\wwwroot\SchoolManagementAPI\ErrorLogs" /grant "IIS AppPool\YourAppPoolName:(OI)(CI)M"
```

### Test Error Logging

Create a test endpoint to trigger an error:

```csharp
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test exception for logging");
    }
}
```

Call: `GET /api/test/test-error`

## Troubleshooting

### Logs Not Created
1. Check Windows Event Viewer for application errors
2. Verify application has write permissions to directories
3. Check if temp directory logs are being created
4. Review console output if available

### Permission Denied Errors
- Run as administrator temporarily to test
- Grant proper permissions to IIS App Pool identity
- Consider using a dedicated logs directory with explicit permissions

### Logs Empty or Not Detailed
- Check `appsettings.Production.json` log level configuration
- Ensure exceptions are actually being thrown
- Verify middleware is registered early in the pipeline

## Best Practices for Production

1. **Use Centralized Logging**: Consider implementing Serilog with a sink to database or external service
2. **Monitor Logs**: Set up alerts for critical errors
3. **Rotate Logs**: Implement log file rotation to prevent disk space issues
4. **Security**: Ensure log files don't contain sensitive information
5. **Access Control**: Restrict log file access to authorized personnel only

## Additional Improvements (Future Enhancements)

Consider implementing:
- **Serilog** with file rolling and structured logging
- **Application Insights** or **Elasticsearch** for centralized logging
- **Log rotation** based on size or date
- **Email notifications** for critical errors
- **Request/Response logging** (be careful with sensitive data)
