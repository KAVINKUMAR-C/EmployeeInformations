using EmployeeInformations.Common.Enums;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Model.AttendanceViewModel
{
    public class AttendaceListViewModel
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
        public string LogTime { get; set; }
        public string Direction { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public long TotalSecounds { get; set; }
        public string OfficeEmail { get; set; }
        public List<EmployeeDropdown> reportingPeople { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int EmployeeStatus { get; set; }
        public bool Status { get; set; }
        public int CompanyId { get; set; }
        public int EsslId { get; set; }

    }

    public class AttendanceStatus
    {
        public string EmployeeName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string EmployeeUserName { set; get; }
        public string EmployeeShortName { get; set; }
        public bool Status { get; set; }
        public int EmployeeId { get; set; }
        public string ClassName { get; set; }
        public List<string> Date { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public string BurningHours { get; set; }
        public string InsideOffice { get; set; }
        public List<EntryStatusDate> EntryStatusDate { get; set; }
        public List<int> count { get; set; }
        public string PresentCount { get; set; }
    }
    public class EntryStatusDate
    {
        public bool EntryStatus { get; set; }
        public string Date { get; set; }
    }
    public class AttendaceListViewModels
    {
        public int EmployeeId { get; set; }
        public Role RoleId { get; set; }
        public string EmployeeName { get; set; }
        public string Date { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public string BurningHours { get; set; }
        public string InsideOffice { get; set; }
        public int Month { get; set; }
        public long TotalSecounds { get; set; }
        public int EmployeeStatus { get; set; }
        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }
        public int Year { get; set; }
        public List<int> Dates { get; set; }
        public string LogDate { get; set; }
        public string EndDate { get; set; }
        public List<ListOfDate> ListOfDate { get; set; }
        public List<AttendaceListViewModel> AttendaceListViewModel { get; set; }
        public List<EmployeeDropdown> reportingPeople { get; set; }
        public List<Years> Years { get; set; }
        public List<FilterViewAttendace> FilterViewAttendace { get; set; }
        public List<ViewAttendanceLog> ViewAttendanceLog { get; set; }
        public List<AttendanceStatus> AttendanceStatuses { get; set; }

    }
    public class FilterViewAttendace
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        //public DateTime StartDate { get; set; }
        public string LogDateTime { get; set; }
        //public DateTime EndDate { get; set; }
        public string Direction { get; set; }
        public string LogDate { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string EmployeeUserId { get; set; }
    }

    public class ListOfDate
    {
        public bool Holidays { get; set; }
        public int Dates { get; set; }
    }

    public class AttendanceLogExcept
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string? LogDateTime { get; set; }
        public string? LogDate { get; set; }
        public string? LogTime { get; set; }
        public string? Direction { get; set; }
    }

    public class ViewAttendanceLog
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public int Employee { get; set; }
        public string EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public int? EmpId { get; set; }
        public string? LogDateTime { get; set; }
        public string? LogDate { get; set; }
        public string? LogTime { get; set; }
        public string? Direction { get; set; }
        public string? TotalHours { get; set; }
        public string? InsideOffice { get; set; }
        public string? BreakHours { get; set; }
        public string? BurningHours { get; set; }
        public string? EntryTime { get; set; }
        public string? ExitTime { get; set; }
        public int WorkingHours { get; set; }
        public bool Status { get; set; }    
    }
}