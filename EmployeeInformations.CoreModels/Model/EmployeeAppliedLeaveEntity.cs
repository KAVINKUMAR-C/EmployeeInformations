using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmployeeAppliedLeave")]
    public class EmployeeAppliedLeaveEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppliedLeaveId { get; set; }
        public int AppliedLeaveTypeId { get; set; }
        public int EmpId { get; set; }
        public decimal LeaveApplied { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public string Reason { get; set; }
        public int IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public string? LeaveName { get; set; }
        public string? LeaveFilePath { get; set; }
        public string? RejectReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int CompanyId { get; set; }
    }
}
