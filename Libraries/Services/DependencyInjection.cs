using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Services.CommonServices;
using Services.InstitutionDetailsServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesDependencyInjection(this IServiceCollection service, IConfiguration config)
        {
            service.RepositoryDependencyInjection(config);
            service.AddScoped<IInstitutionDetailsService, InstitutionDetailsService>();
            service.AddScoped<ICommonService, CommonService>();
            service.AddScoped<IUserService,UserService>();
            return service;
        }
    }
}
