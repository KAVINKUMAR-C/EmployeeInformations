namespace EmployeeInformations.CoreModels
{
    public class LeaveReportDateModel
    {
        public Int32 Id { get; set; }
        public string EmployeeUserId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal LeaveCount { get; set; }
        public string LeaveType { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public string Reason { get; set; }
        public int AppliedLeaveTypeId { get; set; }
    }
}
