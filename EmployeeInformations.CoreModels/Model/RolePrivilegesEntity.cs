using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("RolePrivileges")]
    public class RolePrivilegesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public byte RoleId { get; set; }
        public int SubModuleId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsWritable { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CompanyId { get; set; }
    }
}
