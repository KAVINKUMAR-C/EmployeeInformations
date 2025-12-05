namespace EmployeeInformations.Model.LeaveSummaryViewModel
{
    public class AllLeaveDetails
    {
        public int AllLeaveDetailId { get; set; }
        public int LeaveYear { get; set; }
        public int EmpId { get; set; }
        public decimal CasualLeaveCount { get; set; }
        public decimal SickLeaveCount { get; set; }
        public decimal EarnedLeaveCount { get; set; }
        public decimal MaternityLeaveCount { get; set; }
        public decimal CasualLeaveTaken { get; set; }
        public decimal SickLeaveTaken { get; set; }
        public decimal EarnedLeaveTaken { get; set; }
        public decimal MaternityLeaveTaken { get; set; }

    }
}
