using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
      [Keyless]
      public class EmailSchedulerViewModel
      {
            public int SchedulerId { get; set; }
            public int CompanyId { get; set; }
            public int DurationId  { get; set; }
            public string Durations { get; set; }
            public string ReportName { get; set; }
            public int FileFormat { get; set; }
            public string?FileFormatStatus { get; set; }
            public bool IsActive { get; set; }
           public string? IsActiveStatus { get; set; }
            public DateTime MailTime { get; set; }
           
      }
      public class EmailSchedulerFilterCount
      {
            public int Id { get; set; }
      }
    
}
