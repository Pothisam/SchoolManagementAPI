using Models.ClassSectionModels;
using Models.CommonModels;
using Repository.ClassSectionRepository;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ClassSectionServices
{
    public class ClassSectionService : IClassSectionService
    {
        private readonly IClassSectionRepo _classSectionRepo;

        public ClassSectionService(IClassSectionRepo classSectionRepo)
        {
            _classSectionRepo = classSectionRepo;
        }
        public async Task<CommonResponse<string>> AddClassSectionAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails)
        {
            var sections = await _classSectionRepo.GetSectionsAsync(request,apiRequestDetails);

            // Active section letters
            var activeLetters = sections
                .Where(x => x.Status == "Active")
                .Select(x => x.SectionName.ToUpper())
                .ToList();

            char nextLetter = activeLetters.Any()? (char)(activeLetters.Max()[0] + 1): 'A';

            if (nextLetter > 'O')
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Maximum section limit (A–O) reached"
                };
            }

            // Check if section already exists as InActive
            var inactiveSection = sections.FirstOrDefault(x => x.SectionName == nextLetter.ToString() && x.Status == "InActive");

            if (inactiveSection != null)
            {
                var activated = await _classSectionRepo.ActivateSectionAsync(
                    inactiveSection.SysId,
                    apiRequestDetails);

                return new CommonResponse<string>
                {
                    Status = activated ? Status.Success : Status.Failed,
                    Message = activated
                        ? $"Section {nextLetter} activated successfully"
                        : "Unable to activate section"
                };
            }

            // Insert new
            var entity = new ClassSection
            {
                ClassFkid = request.ClassFkid,
                SectionName = nextLetter.ToString(),
                Status = "Active",
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName
            };

            var inserted = await _classSectionRepo.InsertAsync(entity);

            return new CommonResponse<string>
            {
                Status = inserted ? Status.Success : Status.Failed,
                Message = inserted
                    ? $"Section {nextLetter} added successfully"
                    : "Unable to add section"
            };
        }

        public async Task<CommonResponse<List<ClassSectionResponse>>> GetActiveSectionsAsync(ClassSectionRequest request,APIRequestDetails apiRequestDetails)
        {
            var result = await _classSectionRepo.GetActiveSectionsAsync(request,apiRequestDetails);

            return new CommonResponse<List<ClassSectionResponse>>
            {
                Status = Status.Success,
                Data = result
            };
        }

        public async Task<CommonResponse<string>> RemoveLastSectionAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails)
        {
            var sections = await _classSectionRepo.GetSectionsAsync(request, apiRequestDetails);

            var activeSections = sections.Where(x => x.Status == "Active").OrderByDescending(x => x.SysId).ToList();
            // SP rule: minimum one section (A) must exist
            if (activeSections.Count <= 1)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Minimum one section (A) must exist"
                };
            }
            var lastSection = activeSections.First(); // MAX(SysId)

            var result = await _classSectionRepo.InactivateSectionAsync(lastSection.SysId,apiRequestDetails);
            return new CommonResponse<string>
            {
                Status = result ? Status.Success : Status.Failed,
                Message = result ? $"Section {lastSection.SectionName} removed successfully" : "Unable to remove section"
            };
        }
    }
}
