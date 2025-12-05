namespace EmployeeInformations.CoreModels.DataViewModel
{
    public class ManualLogReportDateModel
    {
        public Int32 Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EntryStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeUserId { get; set; }
    }
}
