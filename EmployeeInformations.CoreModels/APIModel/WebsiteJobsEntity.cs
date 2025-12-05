using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_Jobs")]
    public class WebsiteJobsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobId { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string? Designation { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
        public string? JobType { get; set; }
        public int NoOfPositions { get; set; }
        public string Location { get; set; }
        public string JobSummary { get; set; }
        public DateTime JobPostedDate { get; set; }
        public string SkillName { get; set; }
        public string? RelevantExperience { get; set; }
        public int IsDeleted { get; set; } = 0;
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}