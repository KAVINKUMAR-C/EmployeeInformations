using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_JobApply")]
    public class WebsiteJobApplyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobApplyId { get; set; }
        public int JobId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? SkillSet { get; set; }
        public string? RelevantExperience { get; set; }
        public string? Experience { get; set; }
        public string? FilePath { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string? Attachment { get; set; }
        public bool? IsActive { get; set; }
        public string? IsRecordForm { get; set; }
    }
}
