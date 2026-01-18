using Models.ClassSectionModels;
using Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ClassSectionServices
{
    public interface IClassSectionService
    {
        Task<CommonResponse<string>> AddClassSectionAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> RemoveLastSectionAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<ClassSectionResponse>>> GetActiveSectionsAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<ClassAndSectionResponse>>> GetClassAndSectionsAsync(APIRequestDetails apiRequestDetails);
    }
}
