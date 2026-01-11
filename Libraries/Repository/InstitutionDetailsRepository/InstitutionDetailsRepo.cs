using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
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
        public async Task<bool> AddInstitutionAsync(AddInstitutionRequest request, APIRequestDetails apiRequestDetails)
        {
            var inst = new InstitutionDetail
            {
                InstitutionName = request.InstitutionName,
                Emailid = request.Emailid,
                OfficialMail = request.OfficialMail,
                Address1 = request.Address1,
                Address2 = request.Address2,
                MobileNumer = request.MobileNumer,
                AlternateMobileNumer = request.AlternateMobileNumer,
                Website = request.Website,
                Landline = request.Landline,
                Pincode = request.Pincode,
                PostofficeName = request.PostofficeName,
                Districtname = request.Districtname,
                StateName = request.StateName,
                InstitutionType = request.InstitutionType,
                StaffIdprefix = request.StaffIdprefix,
                EnteredBy = apiRequestDetails.UserName
            };

            _context.InstitutionDetails.Add(inst);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<InstitutionDetail?> GetInstitutionByNameAsync(string institutionName, int institutionCode)
        {
            return await _context.InstitutionDetails
            .FirstOrDefaultAsync(x =>
                x.InstitutionName == institutionName &&
                x.Sysid == institutionCode);
        }

       

        public async Task<InstitutionDetail?> GetInstitutionByIdAsync(int sysid)
        {
            return await _context.InstitutionDetails
           .FirstOrDefaultAsync(x => x.Sysid == sysid);
        }

        public async Task<bool> UpdateInstitutionAsync(UpdateInstitutionRequest request, APIRequestDetails apiRequestDetails)
        {
            var inst = await _context.InstitutionDetails
            .FirstOrDefaultAsync(x => x.Sysid == apiRequestDetails.InstitutionCode);

            if (inst == null)
                return false;

            // Assign updated fields
            inst.InstitutionName = request.InstitutionName;
            inst.Emailid = request.Emailid;
            inst.OfficialMail = request.OfficialMail;
            inst.Address1 = request.Address1;
            inst.Address2 = request.Address2;
            inst.MobileNumer = request.MobileNumer;
            inst.AlternateMobileNumer = request.AlternateMobileNumer;
            inst.Website = request.Website;
            inst.Landline = request.Landline;
            inst.Pincode = request.Pincode;
            inst.PostofficeName = request.PostofficeName;
            inst.Districtname = request.Districtname;
            inst.StateName = request.StateName;
            inst.InstitutionType = request.InstitutionType;
            inst.StaffIdprefix = request.StaffIdprefix;

            inst.ModifiedBy = apiRequestDetails.UserName;
            inst.ModifiedDate = DateTime.Now;

            _context.InstitutionDetails.Update(inst);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateInstitutionLogoAsync(UpdateInstitutionLogoRequest request, APIRequestDetails apiRequestDetails)
        {
            var inst = await _context.InstitutionDetails
        .FirstOrDefaultAsync(x => x.Sysid == request.Sysid);

            if (inst == null)
                return false;

            inst.LogoFileName = request.LogoFileName;
            inst.LogoContentType = request.LogoContentType;
            inst.LogoData = Convert.FromBase64String(request.LogoData.Split(',')[1].Trim());

            inst.ModifiedBy = apiRequestDetails.UserName;
            _context.InstitutionDetails.Update(inst);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateInstitutionFaviconAsync(UpdateInstitutionFaviconRequest request, APIRequestDetails apiRequestDetails)
        {
            var inst = await _context.InstitutionDetails
        .FirstOrDefaultAsync(x => x.Sysid == request.Sysid);

            if (inst == null)
                return false;

            inst.FaviconFileName = request.FaviconFileName;
            inst.FaviconContentType = request.FaviconContentType;
            inst.FaviconData = Convert.FromBase64String(request.FaviconData.Split(',')[1].Trim());

            inst.ModifiedBy = apiRequestDetails.UserName;
            _context.InstitutionDetails.Update(inst);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateInstitutionLogoWithTextAsync(UpdateInstitutionLogoWithTextRequest request, APIRequestDetails apiRequestDetails)
        {
            var inst = await _context.InstitutionDetails
        .FirstOrDefaultAsync(x => x.Sysid == request.Sysid);

            if (inst == null)
                return false;

            inst.LogoWithTextFileName = request.LogoWithTextFileName;
            inst.LogoWithTextContentType = request.LogoWithTextContentType;
            inst.LogoWithTextData = Convert.FromBase64String(request.LogoWithTextData.Split(',')[1].Trim());

            inst.ModifiedBy = apiRequestDetails.UserName;

            _context.InstitutionDetails.Update(inst);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
