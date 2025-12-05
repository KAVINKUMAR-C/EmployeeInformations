using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmployeesLog")]
    public class EmployeesLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeesLogId { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string? SectionName { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string Event { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
