namespace EmployeeInformations.Model.DashboardViewModel
{
    public class TimeLoggerViewModel
    {
        public Int32 CompanyId { get; set; }
        public Int32 EmployeeId { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string EntryStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public long LogSeconds { get; set; }
        public string? Reason { get; set; }
    }
}
