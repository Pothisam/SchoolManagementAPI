using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Services.InstitutionDetailsServices;
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
            return service;
        }
    }
}
