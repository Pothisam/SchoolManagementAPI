using Models.ClassSectionModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ClassSectionRepository
{
    public interface IClassSectionRepo
    {
        Task<List<ClassSection>> GetSectionsAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails);
        Task<bool> InsertAsync(ClassSection entity);
        Task<bool> ActivateSectionAsync(int sysId, APIRequestDetails apiRequestDetails);
        Task<bool> InactivateSectionAsync(int sysId, APIRequestDetails apiRequestDetails);
    }
}
