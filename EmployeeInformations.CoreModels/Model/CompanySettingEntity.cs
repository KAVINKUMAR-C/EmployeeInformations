using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("CompanySetting")]
    public class CompanySettingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanySettingId { get; set; }
        public int CompanyId { get; set; }
        public int ModeId { get; set; }
        public string TimeZone { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string GSTNumber { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsTimeLockEnable { get; set; }
        public string? CompanyCode { get; set; }
    }
}
