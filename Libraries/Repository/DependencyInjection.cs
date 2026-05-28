using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.AcademicYearRepository;
using Repository.ClassRepository;
using Repository.ClassSectionRepository;
using Repository.CommonRepository;
using Repository.DashboardRepository;
using Repository.DocumentLibraryRepository;
using Repository.Entity;
using Repository.FeesTypeRepository;
using Repository.InstitutionDetails;
using Repository.ReportRepository;
using Repository.StaffRepository;
using Repository.StudentFeesTransactionRepository;
using Repository.StudentRepository;
using Repository.UserRepository;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection RepositoryDependencyInjection(this IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<SchoolManagementContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            service.AddScoped<IInstitutionDetailsRepo, InstitutionDetailsRepo>();
            service.AddScoped<IUserRepo, UserRepo>();
            service.AddScoped<ICommonRepo, CommonRepo>();
            service.AddScoped<IClassRepo, ClassRepo>();
            service.AddScoped<IClassSectionRepo, ClassSectionRepo>();
            service.AddScoped<IAcademicYearRepo, AcademicYearRepo>();
            service.AddScoped<IStaffRepo, StaffRepo>();
            service.AddScoped<IDocumentLibraryRepo, DocumentLibraryRepo>();
            service.AddScoped<IStudentRepo, StudentRepo>();
            service.AddScoped<IReportRepo, ReportRepo>();
            service.AddScoped<IFeesTypeRepo, FeesTypeRepo>();
            service.AddScoped<IStudentFeesTransactionRepo, StudentFeesTransactionRepo>();
            service.AddScoped<IDashboardRepo, DashboardRepo>();
            return service;
        }
    }
}
