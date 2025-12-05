using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmailQueue")]
    public class EmailQueueEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailQueueID { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSend { get; set; } = false;
        public int CompanyId { get; set; }
        public string? Reason { get; set; }
        public string? DisplayName { get; set; }
        public string? Attachments { get; set; }
        public string? CCEmail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}