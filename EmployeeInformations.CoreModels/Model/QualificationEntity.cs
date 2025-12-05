using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Qualification")]
    public class QualificationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QualificationId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public virtual EmployeesEntity Employees { get; set; }
        public string QualificationType { get; set; }
        public string Percentage { get; set; }
        public int YearOfPassing { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string InstitutionName { get; set; }
    }
}
