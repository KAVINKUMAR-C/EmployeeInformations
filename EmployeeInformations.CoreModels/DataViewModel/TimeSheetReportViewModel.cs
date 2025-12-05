

using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class TimeSheetReportViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Status { get; set; }
        public int ProjectId { get; set; }
        public string TaskDescription { get; set; }
        public string TaskName { get; set; }
        public string ProjectName { get; set; }
        public string? AttachmentFilePath { get; set; }
        public string? AttachmentFileName { get; set; }
        public string EmployeeUserId { get; set; }                                                                  
        public string EmployeeName { get; set; }                                                                  
        public int CompanyId { get; set; }
        public string TimeSheetStatus { get; set; }
    }

    public class TimeSheetFilterCount
    {
        public int Id { get; set; }
    }
}
