namespace EmployeeInformations.Model.CompanyPolicyViewModel
{
    public class ManualLog
    {
        public int Sno { get; set; }
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string EntryStatus { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
    }
}
