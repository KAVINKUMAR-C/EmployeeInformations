using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels.DataViewModel;

namespace EmployeeInformations.Model.TimesheetSummaryViewModel
{
    public class TimeSheet
    {
        public int TimeSheetId { get; set; }
        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TaskName { get; set; }
        public int Status { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentFilePath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string TimeSheetStartTime { get; set; }
        public string TimeSheetEndTime { get; set; }
        public string? TimeSheetActionName { get; set; }
        public string? TimeSheetViewImage { get; set; }
        public DateTime Startdate { get; set; }
        public List<ProjectNames>? ProjectNames { get; set; }
        public TimeSheetStatus TimeSheetStatus { get; set; }
        public string ProjectName { get; set; }
        // Datetime iisue
        public string StrStartdate { get; set; }
        public string ClassName { get; set; }
        public string splitdocname { get; set; }
        public string EmployeeName { get; set; }
        public bool EmployeeStatus { get; set; }
        public string? ColumnName { get; set; }
        public string? ColumnDirection { get; set; }
        public int? TimeSheetCount { get; set; }
        public List<TimeSheetModel>? TimeSheetModels { get; set; }
    }

    public class ProjectNames
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }

    public class ViewTimeSheet
    {
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string Startdate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
        public string AttachmentFilePath { get; set; }
    }


   
}
