using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.AcademicYearRepository;
using Repository.ClassRepository;
using Repository.ClassSectionRepository;
using Repository.CommonRepository;
using Repository.Entity;
using Repository.InstitutionDetails;
using Repository.UserRepository;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection RepositoryDependencyInjection(this IServiceCollection service, IConfiguration config)
        {
            service.AddScoped<IInstitutionDetailsRepo, InstitutionDetailsRepo>();
            service.AddScoped<IUserRepo, UserRepo>();
            service.AddScoped<ICommonRepo, CommonRepo>();
            service.AddScoped<IClassRepo, ClassRepo>();
            service.AddScoped<IClassSectionRepo, ClassSectionRepo>();
            service.AddScoped<IAcademicYearRepo, AcademicYearRepo>();
            service.AddDbContext<SchoolManagementContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            return service;
        }
    }
}
