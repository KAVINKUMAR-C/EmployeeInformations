using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmployeeReleavingType")]
    public class EmployeesReleaveingTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelieveId { get; set; }
        public string ReleavingType { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
    }
}

