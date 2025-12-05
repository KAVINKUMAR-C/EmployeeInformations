using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("SendEmails")]
    public class SendEmailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailListId { get; set; }
        public int EmailSettingId { get; set; }
        public string EmailId { get; set; }
        public string? DisplayName { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
    }
}
