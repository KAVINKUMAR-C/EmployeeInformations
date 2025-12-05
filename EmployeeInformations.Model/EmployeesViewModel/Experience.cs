namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class Experience
    {
        public int EmpId { get; set; }
        public int ExperienceId { get; set; }
        public int CompanyId { get; set; }
        public string PreviousCompany { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfRelieving { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ExperienceName { get; set; }
        public string Document { get; set; }
        public string? ExperienceActionName { get; set; }
        public string? ExperienceViewImage { get; set; }
        public List<ExperienceAttachment> ExperienceAttachment { get; set; }

        // Datetime issue
        public string StrDateOfJoining { get; set; }
        public string StrDateOfRelieving { get; set; }
    }
    public class ExperienceAttachment
    {
        public int AttachmentId { get; set; }
        public int ExperienceId { get; set; }
        public int EmpId { get; set; }
        public string ExperienceName { get; set; }
        public string Document { get; set; }
    }

    public class ExperienceDocumentFilePath
    {
        public string ExperienceName { get; set; }
        public string Document { get; set; }
        public int EmpId { get; set; }
    }
}
