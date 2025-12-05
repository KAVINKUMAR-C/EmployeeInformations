namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class AttachmentViewModel
    {
        public int EmpId { get; set; }
        public List<Attachments>? Attachments { get; set; }
        public string? DocumentFilePath { get; set; }
        public string? AttachmentName { get; set; }

    }
}
