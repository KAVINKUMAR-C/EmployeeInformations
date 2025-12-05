


namespace EmployeeInformations.Model.OnboardingViewModel
{
     public class OBQualificationViewModel
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public List<OBQulification>? Qualifications { get; set; }
        public string? DocumentFilePath { get; set; }
        public string? QualificationActionName { get; set; }
        public string? QualificationName { get; set; }
        public string? QualificationViewImage { get; set; }
        public List<OBQualificationAttachment>? QualificationAttachmentViewModel { get; set; }
    }
}
