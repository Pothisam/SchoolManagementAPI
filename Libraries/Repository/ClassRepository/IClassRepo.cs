using Models.ClassModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ClassRepository
{
    public interface IClassRepo
    {
        Task<bool> IsClassExistsAsync(AddClassRequest request, APIRequestDetails apiRequestDetails);
        Task<bool> AddClassAsync(AddClassRequest request, APIRequestDetails apiRequestDetails);
        Task<List<ClassResponse>> GetClassListAsync(APIRequestDetails apiRequestDetails);
        Task<Class?> GetClassByIdAsync(UpdateClassStatusRequest request,APIRequestDetails apiRequestDetails);
        Task<bool> UpdateClassAsync(Class entity);
    }
}
