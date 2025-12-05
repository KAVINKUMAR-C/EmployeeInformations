using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.MasterViewModel;


namespace EmployeeInformations.Model.HelpdeskViewModel
{
    public class HelpdeskViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int TicketTypeId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string? Attachment {  get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string? DocumentFilePath { get; set; }
        public List<TicketTypes>? TicketTypes { get; set; }
        public List<Helpdesk>? Helpdesks { get; set; }  
        public List<TicketAttachments>? TicketAttachments { get; set; }
        public List<HelpDeskFilterEntity> helpDeskEntities { get; set; }
    }

    public class Helpdesk
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketType { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public string TicketStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set;}
        public int? UpdatedBy { get; set;}
        public bool IsDeleted { get; set; }
        public string? DocumentFilePath { get; set; }
        public List<TicketAttachments>? TicketAttachments { get; set; }
        public List<TicketTypes>? TicketTypes { get; set; }
    }

    public class TicketAttachments
    {
        public int Id { get; set; }
        public int HelpdeskId { get; set; }
        public string AttachmentName { get; set; } 
        public string Document {  get; set; }
        public bool IsDeleted { get; set; } 
    }
}
