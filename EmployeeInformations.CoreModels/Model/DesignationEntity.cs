using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Designation")]
    public class DesignationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
