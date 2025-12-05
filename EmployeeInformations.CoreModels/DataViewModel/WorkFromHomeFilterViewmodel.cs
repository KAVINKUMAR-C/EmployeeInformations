
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class WorkFromHomeFilterViewmodel
    {
        public int AppliedLeaveTypeId { get; set; }
        public int AppliedLeaveId { get; set; }
        public int EmpId { get; set; }
        public int IsApproved { get; set; }
        public string? LeaveType { get; set; }
        public decimal LeaveApplied { get; set; }
        public DateTime? LeaveFromDate { get; set; }
        public DateTime? LeaveToDate { get; set; }
        public string? Reason { get; set; }
        public string? LeaveFilePath { get; set; }
        public string? LeaveName { get; set; }
        public string? UserName { get; set; }     
        public string? EmployeeName { get; set; }
        public string? ProfileFilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LeaveStatus { get; set; }
        public bool? EmployeeStatus { get; set; }
    }
    public class WorkFromHomeFilterCount
    {
        public int Id { get; set; }
    }
}
