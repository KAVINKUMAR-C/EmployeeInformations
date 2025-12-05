using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_CandidateMenu")]
    public class WebsiteCandidateMenuEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidateMenuId { get; set; }
        public int JobApplyId { get; set; }
        public int JobId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Experience { get; set; }
        public string? FilePath { get; set; }
        public string? SkillSet { get; set;}
        public string? RelevantExperience { get;set; }
        public DateTime? ApplyDate { get; set; }
        public string? Attachment { get;set; }
        public string? IsRecordForm { get; set;}
        public int? CandidateStatusId { get; set; }
        public int? CandidateScheduleId { get; set;}
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? EmployeeName { get; set; }
        public string? MeetingLink {  get; set; }
    }
}
