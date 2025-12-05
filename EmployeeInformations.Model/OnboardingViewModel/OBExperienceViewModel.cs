

namespace EmployeeInformations.Model.OnboardingViewModel
{
    public class OBExperienceViewModel
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public List<OBExperience>? Experiences { get; set; }
        public string? DocumentFilePath { get; set; }
    }
}
