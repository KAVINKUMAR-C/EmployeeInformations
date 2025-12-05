using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("OtherDetails")]
    public class OtherDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public virtual EmployeesEntity Employees { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        //public string DocumentName { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        //public string Document { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? UANNumber { get; set; }
    }
}
