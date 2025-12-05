namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class Qualification
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
        public List<QualificationAttachment>? QualificationAttachments { get; set; }
    }

    public class QualificationViewModel
    {
        public int EmpId { get; set; }
        public List<Qualification>? Qualifications { get; set; }
    }

    public class QualificationAttachment
    {
        public int AttachmentId { get; set; }
        public int QualificationId { get; set; }
        public int EmpId { get; set; }
        public string QualificationName { get; set; }
        public string Document { get; set; }
    }

    public class QualificationDocumentFilePath
    {
        public string QualificationName { get; set; }
        public string Document { get; set; }
        public int EmpId { get; set; }
    }

}
