using EmployeeInformations.Model.LeaveSummaryViewModel;

namespace EmployeeInformations.Model.APIModel
{
    public class LeaveRequestModel
    {
        public int AppliedLeaveId { get; set; }
        public int AppliedLeaveTypeId { get; set; }
        public int EmpId { get; set; }
        public string? EmployeeUserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeName { get; set; }
        public int IsApproved { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public decimal ApprovedLeave { get; set; }
        public string? Reason { get; set; }
        public string? LeaveTypes { get; set; }
        public List<LeaveTypesAPI>? LeaveTypesAPI { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? LeaveFilePath { get; set; }
        public string? LeaveName { get; set; }
        public TimeOnly? FromTime { get; set; }
        public TimeOnly? ToTime { get; set; }
        public string? PermissionFromTime { get; set; }
        public string? PermissionToTime { get; set; }
        public string? RejectReason { get; set; }
        public int RemaingLeave { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public List<AllLeaveDetails>? AllLeaveDetails { get; set; }
        public List<LeaveCountAPI>? LeaveCountAPI { get; set; }
        public List<EmployeeTotalLeaveDetails>? EmployeeTotalLeaveDetails { get; set; }
        public decimal CasualLeaveRemaining { get; set; }
        public decimal SickLeaveRemaining { get; set; }
        public decimal EarnedLeaveRemaining { get; set; }
        public decimal MaternityLeaveRemaining { get; set; }
        public decimal CompensatoryOffRemaining { get; set; }
        public string? EmployeeProfileImage { get; set; }
        public string? ClassName { get; set; }

        // Datetime Issue
        public string? StrLeaveFromDate { get; set; }
        public string? StrLeaveToDate { get; set; }
        public string? splitLeaveName { get; set; }

        //base64
        public string? Base64string { get; set; }
        public string? FileFormat { get; set; }
        public int CompanyId { get; set; }
    }

    public class updateLeaveRequestModel
    {
        public int AppliedLeaveId { get; set; }
        public int AppliedLeaveTypeId { get; set; }
        public int EmpId { get; set; }
        public string Reason { get; set; }

        public string StrLeaveFromDate { get; set; }
        public string StrLeaveToDate { get; set; }

        public string? Base64string { get; set; }
        public string? FileFormat { get; set; }
    }

    public class LeaveCountAPI
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
    }

    public class LeaveTypesAPI
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }

    }
}
