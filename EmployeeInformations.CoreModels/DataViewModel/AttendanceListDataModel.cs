namespace EmployeeInformations.CoreModels.DataViewModel
{
    public class AttendanceListDataModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string TotalHours { get; set; }
        public string InsideOffice { get; set; }
        public string BreakHours { get; set; }
        public string BurningHours { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public long TotalSecounds { get; set; }
        public string OfficeEmail { get; set; }
    }
}
