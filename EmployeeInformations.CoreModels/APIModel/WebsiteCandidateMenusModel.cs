using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Keyless]
    public class WebsiteCandidateMenusModel
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
        public string? SortFullName { get; set; }
        public int? CandidateStatusId { get; set; }
        public int? CandidateScheduleId { get; set; }
        public string? CandidateScheduleName { get; set; }
        public string? CandidateStatusName { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Employees { get; set; }
        public string? EmployeeName { get; set; }
        public string? MeetingLink { get; set; }
    }
    [Keyless]
    public class WebsiteCandidateMenuModelCount
    {
        public int EmployeeCount { get; set; }
    }
}
