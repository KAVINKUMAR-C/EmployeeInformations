namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class Attachments
    {
        public int AttachmentId { get; set; }
        public int EmpId { get; set; }
        public string? AttachmentName { get; set; }
        public string? Document { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
