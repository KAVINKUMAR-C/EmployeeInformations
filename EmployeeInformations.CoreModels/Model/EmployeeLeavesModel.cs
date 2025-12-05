using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.Model
{
    [Keyless]
    public class EmployeeLeavesModel
    {       
        public int AppliedLeaveId { get; set; }
        public int AppliedLeaveTypeId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeUserName { get; set; }
        public string EmployeeName { get; set; }
        public int IsApproved { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public string Reason { get; set; }
        public string LeaveTypes { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }       
        public string? LeaveFilePath { get; set; }
        public string? LeaveName { get; set; }        
        public string? EmployeeProfileImage { get; set; }        
        public bool EmployeeStatus { get; set; }
        public string? ApprovalStatus { get; set; }


    }
}
