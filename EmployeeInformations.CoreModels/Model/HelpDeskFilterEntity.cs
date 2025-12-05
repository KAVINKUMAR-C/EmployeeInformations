using Microsoft.EntityFrameworkCore;
namespace EmployeeInformations.CoreModels.Model
{
    [Keyless]
    public class HelpDeskFilterEntity
    {

        public int Id { get; set; }
        public int EmpId { get; set; }
        public string? EmployeeName { get; set; }
        public int TicketTypeId { get; set; }
        public string? TicketType { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public string? TicketStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? AttachmentName { get; set; }


    }

    [Keyless]
    public class HelpDeskCount
    {
        public int EmployeeCount { get; set; }
    }

}
