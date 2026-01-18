using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.ReportModels;
using Repository.Entity;

namespace Repository.ReportRepository
{
    public class ReportRepo : IReportRepo
    {
        private readonly SchoolManagementContext _context;
        public ReportRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<List<StudentTrasferResponse>> GetStudentPromotionListAsync(StudentTrasferRequest request, APIRequestDetails apiRequestDetails)
        {
            return await
        (
            from scdFrom in _context.StudentClassDetails

            join sd in _context.StudentDetails
                on scdFrom.StudentDetailsFkid equals sd.SysId

            join ayFrom in _context.AcademicYears
                on scdFrom.AcademicYearFkid equals ayFrom.SysId

            join csFrom in _context.ClassSections
                on scdFrom.ClassSectionFkid equals csFrom.SysId

            join cFrom in _context.Classes
                on csFrom.ClassFkid equals cFrom.SysId

            // LEFT JOIN StudentClassDetails (TO)
            join scdToTemp in _context.StudentClassDetails
                .Where(x =>
                    x.AcademicYearFkid == request.AcademicYearTo &&
                    x.InstitutionCode == apiRequestDetails.InstitutionCode)
                on scdFrom.StudentDetailsFkid equals scdToTemp.StudentDetailsFkid
                into scdToGroup
            from scdTo in scdToGroup.DefaultIfEmpty()

                // LEFT JOIN ClassSection (TO)
            join csToTemp in _context.ClassSections
                on scdTo.ClassSectionFkid equals csToTemp.SysId
                into csToGroup
            from csTo in csToGroup.DefaultIfEmpty()

                // LEFT JOIN Class (TO)
            join cToTemp in _context.Classes
                on csTo.ClassFkid equals cToTemp.SysId
                into cToGroup
            from cTo in cToGroup.DefaultIfEmpty()

                // LEFT JOIN AcademicYear (TO – constant)
            join ayToTemp in _context.AcademicYears
                on request.AcademicYearTo equals ayToTemp.SysId
                into ayToGroup
            from ayTo in ayToGroup.DefaultIfEmpty()

            where scdFrom.AcademicYearFkid == request.AcademicYearFrom
               && scdFrom.ClassSectionFkid == request.ClassSectionFrom
               && sd.InstitutionCode == apiRequestDetails.InstitutionCode

            select new StudentTrasferResponse
            {
                StudentId = sd.SysId,
                StudentName = sd.Name + " " + sd.Initial,
                DOB = sd.Dob,

                FromAcademicYear = ayFrom.Year,
                FromClassName = cFrom.ClassName,
                FromSectionName = csFrom.SectionName,

                ToAcademicYear = ayTo.Year,

                ExistingClassName = cTo.ClassName,
                ExistingSectionName = csTo.SectionName,

                AlreadyExisting = scdTo != null ? 1 : 0
            }).ToListAsync();
        }
    }
}
