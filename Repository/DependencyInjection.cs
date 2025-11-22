using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entity;
using Repository.InstitutionDetails;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection RepositoryDependencyInjection(this IServiceCollection service, IConfiguration config)
        {
            service.AddScoped<IInstitutionDetailsRepo, InstitutionDetailsRepo>();
            service.AddDbContext<SchoolManagementContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            return service;
        }
    }
}
