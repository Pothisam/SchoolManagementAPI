@echo off
REM Display message to indicate the start of the process
echo Starting Scaffold-DbContext...

REM Navigate to the project directory where the .csproj file is located
cd /d "D:\Git\SchoolManagementAPI\Libraries\Repository"

REM Run the Scaffold-DbContext command using dotnet ef
dotnet ef dbcontext scaffold "Server=SARASC34693;Database=SchoolManagement;User ID=CMS;Password=Q8w$3Lm9#Vr7Xp2!;Trusted_Connection=False;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Entity --force

REM Check if the command was executed successfully
if %errorlevel% neq 0 (
    echo Scaffold-DbContext failed to execute.
    exit /b 1
)

REM If successful, display success message
echo Scaffold-DbContext executed successfully.
pause