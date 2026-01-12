# ErrorLogs Permission Setup Script
# Run this script as Administrator on your production server

param(
    [Parameter(Mandatory=$true)]
    [string]$ApplicationPath,
    
    [Parameter(Mandatory=$false)]
    [string]$AppPoolName = "DefaultAppPool"
)

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "ErrorLogs Directory Permission Setup" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Define the ErrorLogs path
$errorLogsPath = Join-Path $ApplicationPath "ErrorLogs"

# Check if application path exists
if (-not (Test-Path $ApplicationPath)) {
    Write-Host "ERROR: Application path does not exist: $ApplicationPath" -ForegroundColor Red
    exit 1
}

Write-Host "Application Path: $ApplicationPath" -ForegroundColor Yellow
Write-Host "ErrorLogs Path: $errorLogsPath" -ForegroundColor Yellow
Write-Host "App Pool Name: $AppPoolName" -ForegroundColor Yellow
Write-Host ""

# Create ErrorLogs directory if it doesn't exist
if (-not (Test-Path $errorLogsPath)) {
    Write-Host "Creating ErrorLogs directory..." -ForegroundColor Green
    try {
        New-Item -ItemType Directory -Path $errorLogsPath -Force | Out-Null
        Write-Host "✓ ErrorLogs directory created successfully" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Failed to create ErrorLogs directory: $_" -ForegroundColor Red
        exit 1
    }
}
else {
    Write-Host "✓ ErrorLogs directory already exists" -ForegroundColor Green
}

Write-Host ""

# Grant permissions to IIS App Pool
$appPoolIdentity = "IIS AppPool\$AppPoolName"
Write-Host "Granting permissions to: $appPoolIdentity" -ForegroundColor Yellow

try {
    # Grant Modify permissions (includes Read, Write, Delete)
    $acl = Get-Acl $errorLogsPath
    $permission = $appPoolIdentity, "Modify", "ContainerInherit,ObjectInherit", "None", "Allow"
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
    $acl.SetAccessRule($accessRule)
    Set-Acl $errorLogsPath $acl
    
    Write-Host "✓ Permissions granted successfully" -ForegroundColor Green
}
catch {
    Write-Host "✗ Failed to grant permissions: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Attempting alternative method using icacls..." -ForegroundColor Yellow
    
    try {
        $icaclsCommand = "icacls `"$errorLogsPath`" /grant `"$appPoolIdentity`:(OI)(CI)M`""
        Invoke-Expression $icaclsCommand | Out-Null
        Write-Host "✓ Permissions granted successfully using icacls" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Failed to grant permissions using icacls: $_" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""

# Verify permissions
Write-Host "Verifying permissions..." -ForegroundColor Yellow
try {
    $acl = Get-Acl $errorLogsPath
    $access = $acl.Access | Where-Object { $_.IdentityReference -like "*$AppPoolName*" }
    
    if ($access) {
        Write-Host "✓ Current permissions for $appPoolIdentity`:" -ForegroundColor Green
        $access | ForEach-Object {
            Write-Host "  - $($_.FileSystemRights) ($($_.AccessControlType))" -ForegroundColor Cyan
        }
    }
    else {
        Write-Host "⚠ Warning: Could not verify permissions" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "⚠ Warning: Could not verify permissions: $_" -ForegroundColor Yellow
}

Write-Host ""

# Test write access by creating a test file
Write-Host "Testing write access..." -ForegroundColor Yellow
$testFile = Join-Path $errorLogsPath "test-write-access.tmp"

try {
    "Test write access at $(Get-Date)" | Out-File -FilePath $testFile -Force
    if (Test-Path $testFile) {
        Write-Host "✓ Write access test successful" -ForegroundColor Green
        Remove-Item $testFile -Force
    }
    else {
        Write-Host "⚠ Warning: Test file was not created" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "✗ Write access test failed: $_" -ForegroundColor Red
    Write-Host "The application may not have permission to write to this directory." -ForegroundColor Red
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Restart your IIS Application Pool" -ForegroundColor White
Write-Host "2. Test the application to trigger an error" -ForegroundColor White
Write-Host "3. Check the ErrorLogs directory for log files" -ForegroundColor White
Write-Host ""
Write-Host "To restart the app pool, run:" -ForegroundColor Yellow
Write-Host "  Restart-WebAppPool -Name '$AppPoolName'" -ForegroundColor Cyan
Write-Host ""
