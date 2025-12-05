

namespace EmployeeInformations.Model.OnboardingViewModel
{
     public class OBQulification
    {
        public int QualificationId { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string QualificationType { get; set; }
        public string Percentage { get; set; }
        public int YearOfPassing { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? QualificationActionName { get; set; }
        public string? QualificationViewImage { get; set; }
        public string InstitutionName { get; set; }
        public List<OBQualificationAttachment>? QualificationAttachments { get; set; }
    }
    public class OBQualificationAttachment
    {
        public int AttachmentId { get; set; }
        public int QualificationId { get; set; }
        public int EmpId { get; set; }
        public string QualificationName { get; set; }
        public string Document { get; set; }
    }
    public class OBQualificationDocumentFilePath
    {
        public string QualificationName { get; set; }
        public string Document { get; set; }
        public int EmpId { get; set; }
    }


}
