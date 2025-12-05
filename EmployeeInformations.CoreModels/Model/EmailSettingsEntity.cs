using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmailSettings")]
    public class EmailSettingsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailSettingId { get; set; }
        public string FromEmail { get; set; }
        public string Host { get; set; }
        public int EmailPort { get; set; }
        public string? DisplayName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
       
    }
}
