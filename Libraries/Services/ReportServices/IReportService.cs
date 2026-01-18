using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.ReportModels;

namespace Services.ReportServices
{
    public interface IReportService
    {
        Task<CommonResponse<List<StudentTrasferResponse>>> GetStudentPromotionListAsync(StudentTrasferRequest request,APIRequestDetails apiRequestDetails);
    }
}
