using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Experience")]
    public class ExperienceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExperienceId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public virtual EmployeesEntity Employees { get; set; }
        public string PreviousCompany { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfRelieving { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
