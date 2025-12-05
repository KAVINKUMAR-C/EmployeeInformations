using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Model.LeaveSummaryViewModel
{
    public class EmployeeLeaveViewModel
    {
        //public int LeaveId { get; set; }
        public int AppliedLeaveId { get; set; }
        public int AppliedLeaveTypeId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public int IsApproved { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public decimal ApprovedLeave { get; set; }

        public bool IsHalfDay { get; set; }
        public string Reason { get; set; }
        public string LeaveTypes { get; set; }
        public List<LeaveTypes>? LeaveType { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? LeaveFilePath { get; set; }
        public string? LeaveName { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public string PermissionFromTime { get; set; }
        public string PermissionToTime { get; set; }
        public string RejectReason { get; set; }
        public decimal RemaingLeave { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public List<AllLeaveDetails> AllLeaveDetails { get; set; }
        public List<LeaveCount> LeaveCounts { get; set; }
        public List<EmployeeTotalLeaveDetails> EmployeeTotalLeaveDetails { get; set; }
        public decimal CasualLeaveRemaining { get; set; }
        public decimal SickLeaveRemaining { get; set; }
        public decimal EarnedLeaveRemaining { get; set; }
        public decimal MaternityLeaveRemaining { get; set; }
        public decimal CompensatoryOffRemaining { get; set; }
        public decimal ApprovedLOP { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public decimal LeaveCount { get; set; }
        public bool EmployeeStatus { get; set; }
        public List<WorkFromHomeFilterViewmodel> wfhFilter { get; set; }

        // Datetime Issue
        public string StrLeaveFromDate { get; set; }
        public string StrLeaveToDate { get; set; }
        public string splitLeaveName { get; set; }

        public List<LeaveDetails> leaveDetails { get; set; }

        public List<LeaveFilterViewModel> leaveFilterViewModels { get; set; }
      
        public List<LeaveTypesEntity>? LeavesType { get; set; }
        public List<EmployeeAppliedLeaveEntity>? EmployeeAppliedLeave { get; set; }
        public List<AllLeaveDetailsEntity>? AllLeaveDetail { get; set; }
        public string UserName { get; set; }
		public List<EmployeeDropdown>? reportingPeople { get; set; }
		public int EmployeeId { get; set; }
        public List<EmployeePrivileges>? EmployeePrivileges { get; set; }
    }

    public class EmployeeTotalLeaveDetails
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string? EmployeeName { get; set; }
        public decimal CasualLeaveRemaining { get; set; }
        public decimal SickLeaveRemaining { get; set; }
        public decimal EarnedLeaveRemaining { get; set; }
        public decimal MaternityLeaveRemaining { get; set; }
        public decimal CompensatoryOffRemaining { get; set; }
        public decimal ApprovedLeave { get; set; }
        public decimal RemaingLeave { get; set; }
        public decimal ApprovedLOP { get; set; }

    }
    public class LeaveCount
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public decimal ApprovedLeave { get; set; }
        public decimal RemaingLeave { get; set; }
        public decimal SumOfAppliedLeaveAndApprovedLeave { get; set; }
        public decimal CasualLeaveRemaining { get; set; }
        public decimal SickLeaveRemaining { get; set; }
        public decimal EarnedLeaveRemaining { get; set; }
        public decimal MaternityLeaveRemaining { get; set; }
        public decimal CompensatoryOffRemaining { get; set; }
        public decimal ApprovedLOP {  get; set; }
    }

    public class ViewEmployeeLeave
    {
        public int AppliedLeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal AppliedDayCount { get; set; }
        public string LeaveFromDate { get; set; }
        public string LeaveToDate { get; set; }
        public string Reason { get; set; }
        public string LeaveAppliedDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string RejectReason { get; set; }
        public string LeaveFileName { get; set; }
        public string Status { get; set; }
        public string EmployeeUserName { get; set; }
        public decimal LeaveCount { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveTypes { get; set; }
        public int IsApprove { get; set; }
    }

    public class LeaveFilterViewModel
    {
        public int EmpId { get; set; }
        public string EmployeeUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string Reason { get; set; }
        public string LeaveTypes { get; set; }
        public decimal LeaveCount { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }



    }
    public class LeaveDetails
    {
        public int AppliedLeaveId { get; set; }
        public int AppliedLeaveTypeId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public int IsApproved { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public decimal ApprovedLeave { get; set; }
        public string Reason { get; set; }
        public string LeaveTypes { get; set; }

        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? LeaveFilePath { get; set; }
        public string? LeaveName { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public string PermissionFromTime { get; set; }
        public string PermissionToTime { get; set; }
        public string RejectReason { get; set; }
        public decimal RemaingLeave { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public decimal CasualLeaveRemaining { get; set; }
        public decimal SickLeaveRemaining { get; set; }
        public decimal EarnedLeaveRemaining { get; set; }
        public decimal MaternityLeaveRemaining { get; set; }
        public decimal CompensatoryOffRemaining { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public decimal LeaveCount { get; set; }

        // Datetime Issue
        public string StrLeaveFromDate { get; set; }
        public string StrLeaveToDate { get; set; }
        public string splitLeaveName { get; set; }
    }
}


