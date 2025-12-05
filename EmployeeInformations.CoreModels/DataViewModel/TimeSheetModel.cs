

using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
   public class TimeSheetModel
    {
        
     public int TimeSheetId { get; set; }
     public int? ProjectId { get; set; }
     public int? EmpId { get; set; }
     public string EmployeeName { get; set; }
     public string ProjectName { get; set; }
     public string TaskName { get; set; }
     public string TaskDescription { get; set; }
     public DateTime StartDate { get; set; }
     public DateTime StartTime { get; set; }
     public DateTime EndTime { get; set; }
     public string? AttachmentFileName { get; set; }
     public int? Status { get; set; }   
     public DateTime CreatedDate { get; set; }
     public string? TimesheetStatus { get; set; }
    }
}

