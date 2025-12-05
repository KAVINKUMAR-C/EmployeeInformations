using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("ProjectTypes")]
    public class ProjectTypesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
    }
}
