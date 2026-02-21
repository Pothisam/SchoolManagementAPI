using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.FeesTypeModels;
using Repository.Entity;

namespace Repository.FeesTypeRepository
{
    public interface IFeesTypeRepo
    {
        Task<List<FeesTypeListResponse>> GetFeesTypeListAsync(APIRequestDetails apiRequestDetails);
        Task<int> GetFeesTypeSysIdByNameAsync(SaveFeesTypeRequest request, APIRequestDetails apiRequestDetails);
        Task AddFeesTypeAsync(FeesType entity);
        Task<FeesType> GetFeesTypeBySysIdAsync(int sysId, APIRequestDetails apiRequestDetails);

        Task SaveChangesAsync();
        Task<List<StudentFeeGenerateStatusResponse>> GetFeesListViewAsync(GetFeesGentrationRequest request, APIRequestDetails apiRequestDetails);
    }
}
