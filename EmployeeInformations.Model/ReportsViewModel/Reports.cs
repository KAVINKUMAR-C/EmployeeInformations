using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.MasterViewModel;

namespace EmployeeInformations.Model.ReportsViewModel
{
    public class Reports
    {
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveFromDate { get; set; }
        public string LeaveToDate { get; set; }
         public int EmployeeStatus { get; set; }
        public List<Designation> Designations { get; set; }
        public List<Department> Departments { get; set; }
        public List<EmployeeDropdown> reportingPeople { get; set; }
        public List<LeaveTypes> LeaveTypes { get; set; }
        public List<FilterViewEmployeeLeave> employeeAppliedLeaves { get; set; }
        public int FromDatecol { get; set; }
        public string? FromOrder { get; set;}
    }

    public class EmployeeDropdown
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeIdWithName { get; set; }
    }
    public class Years
    {
        public int Year { get; set; }
        public string StrYear { get; set; }
    }

    public class ManualLogReports
    {
        public int Sno { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string EntryStatus { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public string EmployeeName { get; set; }
        public List<EmployeeDropdown> ReportingPeople { get; set; }
        public List<FilterViewManualLog>? ManualLog { get; set; }
    }

    public class FilterViewManualLog
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

    public class TimeSheetReports
    {
        public int TimeSheetId { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        // public DateTime StartDate { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int Status { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFilePath { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeName { get; set; }
        public List<EmployeeDropdown> ReportingPeople { get; set; }
        public List<FilterViewTimeSheet>? FilterViewTimeSheet { get; set; }
        public List<ProjectNames> ProjectNames { get; set; }
        public List<TimeSheet> timeSheets { get; set; }
        public List<TimeSheetReportViewModel> timeSheetReport { get; set; }

        public string EndDate { get; set; }
        public int StartDatecol { get; set; }
        public string? StartDateOrder { get; set; }
    }

    public class DropdownProjects
    {
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public List<ProjectNames> ProjectNames { get; set; }
    }
    public class FilterViewTimeSheet
    {
        public Int32 Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int Status { get; set; }
        public string StrStatus { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentFilePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeUserId { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public int CompanyId { get; set; }  

    }

    public class ProjectNames
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }


    public class TimeSheetReportsDataModel
    {
        public int TimeSheetId { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        // public DateTime StartDate { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int Status { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFilePath { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeName { get; set; }
        public string EndDate { get; set; }

    }

    public class TimeSheet
    {
        public int TimeSheetId { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        // public DateTime StartDate { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int Status { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFilePath { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeName { get; set; }
        public string EndDate { get; set; }
    }

}
