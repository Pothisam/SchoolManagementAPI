using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Services.AcademicYearServices;
using Services.ClassSectionServices;
using Services.ClassServices;
using Services.CommonServices;
using Services.DocumentLibraryServices;
using Services.InstitutionDetailsServices;
using Services.ReportServices;
using Services.StaffServices;
using Services.StudentServices;
using Services.UserServices;

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
            service.AddScoped<IClassService, ClassService>();
            service.AddScoped<IClassSectionService, ClassSectionService>();
            service.AddScoped<IAcademicYearService, AcademicYearService>();
            service.AddScoped<IStaffService, StaffService>();
            service.AddScoped<IDocumentLibraryServices, DocumentLibraryService>();
            service.AddScoped<IStudentService, StudentService>();
            service.AddScoped<IReportService, ReportService>();
            return service;
        }
    }
}
