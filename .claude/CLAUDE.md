# SchoolManagementAPI — Claude Instructions

## Project Overview

Multi-tenant ASP.NET Core 7/9 Web API for school management. Uses a 4-layer architecture:

```
SchoolManagementAPI/         ← Controllers (Presentation Layer)
Libraries/Models/            ← DTOs, Request/Response objects, Config models
Libraries/Repository/        ← EF Core DbContext, Entities, Data Access
Libraries/Services/          ← Business Logic Layer
```

## Architecture & Layer Rules

- **Controllers** call **Services** only. Never call a Repo directly from a controller.
- **Services** call **Repositories** + other Services (especially `ICommonService`).
- **Repositories** interact with `SchoolManagementContext` only.
- **Models** (DTOs) are shared across all layers — no business logic in them.

Each layer has its own `DependencyInjection.cs`:
- `Libraries/Repository/DependencyInjection.cs` — registers DbContext + all repos
- `Libraries/Services/DependencyInjection.cs` — calls repo DI first, then registers all services
- `SchoolManagementAPI/Program.cs` — calls `ServicesDependencyInjection()` once

## Naming Conventions

| Category | Convention | Example |
|---|---|---|
| Classes | PascalCase | `StudentDetail`, `ClassService` |
| Interfaces | `I` prefix | `IStudentService`, `IClassRepo` |
| Repositories | `Repo` suffix | `StudentRepo`, `ClassRepo` |
| Services | `Service` suffix | `StudentService`, `ClassService` |
| Request DTOs | `Request` suffix | `AddStudentRequest`, `UpdateClassStatusRequest` |
| Response DTOs | `Response` suffix | `StudentMasterViewResponse`, `ClassResponse` |
| FK Columns | `FieldName + Fkid` | `StudentDetailsFkid`, `ClassFkid` |
| DB Constraints | `FK_Table_RefTable` | `FK_ClassSection_Class` |
| Async methods | `Async` suffix | `AddStudentAsync`, `GetClassListAsync` |

## Controller Pattern

Every controller follows this exact pattern:

```csharp
[ApiController]
[Authorize]
[Route("[controller]")]
public class XxxController : Controller
{
    private readonly IXxxService _IXxxService;
    private readonly ICommonService _ICommonService;

    public XxxController(IXxxService xxxService, ICommonService commonService)
    {
        _IXxxService = xxxService;
        _ICommonService = commonService;
    }

    [HttpPost("ActionName")]
    public async Task<IActionResult> ActionName(XxxRequest request)
    {
        var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
        var result = await _IXxxService.ActionName(request, apirequestdetails);
        return Ok(result);
    }
}
```

Key rules:
- **All endpoints use `[HttpPost]`** — no GET, PUT, DELETE verbs used in this project
- **Always extract** `APIRequestDetails` from JWT claims via `_ICommonService.GetAPIRequestDetails(User)`
- **Always return** `Ok(result)` — the `CommonResponse<T>` wrapper carries success/failure internally
- **All controller methods are async**

## Response Wrapper

All service methods return `CommonResponse<T>`:

```csharp
public class CommonResponse<T> where T : class
{
    public string? Message { get; set; }
    public Status Status { get; set; }  // Status.Success = 200, Status.Failed = 300
    public T? Data { get; set; }
}
```

Never return raw data from a service — always wrap in `CommonResponse<T>`.

## Multi-Tenancy

Every entity has `InstitutionCode` (int). Every query **must** filter by it:

```csharp
.Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode)
```

`APIRequestDetails` is extracted from JWT claims and passed down from controller → service → repository.

## Authentication

- JWT Bearer tokens; all controllers have `[Authorize]`
- Login endpoints: `/user/SMS/login` and `/user/Fees/login`
- Password encryption: AES-256 via `CommonService.Encrypt()`
- Token claims: `UserName`, `SysId`, `InstitutionCode`, `LoginType`, `Guid`, `IsPrincipal`

## Adding a New Feature — Checklist

1. Create `Libraries/Repository/XxxRepository/IXxxRepo.cs` (interface)
2. Create `Libraries/Repository/XxxRepository/XxxRepo.cs` (implementation)
3. Register in `Libraries/Repository/DependencyInjection.cs`
4. Create `Libraries/Services/XxxServices/IXxxService.cs` (interface)
5. Create `Libraries/Services/XxxServices/XxxService.cs` (implementation)
6. Register in `Libraries/Services/DependencyInjection.cs`
7. Add Request/Response DTOs in `Libraries/Models/XxxModels/`
8. Create `SchoolManagementAPI/Controllers/XxxController.cs`

## EF Core & Database

- SQL Server via `SchoolManagementContext : DbContext`
- All timestamps default to `GETDATE()` via `HasDefaultValueSql("(getdate())")`
- Status columns default to `'Active'` via `HasDefaultValueSql("('Active')")`
- No cascade deletes — use `OnDelete(DeleteBehavior.ClientSetNull)`
- Audit triggers: `.ToTable(tb => tb.HasTrigger("XxxAudit"))`
- Database views (keyless): `.HasNoKey().ToView("XxxView")`

## Error Handling

- `ErrorLoggingMiddleware` logs unhandled exceptions to `ErrorLogs/yyyy-MM-dd.log`
- Services catch exceptions and return `CommonResponse` with `Status.Failed` + error message
- Never throw exceptions out of service methods

## Document Library

Generic file attachment system using `Fkid` (int) + `TableName` (string) to link binary files to any entity. Always go through `IDocumentLibraryServices` — never write to `DocumentLibrary` directly from a controller or other service.

## Layer-Specific Guides

- [Models.md](Models.md) — DTOs, request/response objects, model conventions
- [Repository.md](Repository.md) — repository patterns, DbContext, EF Core usage
- [Services.md](Services.md) — service patterns, business logic conventions