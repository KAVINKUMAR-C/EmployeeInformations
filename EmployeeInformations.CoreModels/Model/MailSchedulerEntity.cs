using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("MailScheduler")]
    public class MailSchedulerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchedulerId { get; set; }
        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public int FileFormat { get; set; }
        public string ReportName { get; set; }
        public string WhomToSend { get; set; }
        public string MailSendingDays { get; set; }
        public int DurationId { get; set; }
        public DateTime MailTime { get; set; }
        public int EmailDraftId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
