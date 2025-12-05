using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("LeaveTypes")]
    public class LeaveTypesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
