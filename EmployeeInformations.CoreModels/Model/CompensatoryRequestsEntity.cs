using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("CompensatoryRequests")]
    public class CompensatoryRequestsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompensatoryId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public DateTime WorkedDate { get; set; }
        public string Remark { get; set; }
        public string? Reason { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public int IsApproved { get; set; }
        public int DayCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
