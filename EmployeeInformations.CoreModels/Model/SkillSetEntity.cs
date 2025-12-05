using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("SkillSets")]
    public class SkillSetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
