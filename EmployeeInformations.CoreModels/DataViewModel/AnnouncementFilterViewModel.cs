using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class AnnouncementFilterViewModel
    {
        public int AnnouncementId { get; set; }
        public string? AnnouncementName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public int? AnnouncementAssignee { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public DateTime AnnouncementEndDate { get; set; }
        public string? FilePath { get; set; }
        public string? AttachmentName { get; set; }
        public string? Assignee { get; set; }
        public string? ActiveStatus { get; set; }

    }
    public class AnnouncementFilterCount
    {
        public int Id {  get; set; }
    }
}
