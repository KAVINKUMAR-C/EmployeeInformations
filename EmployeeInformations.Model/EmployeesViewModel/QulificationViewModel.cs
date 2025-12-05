namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class QulificationViewModel
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public List<Qualification>? Qualifications { get; set; }
        public string? DocumentFilePath { get; set; }
        public string? QualificationActionName { get; set; }
        public string? QualificationName { get; set; }
        public string? QualificationViewImage { get; set; }
        public List<QualificationAttachment>? QualificationAttachmentViewModel { get; set; }
    }
}

