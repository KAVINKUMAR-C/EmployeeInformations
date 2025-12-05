

using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;

namespace EmployeeInformations.Model.APIModel
{
   public class WebsiteCandidateMenuModel
   {
        public int CandidateMenuId { get; set; }
        public int JobApplyId { get; set; }
        public int JobId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Experience { get; set; }
        public string? FilePath { get; set; }
        public string? SkillSet { get; set; }
        public string? RelevantExperience { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string? Attachment { get; set; }
        public string? IsRecordForm { get; set; }
        public string? JobName { get; set; }
        public bool? IsActive  { get; set; }
        public string SortFullName { get; set; }
        public int? CandidateStatusId { get; set; }
        public int? CandidateScheduleId { get; set; }
        public string? CandidateScheduleName { get; set; }
        public List<WebsiteCandidateMenu> WebsiteCandidateMenus { get; set; }
        public List<WebsiteCandidateSchedule> WebsiteCandidateSchedules { get; set; }
        public List<WebsiteCandidateSchedule> WebsiteCandidateSchedulesById { get; set; }
        public DateTime StatusDate { get; set; }
        public string CandidateStatusName { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string StrScheduledDate { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public List<DropdownEmployees> DropdownEmployee { get; set; }
        public string EmployeeId { get; set; }
        public string Employees { get; set; }
        public string? EmployeeName { get; set; }       
        public int Duration { get; set; }   
        public string? MeetingLink { get; set; }
        public List<WebsiteCandidateMenusModel> websiteCandidateMenuModels { get; set; }


   }

    public class WebsiteCandidateMenu
    {
        public int CandidateMenuId { get; set; }
        public int JobApplyId { get; set; }
        public int JobId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Experience { get; set; }
        public string? FilePath { get; set; }
        public string? SkillSet { get; set; }
        public string? RelevantExperience { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string? Attachment { get; set; }
        public string? IsRecordForm { get; set; }
        public string? JobName { get; set; }
        public bool? IsActive { get; set; }
        public string SortFullName { get; set; }
        public string CandidateStatusName { get; set; }
        public int? CandidateStatusId { get; set; }
        public string? MeetingLink { get; set; }
        public string File {  get; set; }
        public int? CandidateScheduleId { get; set; }
        public string? CandidateScheduleName { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string StrScheduledDate { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public string? EmployeeName { get; set; }
        public string? Employees { get; set; }

    }

    public class DropdownEmployees
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmpName { get; set; }
        public string UserName { get; set; }

    }
}
