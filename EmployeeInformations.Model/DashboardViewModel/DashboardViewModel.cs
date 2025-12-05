using EmployeeInformations.Common.Enums;

namespace EmployeeInformations.Model.DashboardViewModel
{
    public class DashboardViewModel
    {
        public TotalEmployeeView TotalEmployeeView { get; set; }
        public TotalProjectView TotalProjectView { get; set; }
        public TotalClientView TotalClientView { get; set; }
        public TotalActiveEmployeesView TotalActiveEmployeesView { get; set; }
        public TopFiveProjectsView TopFiveProjectsView { get; set; }
        public TopFiveDepartmentsView TopFiveDepartmentsView { get; set; }
        public TopFiveLeaveView TopFiveLeaveView { get; set; }
        public TimeLogView TimeLogViewModel { get; set; }
        public TopFiveCelebrationView TopFiveCelebrationView { get; set; }
        public TopFiveLeaveTypeView TopFiveLeaveTypeView { get; set; }
        public TopFiveAnnuncementView TopFiveAnnuncementView { get; set; }
        public TopFiveLeaveWfhView TopFiveLeaveWfhView { get; set; }
        public TotalEmployeeLeaveView TotalEmployeeLeaveView { get; set; }
        public EmployeeLeaveCount EmployeeLeaveCount { get; set; }
        public EmployeeWorkingHoursView EmployeeWorkingHoursView { get; set; }

        public List<TopFiveLeaveViews> TopFiveLeaveViews { get; set; }

        public List<Announcements> Announcements { get; set; }
        public List<LeaveList> LeaveLists { get; set; }
        public List<Celebration> Celebrations { get; set; }
        public List<EmployeesList> EmployeesList { get; set; }
        public List<DepartmentList> DepartmentLists { get; set; }
    }

    public class TotalEmployeeView
    {
        public int TotalEmployeeCount { get; set; }
        public string TotalEmployeeCountPercentage { get; set; }
        public string Months { get; set; }
        public string EmployeeByMonthCount { get; set; }
        public List<EmployeesList> Employees { get; set; }
    }

    public class TotalProjectView
    {
        public int TotalProjectCount { get; set; }
        public string TotalProjectCountPercentage { get; set; }
        public string ProjectMonths { get; set; }
        public string ProjectByMonthCount { get; set; }
    }

    public class TotalClientView
    {
        public int TotalClientCount { get; set; }
        public string TotalClientCountPercentage { get; set; }
        public string ClientMonths { get; set; }
        public string ClientByMonthCount { get; set; }
    }
    public class TotalActiveEmployeesView
    {
        public int TotalActiveEmployeesCount { get; set; }
        public string TotalActiveEmployeeCountPercentage { get; set; }
        public string ActiveEmployeesMonths { get; set; }
        public string ActiveEmployeesByMonthCount { get; set; }
    }

    public class TopFiveProjectsView
    {
        public List<ProjectList> ProjectLists { get; set; }
        public string StringProjectName { get; set; }
        public string ProjectByEmployeeCount { get; set; }
    }

    public class ProjectList
    {
        public string ProjectColor { get; set; }
        public string ProjectName { get; set; }
    }

    public class TopFiveDepartmentsView
    {
        public List<DepartmentList> DepartmentLists { get; set; }
    }

    public class DepartmentList
    {
        public string DepartmentColor { get; set; }
        public int DepartmentCount { get; set; }
        public string DepartmentName { get; set; }
    }

    public class EmployeesList
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficeEmail { get; set; }
        public int DesignationId { get; set; }
        public Role RoleId { get; set; }
        public int DepartmentId { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeSortName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public DateTime? JoingDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string EmployeeFullName { get; set; }
        public bool EmployeeStatus { get; set; }

    }

    public class TopFiveLeaveView
    {
        public List<LeaveList> LeaveLists { get; set; }
    }

    public class LeaveList
    {
        public string Title { get; set; }
        public string leaveColor { get; set; }
        public string LeaveDay { get; set; }
        public DateTime LeaveDate { get; set; }
    }

    public class TimeLogView
    {
        public int? EmpId { get; set; }
        public string EntryStatus { get; set; }
        public string TimeOngoingTime { get; set; }
        public int TimeClockPercentage { get; set; }
        public bool TodayClockIn { get; set; }
        public string? TotalHours { get; set; }
        public string? InsideOffice { get; set; }
        public string? BreakHours { get; set; }
    }

    public class TopFiveCelebrationView
    {
        public List<Celebration> Celebration { get; set; }
    }

    public class Celebration
    {
        public int EmpId { get; set; }
        public string CelebrationName { get; set; }
        public string CelebrationDate { get; set; }
        public string EmployeeName { get; set; }
        public bool EmployeeStatus { get; set; }
        public string CelebrationColor { get; set; }       
    }

    public class TopFiveLeaveTypeView
    {
        public List<Leave> Leave { get; set; }
    }

    public class TopFiveAnnuncementView
    {
        public List<Announcements> Announcement { get; set; }
    }

    public class TopFiveLeaveWfhView
    {
        public List<Leave> Leave { get; set; }
    }


    public class TopFiveLeaveViews
    {
        public int EmpId { get; set; }
        public DateTime LeaveDate { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string LeaveColor { get; set; }
        public bool EmployeeStatus { get; set; }
        public int LeaveApproved { get; set; }
    }

    public class Leave
    {
        public int EmpId { get; set; }
        public DateTime LeaveDate { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string LeaveColor { get; set; }
        public bool EmployeeStatus { get; set; }
        public int LeaveApproved { get; set; }
    }

    public class Announcements
    {
        public int AnnuncementId { get; set; }
        public string AnnouncementName { get; set; }
        public string Description { get; set; }
        public string AnnouncementColor { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AnnouncementDate { get; set; }
        public string AnnouncementEndDate { get; set; }
        public int CreatedBy { get; set; }
        public string AnnouncerName { get; set; }
    }

    public class TotalEmployeeLeaveView
    {
        public string TotalEmployeePresentCount { get; set; }
        public string TotalEmployeeAbsentCount { get; set; }
        public int TodayPresentEmployeeCount { get; set; }
        public int TotalEmployeeCount { get; set; }
        public string Months { get; set; }
        public string EmployeeByDayCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TotalEmployeeLeaves> TotalEmployeeLeaves { get; set; }
    }

    public class EmployeeWorkingHoursView
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string TotalHours { get; set; }
        public int TotalSecounds { get; set; }
        public int TotalDayHours { get; set; }
    }


    public class EmployeeLeaveCount
    {
        //public int AnnualLeave { get; set; }
        //public int SickLeave { get; set; }
        //public int CasualLeave { get; set; }
        public decimal RemaingLeave { get; set; }
        public List<LeaveCountEmployee> LeaveCount { get; set; }
        public List<LeaveTypesEmployee>? LeaveTypes { get; set; }
    }

    public class LeaveTypesEmployee
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class LeaveCountEmployee
    {
        public string LeaveType { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public decimal ApprovedLeave { get; set; }
        public decimal RemaingLeave { get; set; }
        public decimal SumOfAppliedLeaveAndApprovedLeave { get; set; }
    }

    public class TotalEmployeeLeaves
    {
        public DateTime Date { get; set; }
        public int TotalEmployeePresentCount { get; set; }
        public int TotalEmployeeAbsentCount { get; set; }
        public int TodayPresentEmployeeCount { get; set; }
        public string Months { get; set; }
    }

}

