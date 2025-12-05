using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Salary")]
    public class SalaryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryId { get; set; }
        public decimal Amount { get; set; }
        public int EmpId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }      
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
