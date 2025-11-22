using Microsoft.EntityFrameworkCore;
using Models.InstitutionDetailsModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InstitutionDetails
{
    public class InstitutionDetailsRepo : IInstitutionDetailsRepo
    {
        private readonly SchoolManagementContext _context;
        public InstitutionDetailsRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<InstitutionDetailsResponse> GetInstitutionDetail(int InstitutionCode)
        {
            InstitutionDetailsResponse? Result = await (from x in _context.InstitutionDetails
                                                 where x.Sysid == InstitutionCode
                                                 select new InstitutionDetailsResponse
                                                 {
                                                     InstitutionName = x.InstitutionName,
                                                     Address1 = x.Address1,
                                                     Address2 = x.Address2,
                                                     Pincode = x.Pincode,
                                                     PostofficeName = x.PostofficeName,
                                                     Districtname = x.Districtname,
                                                     StateName = x.StateName,
                                                     Emailid = x.Emailid,
                                                     MobileNumer = x.MobileNumer,
                                                     Website = x.Website,
                                                     AlternateMobileNumer = x.AlternateMobileNumer,
                                                     InstitutionType = x.InstitutionType,
                                                     Landline = x.Landline,
                                                     StaffIdprefix = x.StaffIdprefix,
                                                     EnteredBy = x.EnteredBy,
                                                     ModifiedBy = x.ModifiedBy,
                                                     ModifiedDate = x.ModifiedDate
                                                 }).FirstOrDefaultAsync();
            return Result;
        }
    }
}
