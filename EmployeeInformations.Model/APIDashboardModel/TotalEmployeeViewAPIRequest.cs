using EmployeeInformations.Common.Enums;

namespace EmployeeInformations.Model.APIDashboardModel
{

    public class DashboardViewModelAPI
    {
        public TotalEmployeeViewAPIRequest totalEmployeeViewAPIRequest { get; set; }
        public TotalProjectViewAPIRequest totalProjectViewAPIRequest { get; set; }
        public TotalClientViewAPIRequest totalClientViewAPIRequest { get; set; }
        public TopFiveDepartmentsViewAPI topFiveDepartmentsViewAPI { get; set; }
        public TopFiveProjectsViewAPI topFiveProjectsViewAPI { get; set; }
        public TopFiveLeaveViewAPI topFiveLeaveViewAPI { get; set; }
        public TopFiveCelebrationViewAPI topFiveCelebrationViewAPI { get; set; }
        public TopFiveAnnuncementViewAPI topFiveAnnuncementViewAPI { get; set; }
        public TopFiveLeaveWfhViewAPI topFiveLeaveWfhViewAPI { get; set; }
        public TotalEmployeeLeaveViewAPI totalEmployeeLeaveViewAPI { get; set; }
        public TopFiveLeaveTypeViewAPI topFiveLeaveTypeViewAPI { get; set; }
        public EmployeeLeaveCountAPI employeeLeaveCountAPI { get; set; }
        public TimeLogViewAPI timeLogViewAPI { get; set; }
        public EmployeeWorkingHoursViewAPI employeeWorkingHoursViewAPI { get; set; }
    }

    public class TotalEmployeeViewAPIRequest
    {
        public int TotalEmployeeCount { get; set; }
        public string TotalEmployeeCountPercentage { get; set; }
        public string Months { get; set; }
        public string EmployeeByMonthCount { get; set; }
        public List<EmployeesListAPI> Employees { get; set; }
    }

    public class EmployeesListAPI
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

    }


    public class TotalProjectViewAPIRequest
    {
        public int TotalProjectCount { get; set; }
        public string TotalProjectCountPercentage { get; set; }
        public string ProjectMonths { get; set; }
        public string ProjectByMonthCount { get; set; }
    }

    public class TotalClientViewAPIRequest
    {
        public int TotalClientCount { get; set; }
        public string TotalClientCountPercentage { get; set; }
        public string ClientMonths { get; set; }
        public string ClientByMonthCount { get; set; }
    }

    public class TopFiveDepartmentsViewAPI
    {
        public List<DepartmentListAPI> departmentListAPI { get; set; }
    }

    public class DepartmentListAPI
    {
        public string DepartmentColor { get; set; }
        public int DepartmentCount { get; set; }
        public string DepartmentName { get; set; }
    }

    public class TopFiveProjectsViewAPI
    {
        public List<ProjectListAPI> ProjectLists { get; set; }
        public string StringProjectName { get; set; }
        public string ProjectByEmployeeCount { get; set; }
    }

    public class ProjectListAPI
    {
        public string ProjectColor { get; set; }
        public string ProjectName { get; set; }
    }

    public class TopFiveLeaveViewAPI
    {
        public List<LeaveListAPI> leaveListAPIs { get; set; }
    }

    public class LeaveListAPI
    {
        public string Title { get; set; }
        public string leaveColor { get; set; }
        public string LeaveDay { get; set; }
        public DateTime LeaveDate { get; set; }
    }
    public class TopFiveCelebrationViewAPI
    {
        public List<CelebrationAPI> Celebration { get; set; }
    }

    public class CelebrationAPI
    {
        public string CelebrationName { get; set; }
        public string CelebrationDate { get; set; }
        public string EmployeeName { get; set; }
        public string CelebrationColor { get; set; }
    }

    public class TopFiveAnnuncementViewAPI
    {
        public List<AnnouncementsAPI> Announcement { get; set; }
    }

    public class AnnouncementsAPI
    {
        public int AnnuncementId { get; set; }
        public string AnnouncementName { get; set; }
        public string Description { get; set; }
        public string AnnouncementColor { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string AnnouncerName { get; set; }
        public string AnnouncementDate { get; set; }
    }

    public class TopFiveLeaveWfhViewAPI
    {
        public List<LeaveAPI> Leave { get; set; }
    }

    public class LeaveAPI
    {
        public DateTime LeaveDate { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string LeaveColor { get; set; }
        public int LeaveApproved { get; set; }
    }

    public class TotalEmployeeLeaveViewAPI
    {
        public string TotalEmployeePresentCount { get; set; }
        public string TotalEmployeeAbsentCount { get; set; }
        public int TodayPresentEmployeeCount { get; set; }
        public int TotalEmployeeCount { get; set; }
        public string Months { get; set; }
        public string EmployeeByDayCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TotalEmployeeLeavesAPI> TotalEmployeeLeaves { get; set; }
    }

    public class TotalEmployeeLeavesAPI
    {
        public DateTime Date { get; set; }
        public int TotalEmployeePresentCount { get; set; }
        public int TotalEmployeeAbsentCount { get; set; }
        public int TodayPresentEmployeeCount { get; set; }
        public string Months { get; set; }
    }

    public class TopFiveLeaveTypeViewAPI
    {
        public List<LeaveAPI> Leave { get; set; }
    }

    public class EmployeeLeaveCountAPI
    {
        public decimal RemaingLeave { get; set; }
        public List<LeaveCountEmployeeAPI> LeaveCount { get; set; }
        public List<LeaveTypesEmployeeAPI>? LeaveTypes { get; set; }
    }

    public class LeaveTypesEmployeeAPI
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class LeaveCountEmployeeAPI
    {
        public string LeaveType { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal AppliedLeave { get; set; }
        public decimal ApprovedLeave { get; set; }
        public decimal RemaingLeave { get; set; }
        public decimal SumOfAppliedLeaveAndApprovedLeave { get; set; }
    }

    public class TimeLogViewAPI
    {
        public string EntryStatus { get; set; }
        public string TimeOngoingTime { get; set; }
        public int TimeClockPercentage { get; set; }
        public bool TodayClockIn { get; set; }
    }

    public class EmployeeWorkingHoursViewAPI
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string TotalHours { get; set; }
        public int TotalSecounds { get; set; }
    }
}
