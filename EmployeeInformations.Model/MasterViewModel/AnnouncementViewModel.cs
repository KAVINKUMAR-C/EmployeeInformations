using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Model.MasterViewModel
{
    public class AnnouncementViewModel
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementName { get; set; }
        public string Description { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public DateTime AnnouncementEndDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DesignationsId { get; set; }
        public int? DepartmentsId { get; set; }
        public int AnnouncementAssignee { get; set; }
        public List<Announcement> Announcement { get; set; }
        public List<EmployeeDropdown>? ReportingPeople { get; set; }
        public List<Designation> Designation { get; set; }
        public List<Department> Department { get; set; }
        public string? DocumentFilePath { get; set; }
    }

    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementName { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public DateTime AnnouncementEndDate { get;set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public int AnnouncementNameCount { get; set; }
        public string strAnnouncementDate { get; set; }
        public string strAnnouncementEndDate { get; set; }
        public int AnnouncementAssignee { get; set; }
        public string? EmpId { get; set; }
        public string? DesignationId { get; set; }
        public string? DepartmentId { get; set; }
        public string AssigneeName { get; set; }
        public string? Filepath { get; set; }    
        public List<AnnouncementAttachmentsViewModel> announcementAttachments { get; set; }
    }
}
